namespace AzureFuncs.Functions.GeneratePdfFunctions;
public class HtmlToPdfFunction
{
    private readonly ILogger<HtmlToPdfFunction> _logger;
    private readonly IPdfGeneratorService _pdfGeneratorService;

    public HtmlToPdfFunction(IPdfGeneratorService pdfGeneratorService, ILogger<HtmlToPdfFunction> log)
    {
        _logger = log;

        _pdfGeneratorService = pdfGeneratorService;
    }

    [FunctionName("HtmlToPdfFunction")]
    public async Task<IActionResult> HtmlToPdf(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HtmlToPdfRequest request)
    {

        try
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            _logger.LogInformation("C# HTTP trigger function processed a request.");

            if (string.IsNullOrEmpty(request.HtmlContent))
            {
                return new BadRequestObjectResult("HtmlContent is required");
            }

            using var ms = await _pdfGeneratorService.GetPdfStream(request);

            return new FileContentResult(ms.ToArray(), "application/pdf") { FileDownloadName = $"{request.PDFFileName}.pdf" };
        }

        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex);
        }
    }

    /// <summary>
    /// This function is triggered by a blob storage event. 
    /// It takes an HTML file from the "invoice-processing-step2-as-html-data" container and converts it to a PDF file using a PDF generator service. 
    /// It then uploads the PDF file to the "invoice-processing-step3-as-pdf-data" container with the same name as the original HTML file but with .pdf extension.
    /// The function uses the "AzureWebJobsStorage" connection string to access the blob storage.    
    /// </summary>
    /// <param name="blobHtmlContent"></param>
    /// <param name="blobToUpload"></param>
    /// <returns></returns>
    [FunctionName("InvoiceProcessingHtmlToPdf")]
    public async Task InvoiceProcessingHtmlToPdf([BlobTrigger("invoice-processing-step2-as-html-data/{name}", Connection = "AzureWebJobsStorage")] string blobHtmlContent,
        [Blob("invoice-processing-step3-as-pdf-data/{name}.pdf", FileAccess.Write, Connection = "AzureWebJobsStorage")] Stream blobToUpload)
    {
        var request = new HtmlToPdfRequest { HtmlContent = blobHtmlContent };
        using var ms = await _pdfGeneratorService.GetPdfStream(request);
        ms.Position = 0;

        await ms.CopyToAsync(blobToUpload);
    }

}

