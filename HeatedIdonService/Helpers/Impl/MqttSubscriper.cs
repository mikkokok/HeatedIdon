using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HeatedIdonService.Config;
using HeatedIdonService.DTO;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;

namespace HeatedIdonService.Helpers.Impl
{
    public class MqttSubscriper
    {
        private readonly ConfigData _config;
        private string _mqttServer;
        private string _mqttUser;
        private string _mqttPassword;
        private string _mqttTopic;
        private readonly string _clientId = "HeatedIdonService";
        private readonly FalconConsumer _falconConsumer;

        public MqttSubscriper(ConfigData config, FalconConsumer falconConsumer)
        {
            _config = config;
            _falconConsumer = falconConsumer;
            InitializeMqttClient();
        }

        private async Task InitializeMqttClient()
        {
            _mqttServer = _config.Messaging.mqttServer;
            _mqttUser = _config.Messaging.mqttUser;
            _mqttPassword = _config.Messaging.mqttPassword;
            _mqttTopic = _config.Messaging.mqttTopic;
            await ConnectAsync();
        }


        public async Task ConnectAsync()
        {
            var mqttClient = new MqttFactory().CreateMqttClient();
            mqttClient.UseApplicationMessageReceivedHandler(e => { HandleMessage(e.ApplicationMessage); });
            mqttClient.UseDisconnectedHandler(e => { HandleDisconnect(e.AuthenticateResult); });
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
        }

        private void HandleMessage(MqttApplicationMessage applicationMessage)
        {
            var message = applicationMessage;
            var payload = Encoding.UTF8.GetString(message.Payload);
            var deSerializerOptions = new JsonSerializerOptions();
            deSerializerOptions.Converters.Add(new DoubleFromJsonConverter());
            var data = JsonSerializer.Deserialize<SensorData>(payload, deSerializerOptions);
            Task.Run(async () =>
            {
                await _falconConsumer.SendSensorData(data);
            });
        }

        private static void HandleDisconnect(MqttClientAuthenticateResult mqttClientAuthenticateResult)
        {
            Console.WriteLine("Got disconnection");
            var result = mqttClientAuthenticateResult;
            Console.WriteLine(result.ReasonString);
        }
    }
}
