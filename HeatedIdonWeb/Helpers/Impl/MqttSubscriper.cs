using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace HeatedIdonWeb.Helpers.Impl
{
    public class MqttSubscriper : IMqttSubscriper
    {
        private IConfiguration _config;
        private string mqttServer;
        private string mqttUser;
        private string mqttPassword;
        private string mqttTopic;
        private readonly string clientId = "HeatedIdon";

        public MqttSubscriper(IConfiguration config)
        {
            _config = config;
            InitializeMqttClient();
        }

        private async Task InitializeMqttClient()
        {
            mqttServer = _config.GetValue<string>("Messaging:mqttServer");
            mqttUser = _config.GetValue<string>("Messaging:mqttUser");
            mqttPassword = _config.GetValue<string>("Messaging:mqttPassword");
            mqttTopic = _config.GetValue<string>("Messaging:mqttTopic");
            await ConnectAsync();
        }


        public async Task ConnectAsync()
        {
            var mqttClient = new MqttFactory().CreateMqttClient();
            mqttClient.UseApplicationMessageReceivedHandler(e => { HandleMessage(e.ApplicationMessage); });
            MqttTopicFilter topicFilter = new MqttTopicFilter
            {
                Topic = mqttTopic
            };

            var options = new MqttClientOptionsBuilder()
                .WithClientId(clientId)
                .WithTcpServer(mqttServer, 1883)
                .WithCredentials(mqttUser, mqttPassword)
                .WithCleanSession()
                .Build();

            mqttClient.ConnectAsync(options, CancellationToken.None).Wait();
            await mqttClient.SubscribeAsync(topicFilter);
        }

        private void HandleMessage(MqttApplicationMessage applicationMessage)
        {
            var message = applicationMessage;
            var payload = Encoding.UTF8.GetString(message.Payload);
        }
    }

}
