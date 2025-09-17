#pragma warning disable SKEXP0080

using System.Text.Json;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureAIInference;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;

// In this initial step, we can do some pre-processing. In this case, we're just going to log the details.
// However we can imagine maybe reading in a PO, or parsing some JSON, reading an email, or something similar.
public sealed class IntakeStep : KernelProcessStep
{
    // Since this is a simple step and there is only one KernelFunction, we can use the [KernelFunction]
    // attribute without giving it a name. Also the method name can be called anything, but ExecuteAsync
    // seems to make the most sense.
    [KernelFunction]
    public async Task<PurchaseOrder> ExecuteAsync(KernelProcessStepContext context, string documentPath, Kernel kernel)
    {
        // Lets verify there as a document path
        if (string.IsNullOrWhiteSpace(documentPath))
        {
            throw new ArgumentException("Document path is required", nameof(documentPath));
        }
        else
        {
            Console.WriteLine($"IntakeStep: Document path provided: {documentPath}");
        }

        Console.WriteLine($"IntakeStep: Processing PO Document.");

        // First lets take the document path and read in the PO data.
        string fullPath = Path.GetFullPath(documentPath);
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException($"The file at path '{fullPath}' does not exist.", fullPath);
        }

        byte[] imageBytes = await File.ReadAllBytesAsync(fullPath);

        // Get chat service
        var chatService = kernel.GetRequiredService<IChatCompletionService>();

        // Create chat history
        var history = new ChatHistory
        {
            new ChatMessageContent(AuthorRole.System, "You are a helpful assistant that extracts data from purchase order images accurately.")
        };

        string extractionPrompt = """
        Analyze this purchase order image and extract the key details: PO Number and Total Amount.
        Return the data strictly as JSON matching this schema:
        {
            "poNumber": "string",
            "amount": "number"
            "vendorName": "string",
            "buyerDepartment": "string"
        }
        Do not include any additional text or explanations.
        """;


        var userMessage = new ChatMessageContentItemCollection
        {
            new TextContent(extractionPrompt),
            new ImageContent(imageBytes, "image/png") // Adjust MIME type if JPEG: "image/jpeg"
        };

        history.AddUserMessage(userMessage);

        // Configure execution settings for structured output if using Azure OpenAI
        var executionSettings = new PromptExecutionSettings();

        // Invoke the chat and get response
        var response = await chatService.GetChatMessageContentAsync(history, executionSettings, kernel);

        Console.WriteLine($"LLM Response: {response.Content}");

        var poDetails = JsonSerializer.Deserialize<PurchaseOrder>(response.Content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        return poDetails ?? throw new InvalidOperationException("Failed to parse purchase order details from LLM response.");
    }
}