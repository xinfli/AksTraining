param managedClusters_xfakseuwe01_name string = 'xfakseuwe01'
param publicIPAddresses_81b62c03_a126_4765_a382_c4905aecb1a3_externalid string = '/subscriptions/ee6c4145-f2fc-4366-bca4-1a38775414f3/resourceGroups/MC_xfakseuwe01_xfakseuwe01_westeurope/providers/Microsoft.Network/publicIPAddresses/81b62c03-a126-4765-a382-c4905aecb1a3'

resource managedClusters_xfakseuwe01_name_resource 'Microsoft.ContainerService/managedClusters@2021-02-01' = {
  name: managedClusters_xfakseuwe01_name
  location: 'westeurope'
  sku: {
    name: 'Basic'
    tier: 'Free'
  }
  properties: {
    kubernetesVersion: '1.18.14'
    dnsPrefix: 'xfakseuwe0-${managedClusters_xfakseuwe01_name}-ee6c41'
    agentPoolProfiles: [
      {
        name: 'nodepool1'
        count: 1
        vmSize: 'Standard_DS2_v2'
        osDiskSizeGB: 128
        osDiskType: 'Managed'
        kubeletDiskType: 'OS'
        maxPods: 110
        type: 'VirtualMachineScaleSets'
        orchestratorVersion: '1.18.14'
        enableNodePublicIP: false
        nodeLabels: {}
        mode: 'System'
        osType: 'Linux'
      }
    ]
    linuxProfile: {
      adminUsername: 'azureuser'
      ssh: {
        publicKeys: [
          {
            keyData: 'ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABAQC/RQmvrbFLRRRc2do33T+fhiusDgx4gCfAtLWr3MWjVvM94KWhhtLreTDvubOpjd1GGN253RpLSCTM/t9sgZZ8SaPOtteLdJeSSb71V7kQitZJIhgT2iK6ZTLDpDhBZ3zqeTqRDjEdG3O39T/Fdy2xb3lphTjfk0cGlqUVit++oowLIUeKjrztox5HHBsCpZZdgPYEhsSo/GOTBl8Brp5z4P8nsI5GE/KPL5pQeR0voQSHf0X7A2CEcRb2442gFEJWBcK0uKFfd165q07G1nqJSgh+w80UZnfQEz74ZXizBZUqC4KjhaObjxRVkdPQOR8Un+QskuCMs36QidABW7IR'
          }
        ]
      }
    }
    servicePrincipalProfile: {
      clientId: 'e4dfc1e3-0641-459e-a42a-93c02435a085'
    }
    addonProfiles: {
      kubeDashboard: {
        enabled: true
      }
    }
    nodeResourceGroup: 'MC_${managedClusters_xfakseuwe01_name}_${managedClusters_xfakseuwe01_name}_westeurope'
    enableRBAC: true
    networkProfile: {
      networkPlugin: 'kubenet'
      loadBalancerSku: 'standard'
      loadBalancerProfile: {
        managedOutboundIPs: {
          count: 1
        }
        effectiveOutboundIPs: [
          {
            id: publicIPAddresses_81b62c03_a126_4765_a382_c4905aecb1a3_externalid
          }
        ]
      }
      podCidr: '10.244.0.0/16'
      serviceCidr: '10.0.0.0/16'
      dnsServiceIP: '10.0.0.10'
      dockerBridgeCidr: '172.17.0.1/16'
      outboundType: 'loadBalancer'
    }
  }
}

resource managedClusters_xfakseuwe01_name_nodepool1 'Microsoft.ContainerService/managedClusters/agentPools@2021-02-01' = {
  name: '${managedClusters_xfakseuwe01_name_resource.name}/nodepool1'
  properties: {
    count: 1
    vmSize: 'Standard_DS2_v2'
    osDiskSizeGB: 128
    osDiskType: 'Managed'
    kubeletDiskType: 'OS'
    maxPods: 110
    type: 'VirtualMachineScaleSets'
    orchestratorVersion: '1.18.14'
    enableNodePublicIP: false
    nodeLabels: {}
    mode: 'System'
    osType: 'Linux'
  }
}