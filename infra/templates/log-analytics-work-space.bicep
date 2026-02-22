@description('Log Analytics workspace name. Must be between 4 and 64 characters long.')
@minLength(4)
@maxLength(63)
param logAnalyticsWorkspaceName string

@description('Log Analytics workspace pricing tier. Allowed values are: Free, LACluster, PerGB2018, PerNode, Premium, Standalone, Standard.')
@allowed([
  'Free'
  'LACluster'
  'PerGB2018'
  'PerNode'
  'Premium'
  'Standalone'
  'Standard'
])
param logAnalyticsWorkspaceSku string

@description('Location to deploy the resources. Defaults to the location of the resource group.')
param location string = resourceGroup().location

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2021-06-01' = {
  name: logAnalyticsWorkspaceName
  location: location
  properties: {
    sku: {
      name: logAnalyticsWorkspaceSku
    }
	retentionInDays: 30
    features: {
      enableLogAccessUsingOnlyResourcePermissions: true
    }
    workspaceCapping: {
      dailyQuotaGb: 1
    }
	publicNetworkAccessForIngestion: 'Enabled'
  publicNetworkAccessForQuery: 'Enabled'
  }
}

output logAnalyticsWorkspaceId string = logAnalyticsWorkspace.id
