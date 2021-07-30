using HeatedIdon.DTO;
using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HeatedIdon.Models;
using System.Collections.Generic;

namespace HeatedIdon.Helpers.Impl
{
    public class MqttSubscriper : IMqttSubscriper
    {
        private readonly IConfiguration _config;
        private string _mqttServer;
        private string _mqttUser;
        private string _mqttPassword;
        private string _mqttTopic;
        private readonly string _clientId = "HeatedIdonService";
        private readonly FalconConsumer _falconConsumer;
        private bool _connected;
        private SensorWorker _sensorWorker;

        public MqttSubscriper(IConfiguration config, FalconConsumer falconConsumer)
        {
            _config = config;
            _falconConsumer = falconConsumer;
            _sensorWorker = new SensorWorker();
            _ = InitializeMqttClient();
        }

        private async Task InitializeMqttClient()
        {
            _mqttServer = _config["RabbitMQ:mqttServer"];
            _mqttUser = _config["RabbitMQ:mqttUser"];
            _mqttPassword = _config["RabbitMQ:mqttPassword"];
            _mqttTopic = _config["RabbitMQ:mqttTopic"];
            await ConnectAsync().ConfigureAwait(false);
        }
        public bool GetConnectionStatus()
        {
            return _connected;
        }
        public List<Sensor> GetSensorInfo()
        {
            return _sensorWorker.GetSensorInfo();
        }


        public async Task ConnectAsync()
        {
            var mqttClient = new MqttFactory().CreateMqttClient();
            mqttClient.UseApplicationMessageReceivedHandler(e => HandleMessage(e.ApplicationMessage));
            mqttClient.UseDisconnectedHandler(e => HandleDisconnect(e.AuthenticateResult));
            var topicFilter = new MqttTopicFilter
            {
                Topic = _mqttTopic
            };

            var options = new MqttClientOptionsBuilder()
                .WithClientId(_clientId)
                .WithTcpServer(_mqttServer, 1883)
                .WithCredentials(_mqttUser, _mqttPassword)
                .WithCleanSession()
                .Build();

            mqttClient.ConnectAsync(options, CancellationToken.None).Wait();
            await mqttClient.SubscribeAsync(topicFilter);
            _connected = true;
        }

        private void HandleMessage(MqttApplicationMessage applicationMessage)
        {
            var payload = Encoding.UTF8.GetString(applicationMessage.Payload);
            var deSerializerOptions = new JsonSerializerOptions();
            deSerializerOptions.Converters.Add(new DoubleFromJsonConverter());
            var data = JsonSerializer.Deserialize<SensorData>(payload, deSerializerOptions);
            _sensorWorker.AddOrUpdateSensor(data);
            Task.Run(async () => await _falconConsumer.SendSensorData(data));
        }

        private void HandleDisconnect(MqttClientAuthenticateResult mqttClientAuthenticateResult)
        {
            Console.WriteLine("Got disconnection");
            var result = mqttClientAuthenticateResult;
            Console.WriteLine(result.ReasonString);
            _connected = false;
        }
    }
}
