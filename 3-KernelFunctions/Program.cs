using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
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

            
            var settings = new OpenAIPromptExecutionSettings
            {
                Temperature = Math.Clamp(0.7f, 0.0f, 1.0f), // Ensure temperature is between 0 and 1
                TopP = Math.Clamp(1.0f, 0.0f, 1.0f),       // Ensure TopP is between 0 and 1
                MaxTokens = Math.Max(1, 500),               // Ensure positive token count
                StopSequences = new List<string> { "." },
                PresencePenalty = Math.Clamp(0.0f, -2.0f, 2.0f),    // Valid range is -2.0 to 2.0
                FrequencyPenalty = Math.Clamp(0.0f, -2.0f, 2.0f)    // Valid range is -2.0 to 2.0
            };

            if (settings.MaxTokens > 4000) // Common token limit for many models
            {
                throw new ArgumentOutOfRangeException(nameof(settings.MaxTokens), "Token limit exceeds model maximum");
            }

            // 1. Create a semantic function using a prompt template
            var promptTemplate = @"Write a {{$length}} sentence story about {{$topic}}.";
            var promptFunction = kernel.CreateFunctionFromPrompt(
                promptTemplate,settings,
                functionName: "StoryGenerator"
            );

            // 2. Create functions from a plugin type
            var storyPlugin = kernel.ImportPluginFromObject(new StoryPlugin(), "StoryPlugin");

            // Test both approaches
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
                ["length"] = "three",
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
