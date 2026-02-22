namespace AzureFuncs.GeneratePdfFunctions.Services;
public interface IPdfGeneratorService
{
    Task<MemoryStream> GetPdfStream(HtmlToPdfRequest htmlToPdfRequest);
    Task<MemoryStream> GetPdfStream(UrlToPdfRequest url2PdfRequest);
}
public class PuppeteerSharpPdfGeneratorService : IPdfGeneratorService
{

    private readonly IPdfGeneratorProvider _pdfGeneratorProvider;

    public PuppeteerSharpPdfGeneratorService(IPdfGeneratorProvider pdfGeneratorProvider)
    {
        _pdfGeneratorProvider = pdfGeneratorProvider;
    }

    public async Task<MemoryStream> GetPdfStream(HtmlToPdfRequest htmlToPdfRequest)
    {
        var executablePath = await _pdfGeneratorProvider.GetExecutablePath();

        using var browser = await GetBrowser(executablePath);

        using IPage page = await GetNewPage(htmlToPdfRequest.ExtraHttpHeaders, browser);

        await page.SetContentAsync(htmlToPdfRequest.HtmlContent);

        await SetsWaitForScriptExpression(htmlToPdfRequest.IsWaitForScriptExpression, page);

        var pdfContent = await GetPdfContent(page);

        MemoryStream ms = await GetPdfMemoryStream(pdfContent);

        await browser.DisposeAsync();

        return ms;

    }

    public async Task<MemoryStream> GetPdfStream(UrlToPdfRequest url2PdfRequest)
    {
        var executablePath = await _pdfGeneratorProvider.GetExecutablePath();

        using var browser = await GetBrowser(executablePath);

        using IPage page = await GetNewPage(url2PdfRequest.ExtraHttpHeaders, browser);

        await page.GoToAsync(url2PdfRequest.Url);

        await SetsWaitForScriptExpression(url2PdfRequest.IsWaitForScriptExpression, page);

        var pdfContent = await GetPdfContent(page);

        using MemoryStream ms = await GetPdfMemoryStream(pdfContent);

        await browser.DisposeAsync();

        return ms;
    }

    private static async Task<IBrowser> GetBrowser(string executablePath)
    {
        return await Puppeteer.LaunchAsync(new LaunchOptions {
            Headless = true,
            ExecutablePath = executablePath,
        });
    }

    private static async Task<IPage> GetNewPage(Dictionary<string, string>? extraHttpHeaders, IBrowser browser)
    {
        var page = await browser.NewPageAsync();

        if (extraHttpHeaders != null && extraHttpHeaders.Count > 0)
        {
            await page.SetExtraHttpHeadersAsync(extraHttpHeaders);
        }

        await page.EmulateMediaTypeAsync(MediaType.Screen);

        return page;
    }

    /// <summary>
    /// For pages that require javascript to load, this method will wait for the script to be executed
    /// Add <script>window.onload = function () { window.IsPageLoaded = true; };</script> at the end of the page and set isWaitForScriptExpression to true for this method to work
    /// For more details check Demo-template.html in the Template folder
    /// </summary>
    /// <param name="isWaitForScriptExpression"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    private static async Task SetsWaitForScriptExpression(bool? isWaitForScriptExpression, IPage page)
    {
        if (isWaitForScriptExpression.GetValueOrDefault())
        {
            await page.WaitForExpressionAsync("window.IsPageLoaded");
        }
    }

    private static async Task<Stream> GetPdfContent(IPage page)
    {
        return await page.PdfStreamAsync(new PdfOptions {
            Format = PaperFormat.A4,
            PrintBackground = true,
        });
    }

    private static async Task<MemoryStream> GetPdfMemoryStream(Stream pdfContent)
    {
        MemoryStream ms = new();

        await pdfContent.CopyToAsync(ms);

        return ms;
    }
}
