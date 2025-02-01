// Create custom HTTP handler
using BuildingKernels;
using Microsoft.SemanticKernel;

var handler = new CustomHttpMessageHandler();
var httpClient = new HttpClient(handler);

string modelId = "this is going to be ignored";

// Create kernel builder with custom client
var kernel = Kernel.CreateBuilder()
    .AddOpenAIChatCompletion(modelId, "ApiKey", "OrgId", "ServiceId", httpClient)

    .Build();


var prompt = "Write a hello world program in C#";
var result = await kernel.InvokePromptAsync(prompt);

Console.WriteLine(result);
Console.ReadKey();