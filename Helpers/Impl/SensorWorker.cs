using System.Collections.Generic;
using HeatedIdon.DTO;
using HeatedIdon.Models;

namespace HeatedIdon.Helpers.Impl
{
    public class SensorWorker
    {
        private List<Sensor> _sensors;

        public SensorWorker()
        {
            _sensors = new List<Sensor>();
        }
        public void AddOrUpdateSensor(SensorData sensor)
        {
            var update = _sensors.FindIndex(s => s.SensorName == sensor.SensorName);
            if (update == -1)
            {
                _sensors.Add(new Sensor
                {
                    SensorName = sensor.SensorName,
                    LastSeen = TimeConverter.GetCurrentTime()
                });
            }
            else
            {
                _sensors[update].LastSeen = TimeConverter.GetCurrentTime();
            }
        }
        public List<Sensor> GetSensorInfo()
        {
            return _sensors;
        }
    }
}
