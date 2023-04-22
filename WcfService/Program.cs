using Common;
using IdentityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WcfService
{
    internal class Program
    {
        private static ServiceHost hostSetup()
        {
            string address = "https://192.168.0.30:8003/CertificateService/";
            Uri uri = new Uri(address);
            IService service = new MessageService();

            ServiceHost host = new ServiceHost(service, uri);
            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.PeerTrust;
            //host.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new CustomCertificateValidator();

            WSHttpBinding binding = new WSHttpBinding();

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;

            host.AddServiceEndpoint(typeof(IService), binding, address);
            
            return host;
        }

        static void Main(string[] args)
        {
            ServiceHost host = hostSetup();

            try
            {
                host.Open();
                Console.WriteLine("Service host started");
                Console.WriteLine("----------------------------");
            }
            catch(InvalidOperationException e)
            {
                Console.WriteLine("Unauthorised\n");
            }

            Console.ReadLine();
        }
    }
}
