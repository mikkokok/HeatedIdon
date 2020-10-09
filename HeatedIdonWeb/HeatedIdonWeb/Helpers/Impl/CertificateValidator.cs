using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace HeatedIdonWeb.Helpers.Impl
{
    public class CertificateValidator
    {
        private IConfiguration _config;
        private readonly string _trustedThumbprint;

        public CertificateValidator(IConfiguration config)
        {
            _config = config;
            _trustedThumbprint = _config.GetValue<string>("RestlessFalcon:sslThumbprint");
        }

        public bool ValidateCertificate(HttpRequestMessage request, X509Certificate2 certificate, X509Chain certificateChain, SslPolicyErrors policy)
        {
            var certificate2 = new X509Certificate2(certificate);
            if (certificate2.Thumbprint != null && certificate2.Thumbprint.Equals(_trustedThumbprint, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}
