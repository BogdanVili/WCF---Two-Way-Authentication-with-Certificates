using Common;
using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;

namespace WcfClient
{
    internal class Program
    {
        private static IService proxySetup()
        {
            var address = new EndpointAddress(new Uri("https://192.168.0.30:8003/CertificateService/"));

            WSHttpBinding binding = new WSHttpBinding();

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;

            var channel = new ChannelFactory<IService>(binding, address);

            channel.Credentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication();
            channel.Credentials.ServiceCertificate.SslCertificateAuthentication.CertificateValidationMode = X509CertificateValidationMode.PeerTrust;
            //channel.Credentials.ServiceCertificate.SslCertificateAuthentication.CustomCertificateValidator = new CustomCertificateValidator();

            channel.Credentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser,
                                                                 StoreName.My,
                                                                 X509FindType.FindBySubjectName,
                                                                 "192.168.0.39");

            var proxy = channel.CreateChannel();



            return proxy;
        }

        static void Main(string[] args)
        {
            IService proxy = proxySetup();

            try
            {
                var result = proxy.GetMessage("Hello server");

                Console.WriteLine("Client Started");
                Console.WriteLine("----------------------------");
                Console.WriteLine("Server: " + result);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("Unauthorised\n");
            }

            Console.ReadLine();
        }


    }
}
