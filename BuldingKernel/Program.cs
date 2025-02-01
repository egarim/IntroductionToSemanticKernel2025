using Microsoft.SemanticKernel;
using OpenAI;
using System.Diagnostics;

namespace BuldingKernel
{
    internal class Program
    {
        static async Task Main(string[] args)
        {


            //Create kernel builder with custom client
            string modelId = "gpt-4o";
            string OpenAiKey = Environment.GetEnvironmentVariable("OpenAiTestKey");
            var kernel = Kernel.CreateBuilder()
                .AddOpenAIChatCompletion(modelId, OpenAiKey)
                .Build();

            var prompt = "Write a hello world program in C#";
            var result = await kernel.InvokePromptAsync(prompt);

            Console.WriteLine(result);
            Console.ReadKey();

        }
    }
}
