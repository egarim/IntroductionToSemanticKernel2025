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
            KernelFunction promptFunction = kernel.CreateFunctionFromPrompt(
                promptTemplate, settings,
                functionName: "StoryGenerator"
            );

       
         
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

            // 2. Create functions from a plugin type
            var textAnalysisPlugin = kernel.ImportPluginFromObject(new TextAnalysisPlugin());

            Console.WriteLine("\n2. Testing typed function - CountWords :");
            var typedResult = await kernel.InvokeAsync(textAnalysisPlugin["CountWords"], new KernelArguments
            {
                ["input"] = "On April 12, 1961, Soviet cosmonaut Yuri Gagarin made history as the first human to travel into space. A former fighter pilot, Gagarin was selected from a group of 20 Soviet Air Force candidates due to his exceptional skills and charisma."
            });
            Console.WriteLine(typedResult);
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();

            Console.WriteLine("\n3. Testing typed function - DetectSentiment");
            var moralResult = await kernel.InvokeAsync(textAnalysisPlugin["DetectSentiment"], new KernelArguments
            {
                ["input"] = "On April 12, 1961, the Soviet cosmonaut Yuri Gagarin made history as the first human to travel into space. A former fighter pilot, Gagarin was selected from a group of 20 Soviet Air Force candidates due to his exceptional skills and charisma."
            });
            Console.WriteLine(moralResult);
            Console.ReadKey();
        }
    }

    public class TextAnalysisPlugin
    {
        [KernelFunction]
        [Description("Counts the number of words in a given text.")]
        public int CountWords([Description("The text to analyze.")] string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return 0;
            int wordCount = input.Split(new[] { ' ', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
            return wordCount;
        }

        [KernelFunction]
        [Description("Detects the sentiment of a given text.")]
        public Task<string> DetectSentiment([Description("The text to analyze.")]  string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return Task.FromResult("Neutral");

            string[] positiveWords = { "good", "great", "happy", "awesome", "excellent" };
            string[] negativeWords = { "bad", "sad", "terrible", "horrible", "awful" };

            int positiveCount = input.Split().Count(word => positiveWords.Contains(word.ToLower()));
            int negativeCount = input.Split().Count(word => negativeWords.Contains(word.ToLower()));

            string sentiment = positiveCount > negativeCount ? "Positive" :
                               negativeCount > positiveCount ? "Negative" : "Neutral";

            return Task.FromResult(sentiment);
        }
    }
}
