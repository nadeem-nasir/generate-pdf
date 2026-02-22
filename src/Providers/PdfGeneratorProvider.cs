using System.Runtime.InteropServices;

namespace AzureFuncs.GeneratePdfFunctions.Providers;
public interface IPdfGeneratorProvider
{
    Task<string> GetExecutablePath();
}

public class PuppeteerSharpPdfGeneratorProvider : IPdfGeneratorProvider
{
    public virtual bool IsAzureEnvironment => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID"));
    public virtual string FileShareMountPath => Environment.GetEnvironmentVariable("FileShareMountPath") ?? "";

    /// <summary>
    /// download chromium to the azure file share and return the path to the executable
    /// If the executable is already downloaded, it will return the path to the executable
    /// If azure file share path is not provided, it will download chromium to the temp folder
    /// for windows it will download chromium to default path
    /// in local development environment it will download chromium to the bin folder
    /// </summary>
    /// <returns></returns>
    public virtual async Task<string> GetExecutablePath()
    {
        var bfOptions = new BrowserFetcherOptions();
        var executablePath = GetDownloadsFolderPath();
        if (!string.IsNullOrWhiteSpace(executablePath))
        {
            bfOptions.Path = executablePath;
        }

        var browserFetcher = new BrowserFetcher(bfOptions);

        // if  the executable are not downloaded, download them
        await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);

        return browserFetcher.GetExecutablePath(BrowserFetcher.DefaultChromiumRevision);

    }

    /// <summary>
    /// A path for the downloads folder. Defaults to [root]/.local-chromium, where [root] is where the project binaries are located.
    /// path where the chromium binaries will be downloaded
    /// </summary>
    /// <returns></returns>
    private string GetDownloadsFolderPath()
    {
        var executablePath = string.Empty;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && !string.IsNullOrWhiteSpace(FileShareMountPath))
        {
            executablePath = FileShareMountPath;
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            executablePath = Path.GetTempPath();
        }

        return executablePath;
    }
}
