@description('The name of the function app.')
param functionAppName string 

@description('The ID of the hosting plan for the function app.')
param hostingPlanId string 

@description('The name of the storage account for the function app.')
param storageAccountName string 

@description('The location for all resources. Defaults to the location of the resource group.')
param location string = resourceGroup().location

@description('The language worker runtime to load in the function app. Allowed values are "dotnet", "node", "python", "java", and "dotnet-isolated". Defaults to "dotnet".')
@allowed([
  'dotnet'
  'node'
  'python'
  'java'
  'dotnet-isolated'
])
param functionWorkerRuntime string = 'dotnet'

@description('The instrumentation key for Application Insights in the function app.')
@secure()
param applicationInsightsInstrumentationKey string 

@description('The kind of the function app. Allowed values are "functionapp,linux" and "functionapp".')
@allowed([
  'functionapp,linux'
  'functionapp'
])
param kind string

@description('The path to mount the file share to.')
param fileShareMountPath string = '/data001'

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-05-01' existing ={
  name: storageAccountName
}


resource functionApp 'Microsoft.Web/sites@2021-03-01' = {
  name: functionAppName
  location: location
  kind: kind
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: hostingPlanId
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTSHARE'
          value: toLower(functionAppName)
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: applicationInsightsInstrumentationKey
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: functionWorkerRuntime
        }
        {
          name: 'FileShareMountPath'
          value: fileShareMountPath
        }
      ]     
      ftpsState: 'FtpsOnly'
      minTlsVersion: '1.2'
    }
    httpsOnly: true
  }
  dependsOn: [
    storageAccount
  ]
}

/* 
  Azure storage account fileshare mount using Bicep
*/
resource storageSetting 'Microsoft.Web/sites/config@2021-01-15' = {
  name: 'azurestorageaccounts'
  parent: functionApp
  properties: {
    '${functionAppName}': {
      type: 'AzureFiles'
      shareName: functionAppName
      mountPath: fileShareMountPath
      accountName: storageAccount.name
      accessKey: storageAccount.listKeys().keys[0].value
    }
  }
}


output name string = functionApp.name
output principalId string = functionApp.identity.principalId

