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
        

        public HeatedIdonControllerBase(IConfiguration configuration)
        {
            _config = configuration;
            _mqttSubscriper = new MqttSubscriper(_config);
        }
    }
}