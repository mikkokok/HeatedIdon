using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeatedIdonWeb.Helpers.Impl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HeatedIdonWeb.Controllers
{
    public class HeatedIdonControllerBase : Controller
    {
        protected IConfiguration _config;
        protected MqttSubscriper _mqttSubscriper;
        protected FalconConsumer _falconConsumer;


        public HeatedIdonControllerBase(IConfiguration configuration)
        {
            _config = configuration;
            _falconConsumer = new FalconConsumer(_config);
            _mqttSubscriper = new MqttSubscriper(_config, _falconConsumer);
        }
    }
}
