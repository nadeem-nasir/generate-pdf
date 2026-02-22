
@description('Location to deploy the resources')
param location string = resourceGroup().location

@description('Prefix for the environment name when creating the resources in this deployment.')
param environmentNamePrefix string

@description('Prefix for the name of the application, workload, or service that the resource is a part of.')
param ProjectOrApplicationNamePrefix string = 'showcase'

@description('Prefix for the name of the application instance.')
param ProjectOrApplicationInstanceNamePrefix string = '01'

@description('Runtime for the function worker.')
@allowed([
  'dotnet'
  'node'
  'python'
  'java'
  'dotnet-isolated'
])
param functionWorkerRuntime string 

@description('Name of the function app for generating PDFs.')
param FunctionAppNameForGeneratePdf string

@description('Specifies the operating system used for the Azure Function hosting plan.')
@allowed([
  'Windows'
  'Linux'
])
param functionPlanOS string


var applicationRegionPrefix = substring(location, 0, 2)

var resourceNameSuffix = '${environmentNamePrefix}-${applicationRegionPrefix}-${ProjectOrApplicationInstanceNamePrefix}'

var hostingPlanName = 'asp-${ProjectOrApplicationNamePrefix}-${resourceNameSuffix}'

var logAnalyticsWorkspaceName = 'log-${ProjectOrApplicationNamePrefix}-${resourceNameSuffix}'

var applicationInsightsName = 'appi-${ProjectOrApplicationNamePrefix}-${resourceNameSuffix}'

var storageAccountNamePrefix = 'st-${ProjectOrApplicationNamePrefix}-${resourceNameSuffix}'

var storageAccountFullName = toLower(replace(storageAccountNamePrefix, '-', ''))

var storageAccountName = length(storageAccountFullName) > 23 ? substring(storageAccountFullName,0,23) : storageAccountFullName

var isReserved = ((functionPlanOS == 'Linux') ? true : false)

var functionAppkind = (isReserved ? 'functionapp,linux' : 'functionapp')


var functionAppNameGeneratePdf = 'func-${ProjectOrApplicationNamePrefix}-${FunctionAppNameForGeneratePdf}-${resourceNameSuffix}'

module storageAccountResource './templates/storage-account.bicep' ={
  name: storageAccountName
  params: { 
  storageAccountName:storageAccountName
  storageAccountType:'Standard_LRS'
  location:location
  }
}

module logAnalyticsWorkSpace './templates/log-analytics-work-space.bicep' ={
  name: logAnalyticsWorkspaceName
  params: {
    logAnalyticsWorkspaceName:logAnalyticsWorkspaceName
    logAnalyticsWorkspaceSku:'PerGB2018' 
    location:location
   }
}


module ApplicationInsights './templates/application-insights.bicep' ={
  name: 'ApplicationInsights01'
  params: { 
    appInsightsLocation:location
    applicationInsightsName:applicationInsightsName
    logAnalyticsWorkspaceId:logAnalyticsWorkSpace.outputs.logAnalyticsWorkspaceId
  }
  dependsOn: [
    logAnalyticsWorkSpace
  ]
}

module HostingPlan './templates/hosting-plan.bicep' ={
  name: hostingPlanName
  params: { 
    location:location
    hostingPlanName:hostingPlanName
    isReserved:isReserved
  }
  dependsOn: [
    storageAccountResource
  ]
}


module FunctionAppGeneratePdf './templates/function-app.bicep'={
  name: functionAppNameGeneratePdf
  params: {
    functionAppName:functionAppNameGeneratePdf
    hostingPlanId:HostingPlan.outputs.hostingPlanId
    storageAccountName:storageAccountResource.name
    location:location
    functionWorkerRuntime:functionWorkerRuntime
    kind:functionAppkind
    applicationInsightsInstrumentationKey:ApplicationInsights.outputs.instrumentationKey
    fileShareMountPath:'/data001'
   }
   dependsOn: [
    HostingPlan
    storageAccountResource
    ApplicationInsights
  ]
}

