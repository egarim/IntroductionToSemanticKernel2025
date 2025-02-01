// Create custom HTTP handler
using BuildingKernels;
using Microsoft.SemanticKernel;

//this can redirect to any local model that uses the same API as Azure OpenAI or OpenAI
//you can use L.M studio or Ollama
var handler = new CustomHttpMessageHandler("http://localhost:1234/v1/chat/completions");
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