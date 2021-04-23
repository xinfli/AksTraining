using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace AksTestFrontend.Services
{
    public class NetworkInfoService
    {
        public NetworkInfo GetNetworkInfo()
        {
            var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            return new NetworkInfo
            {
                HostName = ipGlobalProperties.HostName,
                DomainName = ipGlobalProperties.DomainName,
                NodeType = ipGlobalProperties.NodeType,
                DefaultGateway = getDefaultGateway(),
                NetworkInterfaces = getTypeAndAddress()
            };
        }


        public IEnumerable<LocalNetworkInterface> getTypeAndAddress()
        {
            var computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            var nics = NetworkInterface.GetAllNetworkInterfaces();
            Console.WriteLine("Interface information for {0}.{1}     ",
                computerProperties.HostName, computerProperties.DomainName);

            var result = nics.Select(adapter => new LocalNetworkInterface
                {
                    Description = adapter.Description,
                    Type = adapter.NetworkInterfaceType,
                    PhysicalAddress = adapter.GetPhysicalAddress(),
                    SupportsMulticast = adapter.SupportsMulticast
                })
                .ToList();

            return result;
        }

        public IPAddress getDefaultGateway()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up)
                .Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(n => n.GetIPProperties()?.GatewayAddresses)
                .Select(g => g?.Address)
                .SingleOrDefault(a => a != null);
        }
    }

    public class NetworkInfo
    {
        public string HostName { get; set; }
        public string DomainName { get; set; }
        public NetBiosNodeType NodeType { get; set; }
        public IPAddress DefaultGateway { get; set; }
        public IEnumerable<LocalNetworkInterface> NetworkInterfaces { get; set; }
    }

    public class LocalNetworkInterface
    {
        public string Description { get; set; }
        public NetworkInterfaceType Type { get; set; }
        public PhysicalAddress PhysicalAddress { get; set; }
        public bool SupportsMulticast { get; set; }
    }
}
