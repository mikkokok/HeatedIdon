using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using HeatedIdonWeb.DTO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace HeatedIdonWeb.Helpers.Impl
{
    public class FalconConsumer
    {
        private readonly IConfiguration _config;
        private string _falconUrl;
        private string _falconKey;


        public FalconConsumer(IConfiguration config)
        {
            _config = config;
            InitializeFalconConsumer();
        }

        private void InitializeFalconConsumer()
        {
            _falconUrl = _config.GetValue<string>("RestlessFalcon:url");
            _falconKey = _config.GetValue<string>("RestlessFalcon:key");
        }
        public async Task sendSensorData(SensorData data)
        {
            using var client = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, _falconUrl);
            var json = JsonConvert.SerializeObject(data);
            using var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            request.Content = stringContent;

            using var response = await client
                .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }
    }
}
