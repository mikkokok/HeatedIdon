using Microsoft.AspNetCore.Mvc;
using HeatedIdon.Models;
using System.Collections.Generic;
using HeatedIdon.Helpers;

namespace HeatedIdon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private IMqttSubscriper _mqttSubscriper;
        public StatusController(IMqttSubscriper mqttSubscriper)
        {
            _mqttSubscriper = mqttSubscriper;
        }
        [HttpGet]
        public ConnectionStatus Get()
        {
            return new ConnectionStatus {
                ConnStatus = _mqttSubscriper.GetConnectionStatus()
            };
        }

        [HttpGet("~/Sensors")]
        public List<Sensor> Sensors()
        {
            return _mqttSubscriper.GetSensorInfo();
        }
    }
}
