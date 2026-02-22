using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Options;

namespace AzureFuncs.GeneratePdfFunctions.Providers;


public interface ITemplateProvider
{

    Task<string> GetTemplate(string templateName);
    string GetTemplatePath(string templateName);
}


public class TemplateProvider : ITemplateProvider
{
    // Declare a private field to store the execution context options
    private readonly IOptions<ExecutionContextOptions> _executionContext;

    // Define a constructor that takes the execution context options as a parameter and assigns it to the field
    public TemplateProvider(IOptions<ExecutionContextOptions> executionContext)
    {
        _executionContext = executionContext;
    }

    public async Task<string> GetTemplate(string templateName)
    {
        // Get the value of the execution context options
        var context = _executionContext.Value;

        // Get the app directory from the context
        var rootDir = context.AppDirectory;

        // Combine the app directory and the template name to get the full path of the template file
        var templatePath = Path.Combine(rootDir, "Template", templateName);

        // Read and return the content of the template file
        return await File.ReadAllTextAsync(templatePath);
    }
    public string GetTemplatePath(string templateName)
    {
        // Get the value of the execution context options
        var context = _executionContext.Value;

        // Get the app directory from the context
        var rootDir = context.AppDirectory;

        // Combine the app directory and the template name to get the full path of the template file
        var templatePath = Path.Combine(rootDir, "Template", templateName);

        return templatePath;
    }
}
