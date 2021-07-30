using HeatedIdon.DTO;
using System.Threading.Tasks;

namespace HeatedIdon.Helpers
{
    public interface IFalconConsumer
    {
        Task SendSensorData(SensorData data);
    }
}