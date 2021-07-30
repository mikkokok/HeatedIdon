using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace HeatedIdon.Helpers
{
    public interface ICertificateValidator
    {
        bool ValidateCertificate(HttpRequestMessage request, X509Certificate2 certificate, X509Chain certificateChain, SslPolicyErrors policy);
    }
}