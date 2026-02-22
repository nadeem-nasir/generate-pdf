# Title
- Html to PDF Generator: A Serverless Application using Azure Functions and PuppeteerSharp to generate PDF from HTML. It also includes Azure Bicep templates and Azure DevOps pipelines for CI/CD.
- It is scalable, cost-effective, and easy to maintain.
# Introduction 
- This project is a serverless application that utilizes the power of Azure Functions and PuppeteerSharp to convert HTML into PDF documents. 
- This project is using PuppeteerSharp, a .NET port of the headless Chrome Node.js API Puppeteer, is used to control headless Chrome or Chromium browsers and enables the conversion of HTML to PDF.
- CI/CD is implemented using yaml pipelines in Azure DevOps and code included in this repository.
- Azure infrastructure is implemented using Azure bicep and code included in this repository.
# Prerequisites
- Azure subscription
- Visual Studio 2022 or later 
- .NET 8.0
- Azure Functions Core Tools v4.x
- Azure development workload in Visual Studio
# Azure resources used in the project
- Azure Function App in-process model and .Net 6
- Azure Application Insights
- Azure Storage Account
- Azure hosting plan
- azure log analytics workspace
- File share
- Mount File share using bicep template
# To run the application locally
- Clone the repository
- Open the solution in Visual Studio
- Restore the NuGet packages
- Rename template.settings.json to local.settings.json
- Update the local.settings.json 
- Bicep template deploy all the required resources and also set the required configurations. so it is easy to deploy the infrastructure and test the application
# Deploying Azure Function Apps using Azure DevOps
- Azure subscription
- Deploy infrastructure using Azure Bicep temnplate from infra folder
- Create a new project in Azure DevOps or GitHub
- Create a new service connection in Azure DevOps or GitHub
- Create variables in Azure DevOps or GitHub
- Create a new pipeline in Azure DevOps or GitHub
- Push the code to the repository
# Learning Outcomes of the Case Study 
- How to create a serverless application using Azure Functions and PuppeteerSharp
- Generate PDF from HTML using PuppeteerSharp
- Using File Share in Azure Functions
- Write different types of Azure Functions using triggers and bindings in .Net 6
- Ping function to test the application
- How to create a CI/CD pipeline using Azure DevOps
- How to create Azure infrastructure using Azure Bicep
- C# latest features 
# Extend the Case Study: Todo For Learning 
- Deploy the infrastructure using Azure Bicep template
- Change the AzureWebJobsStorage and APPINSIGHTS_INSTRUMENTATIONKEY in local.settings.json to use the Azure Storage connection string
- Run the application locally
- Try to run PingPdf_HttpTrigger function locally and see the output
- Create a new pipeline in Azure DevOps 
- Deploy the infrastructure using Azure Bicep template
- Deploy the application using Azure DevOps
- Test the application in Azure portal 
- Create a function that generates html and uploads to blob container
- Create another function that generates pdf from the html that is uploaded to blob container and uploads the pdf to an other blob container
- Migrate this application to .Net 8.0 and isolate model.
# Known Issues
- only works with Linux OS in Azure Functions in azure portal
