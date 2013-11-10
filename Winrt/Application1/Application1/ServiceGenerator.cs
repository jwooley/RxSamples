using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Application1
{
    public static class ServiceGenerator
    {
        public static BingTranslatorService.LanguageServiceClient GenerateBasic(Uri addressUri)
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            binding.AllowCookies = true;
            binding.MaxBufferSize = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.OpenTimeout = TimeSpan.FromSeconds(10);
            EndpointAddress address = new EndpointAddress(addressUri);
            return new BingTranslatorService.LanguageServiceClient(binding, address);
        }
    }
}
