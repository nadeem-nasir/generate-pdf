namespace AzureFuncs.Functions.GeneratePdfFunctions;
public class UrlToPdfFunction
{
    private readonly ILogger<UrlToPdfFunction> _logger;
    private readonly IPdfGeneratorService _pdfGeneratorService;
    public UrlToPdfFunction(IPdfGeneratorService pdfGeneratorService, ILogger<UrlToPdfFunction> log)
    {
        _logger = log;

        _pdfGeneratorService = pdfGeneratorService;
    }

    [FunctionName("UrlToPdfFunction")]
    public async Task<IActionResult> UrlToPdf(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] UrlToPdfRequest request)
    {

        try
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            _logger.LogInformation("C# HTTP trigger function processed a request.");

            if (string.IsNullOrEmpty(request.Url))
            {
                return new BadRequestObjectResult("Url is required");
            }

            var ms = await _pdfGeneratorService.GetPdfStream(request);

            return new FileContentResult(ms.ToArray(), "application/pdf") { FileDownloadName = $"{request.PDFFileName}.pdf" };
        }

        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex);
        }
    }
}

