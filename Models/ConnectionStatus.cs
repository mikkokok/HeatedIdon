namespace HeatedIdon.Models
{
    public class ConnectionStatus
    {
        private bool _connStatus { get; set; }

        private string _statusText { get; set; }

        public bool ConnStatus
        {
            get { return _connStatus; }
            set
            {
                if (value)
                {
                    _connStatus = true;
                    _statusText = "Connected to RabbitMQ Topic";
                }
                else
                {
                    _connStatus = false;
                    _statusText = "Disconnected from RabbitMQ Topic";
                }
            }
        }

        public string StatusText { get { return _statusText; } }
    }
}
