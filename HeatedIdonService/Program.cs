using System;

namespace HeatedIdonService
{
    class Program
    {
        static void Main(string[] args)
        {
            var apploader = AppLoader.Instance;
            AppLoader.LoadConfig();
            Console.WriteLine("Config loaded");
            AppLoader.StartMqttSubscription();
        }
    }
}
