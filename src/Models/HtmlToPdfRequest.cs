namespace AzureFuncs.GeneratePdfFunctions.Models;

public class HtmlToPdfRequest : PdfRequestBase
{
    /// <summary>
    /// The HTML content that needs to be converted.
    /// </summary>
    public string? HtmlContent { get; set; }
}
