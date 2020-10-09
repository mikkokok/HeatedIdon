using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
        private HttpClient _httpClient;
        private CertificateValidator _certificateValidator;
        
        public FalconConsumer(IConfiguration config)
        {
            _config = config;
            InitializeFalconConsumer();
        }

        private void InitializeFalconConsumer()
        {
            _falconUrl = _config.GetValue<string>("RestlessFalcon:url");
            _falconKey = _config.GetValue<string>("RestlessFalcon:key");
            _certificateValidator = new CertificateValidator(_config);
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = _certificateValidator.ValidateCertificate

            };
            _httpClient = new HttpClient(handler)
            {
                Timeout = new TimeSpan(0, 0, 30)
            };
        }
        public async Task SendSensorData(SensorData data)
        {
            var uriBuilder = new UriBuilder(_falconUrl)
            {
                Scheme = Uri.UriSchemeHttps,
                Port = 443
            };
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["authKey"] = _falconKey;
            query["sensorName"] = data.SensorName;
            uriBuilder.Query = query.ToString() ?? throw new Exception("Empty URL built");

            using var request = new HttpRequestMessage(HttpMethod.Post, uriBuilder.Uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var json = JsonConvert.SerializeObject(data);
            request.Content = new StringContent(json, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            try
            {
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
    }
}
