using Microsoft.SemanticKernel;
using System.ComponentModel;

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


            Console.ForegroundColor = ConsoleColor.Green;
            // 1. Create a semantic function using a prompt template
            var promptTemplate = @"Write a {{$length}} sentence story about {{$topic}}.";
            var promptFunction = kernel.CreateFunctionFromPrompt(
                promptTemplate,
                functionName: "StoryGenerator"
            );

            // 2. Create functions from a plugin type
            var storyPlugin = kernel.ImportPluginFromObject(new StoryPlugin(), "StoryPlugin");

            // Test both approaches
            Console.WriteLine("1. Testing prompt-based function:");
            var promptResult = await kernel.InvokeAsync(promptFunction, new KernelArguments
            {
                ["length"] = "three",
                ["topic"] = "a brave astronaut"
            });
            Console.WriteLine(promptResult);
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n2. Testing typed function - Generate Story:");
            var typedResult = await kernel.InvokeAsync(storyPlugin["GenerateStory"], new KernelArguments
            {
                ["length"] = "ten",
                ["topic"] = "a curious robot",
                ["genre"] = "sci-fi"
            });
            Console.WriteLine(typedResult);
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();

            Console.WriteLine("\n3. Testing typed function - Add Moral:");
            var moralResult = await kernel.InvokeAsync(storyPlugin["AddMoral"], new KernelArguments
            {
                ["story"] = typedResult.ToString(),
                ["moralType"] = "funny"
            });
            Console.WriteLine(moralResult);
            Console.ReadKey();
        }
    }

    public class StoryPlugin
    {
        [KernelFunction, Description("Generate a story based on given parameters")]
        public async Task<string> GenerateStory(
            [Description("Number of sentences in the story")] string length,
            [Description("Main topic of the story")] string topic,
            [Description("Genre of the story (e.g., sci-fi, fantasy, drama)")] string genre = "general"
        )
        {
            return $"This is a {genre} story about {topic} that will be {length} sentences long.";
        }

        [KernelFunction, Description("Add a moral to the end of a story")]
        public string AddMoral(
            [Description("The story to add a moral to")] string story,
            [Description("The type of moral (funny, serious, thoughtful)")] string moralType = "thoughtful"
        )
        {
            return $"{story}\nMoral of the story ({moralType}): Every great story has a lesson to learn.";
        }
    }
}
