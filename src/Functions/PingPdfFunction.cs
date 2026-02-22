using Microsoft.AspNetCore.Http;

namespace AzureFuncs.GeneratePdfFunctions;

public sealed class PingPdfFunction
{
    private readonly IPdfGeneratorService _pdfGeneratorService;
    private readonly ITemplateProvider _templateProvider;
    public PingPdfFunction(IPdfGeneratorService pdfGeneratorService, ITemplateProvider templateProvider)
    {
        _pdfGeneratorService = pdfGeneratorService;
        _templateProvider = templateProvider;
    }

    /// <summary>
    /// This function is a demo function that shows how to convert an HTML string into a PDF file.
    /// </summary>
    /// <param name="req"></param>
    /// <param name="log"></param>
    /// <returns></returns>
    [FunctionName("PingPdf_HttpTrigger")]
    public async Task<IActionResult> PingPdf_HttpTrigger(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");
        log.LogError("error from local system.usinmg cloud role");
        try
        {
            var request = new HtmlToPdfRequest {
                HtmlContent = "<div><h1>Hello PDF world!</h1><h2 style='color: red; text-align: center;'>Greetings from <i>HTML</i> world</h2></div>"
            };

            var ms = await _pdfGeneratorService.GetPdfStream(request);

            return new FileContentResult(ms.ToArray(), "application/pdf") { FileDownloadName = $"{request.PDFFileName}.pdf" };
        }

        catch (Exception ex)
        {
            return new OkObjectResult(ex);
        }
    }

    /// <summary>
    /// This function is a demo function that shows how to convert an HTML file into a PDF file .
    /// The HTML file can contain any valid HTML code and can use external resources such as images, fonts, or stylesheets. 
    /// The PDF generator service uses a headless browser to render the HTML content and capture it as a PDF stream. 
    /// The function can be tested by sending an HTTP request to the function URL with any query string or body parameters. 
    /// The function will ignore the parameters and use the predefined HTML template. 
    /// The function will return the PDF file as a response that can be downloaded or viewed in a browser.
    /// The Demp Html template is stored in Template folder in root directory.
    /// Only for demo and testing purpose.
    /// </summary>
    /// <param name="req"></param>
    /// <param name="log"></param>
    /// <returns></returns>
    [FunctionName("Ping_File_Pdf_HttpTrigger")]
    public async Task<IActionResult> Ping_File_Pdf_HttpTrigger(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        try
        {
            var html = await _templateProvider.GetTemplate("Demo-template.html");

            var request = new HtmlToPdfRequest {
                HtmlContent = html,
                IsWaitForScriptExpression = true
            };

            var ms = await _pdfGeneratorService.GetPdfStream(request);

            return new FileContentResult(ms.ToArray(), "application/pdf") { FileDownloadName = $"{request.PDFFileName}.pdf" };
        }

        catch (Exception ex)
        {
            return new OkObjectResult(ex);
        }
    }

}
