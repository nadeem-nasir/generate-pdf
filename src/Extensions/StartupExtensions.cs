using Microsoft.Extensions.DependencyInjection;

namespace AzureFuncs.GeneratePdfFunctions.Extensions;
public static class StartupExtensions
{
    public static IServiceCollection AddTemplateProvider(this IServiceCollection services)
    {
        services.AddSingleton<ITemplateProvider, TemplateProvider>();

        return services;
    }

    public static IServiceCollection AddPdfGeneratorProvider(this IServiceCollection services)
    {
        services.AddSingleton<IPdfGeneratorProvider, PuppeteerSharpPdfGeneratorProvider>();

        return services;
    }

    public static IServiceCollection AddPdfGeneratorService(this IServiceCollection services)
    {
        services.AddScoped<IPdfGeneratorService, PuppeteerSharpPdfGeneratorService>();

        return services;
    }

    /// <summary>
    /// This method download the chromium browser when the function app starts.
    /// This willl increase the start time of the function app but first request will be faster.
    /// If you want to download the chromium browser on the first request, you can remove this method.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static async Task<IServiceCollection> DownloadChromiumAsync(this IServiceCollection services)
    {
        using (var serviceProvider = services.BuildServiceProvider())
        {
            var pdfGeneratorProvider = serviceProvider.GetService<IPdfGeneratorProvider>();
            if (pdfGeneratorProvider != null)
            {
                await pdfGeneratorProvider.GetExecutablePath();
            }
        }

        return services;
    }
}

