using OpenAI.Responses;
using System.ClientModel;

namespace ChatDemo.Service;

#pragma warning disable OPENAI001

public class FoundryChatService
{
    private readonly ResponsesClient _client;
    private readonly string _deploymentName;

    public FoundryChatService(IConfiguration configuration)
    {
        string endpoint =
            configuration["AzureAI:Endpoint"]
            ?? throw new InvalidOperationException(
                "AzureAI:Endpoint is missing.");

        _deploymentName =
            configuration["AzureAI:DeploymentName"]
            ?? throw new InvalidOperationException(
                "AzureAI:DeploymentName is missing.");

        string apiKey =
            configuration["AzureAI:ApiKey"]
            ?? throw new InvalidOperationException(
                "AzureAI:ApiKey is missing.");

        var options = new ResponsesClientOptions
        {
            Endpoint = new Uri(endpoint)
        };

        _client = new ResponsesClient(
            new ApiKeyCredential(apiKey),
            options);
    }

    public async Task<string> AskAsync(string question)
    {
        var request = new CreateResponseOptions
        {
            Model = _deploymentName
        };

        request.InputItems.Add(
            ResponseItem.CreateUserMessageItem(question));

        var result = await _client.CreateResponseAsync(request);

        return result.Value.GetOutputText();
    }
}