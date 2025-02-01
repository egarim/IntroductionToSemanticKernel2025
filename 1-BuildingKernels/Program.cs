using Microsoft.SemanticKernel;

namespace BuildingKernels
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

            // Basic prompt
            var prompt1 = "Write a hello world program in C#";

            // More detailed prompt
            var prompt2 = "Write a C# console application that displays 'Hello, World!' in green text and waits for a key press before closing";

            // Very detailed prompt
            var prompt3 = "Create a C# console application that:\n" +
                         "1. Changes console text color to green\n" +
                         "2. Displays 'Hello, World!' centered on the screen\n" +
                         "3. Restores original console color\n" +
                         "4. Waits for any key press before exit\n" +
                         "Use proper exception handling";

            // Most detailed prompt
            var prompt4 = "Create a C# console application that implements these requirements:\n" +
                         "1. Use top-level statements for Program.cs\n" +
                         "2. Set console title to 'Hello World App'\n" +
                         "3. Store original console colors before changing\n" +
                         "4. Change text color to green (ConsoleColor.Green)\n" +
                         "5. Clear the console screen\n" +
                         "6. Calculate center position for text placement\n" +
                         "7. Display 'Hello, World!' centered horizontally\n" +
                         "8. Restore original console colors\n" +
                         "9. Add try-catch block for error handling\n" +
                         "10. Display 'Press any key to exit' at bottom\n" +
                         "Use .NET 9 features where applicable";

            Console.WriteLine("Basic Prompt Result:");
            Console.WriteLine(await kernel.InvokePromptAsync(prompt1));
            Console.WriteLine("\nPress any key for next example...");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Detailed Prompt Result:");
            Console.WriteLine(await kernel.InvokePromptAsync(prompt2));
            Console.WriteLine("\nPress any key for next example...");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Very Detailed Prompt Result:");
            Console.WriteLine(await kernel.InvokePromptAsync(prompt3));
            Console.WriteLine("\nPress any key for next example...");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Most Detailed Prompt Result:");
            Console.WriteLine(await kernel.InvokePromptAsync(prompt4));
            Console.ReadKey();
        }
    }
}
