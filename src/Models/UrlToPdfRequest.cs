namespace AzureFuncs.GeneratePdfFunctions.Models;

public class UrlToPdfRequest : PdfRequestBase
{
    /// <summary>
    /// The url that needs to be converted.
    /// </summary>
    public string? Url { get; set; }
}
