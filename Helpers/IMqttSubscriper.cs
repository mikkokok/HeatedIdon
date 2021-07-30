using HeatedIdon.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HeatedIdon.Helpers
{
    public interface IMqttSubscriper
    {
        Task ConnectAsync();
        bool GetConnectionStatus();
        List<Sensor> GetSensorInfo();
    }
}