using Microsoft.SemanticKernel;

namespace KernelFunctions
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

            // Create a semantic function using a prompt template
            var promptTemplate = @"Write a {{$length}} sentence story about {{$topic}}.";
            var storyFunction = kernel.CreateFunctionFromPrompt(
                promptTemplate,
                functionName: "StoryGenerator"
            );

            // Prepare the arguments for the function
            var arguments = new KernelArguments
            {
                ["length"] = "three",
                ["topic"] = "a brave astronaut"
            };

            // Execute the function
            var result = await kernel.InvokeAsync(storyFunction, arguments);

            Console.WriteLine("Generated Story:");
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
