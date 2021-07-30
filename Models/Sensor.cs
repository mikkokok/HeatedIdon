
using System;
using HeatedIdon.Helpers.Impl;

namespace HeatedIdon.Models
{
    public class Sensor
    {
        public string SensorName { get; set; }
        public DateTime LastSeen { get; set; }
        public bool Online
        {
            get
            {
                return LastSeen <= TimeConverter.GetCurrentTime().AddMinutes(15);
            }
        }
    }
}
