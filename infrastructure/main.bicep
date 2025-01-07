param location string = resourceGroup().location

var uniqueId = uniqueString(resourceGroup().id)
var keyVaultName = 'kv-${uniqueId}'
var vnetName = 'vnet-${uniqueId}'
var apiSubnetName = 'subnet-api-${uniqueId}'
var redirectApiSubnetName = 'subnet-redirect-${uniqueId}'
var tokenRangeSubnetName = 'subnet-token-range-${uniqueId}'
var cosmosTriggerSubnetName = 'subnet-cosmos-trigger-${uniqueId}'

module keyVault 'modules/secrets/keyvault.bicep' = {
  name: 'keyVaultDeployment'
  params: {
    vaultName: keyVaultName
    location: location
    subnets: [
      resourceId('Microsoft.Network/virtualNetworks/subnets', vnetName, apiSubnetName)
      resourceId('Microsoft.Network/virtualNetworks/subnets', vnetName, cosmosTriggerSubnetName)
      resourceId('Microsoft.Network/virtualNetworks/subnets', vnetName, tokenRangeSubnetName)
      resourceId('Microsoft.Network/virtualNetworks/subnets', vnetName, redirectApiSubnetName)
    ]
  }
}

module apiService 'modules/compute/appservice.bicep' = {
  name: 'apiDeployment'
  params: {
    appName: 'api-${uniqueId}'
    appServicePlanName: 'plan-api-${uniqueId}'
    location: location
  }
}
