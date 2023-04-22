using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    internal class MessageService : IService
    {
        public string GetMessage(string clientMessage)
        {
            Console.WriteLine("Client: " + clientMessage);
            return "Hello Client";
        }
    }
}
