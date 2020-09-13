using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace HeatedIdonWeb.Helpers.Impl
{
    public class FalconConsumer
    {
        private readonly IConfiguration _config;
        private string _falconUrl;
        private string _falconKey;


        public FalconConsumer(IConfiguration config)
        {
            _config = config;
            InitializeFalconConsumer();
        }

        private void InitializeFalconConsumer()
        {
            _falconUrl = _config.GetValue<string>("RestlessFalcon:url");
            _falconKey = _config.GetValue<string>("RestlessFalcon:key");
        }
    }
}
