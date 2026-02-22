using AzureFuncs.GeneratePdfFunctions.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
[assembly: FunctionsStartup(typeof(AzureFuncs.GeneratePdfFunctions.Startup))]
namespace AzureFuncs.GeneratePdfFunctions;
public sealed class Startup : FunctionsStartup
{
    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
    }
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddPdfGeneratorProvider();
        builder.Services.AddTemplateProvider();
        builder.Services.AddPdfGeneratorService();        
       // wait for the Chromium download to complete before proceeding.
       // If you want to download the chromium browser on the first request, you can remove this line.
       // This will increase the start time of the function app but the first request will be faster.
       //This method only downloads the chromium if not already downloaded.
        builder.Services.DownloadChromiumAsync().GetAwaiter().GetResult();

    }
}
