namespace AzureFuncs.GeneratePdfFunctions.Models;

public abstract class PdfRequestBase
{
    /// <summary>
    /// The name of the PDF file to be generated
    /// </summary>
    public string? PDFFileName { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Waits for an expression to be evaluated to a truthy value. if set to true add this code at the end of the page
    /// <script>window.onload = function () { window.IsPageLoaded = true; };</script>
    /// </summary>
    public bool? IsWaitForScriptExpression { get; set; } = false;


    /// <summary>
    ///  Sets extra HTTP headers that will be sent with every request the page initiates
    /// </summary>
    public Dictionary<string, string>? ExtraHttpHeaders { get; set; }
}
