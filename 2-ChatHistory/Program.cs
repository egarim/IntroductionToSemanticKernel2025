using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OpenAI.Chat;

namespace ChatHistoryExample
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

            var ChatCompletionService = kernel.Services.GetRequiredService<IChatCompletionService>();

            // Create a new chat history
            var chatHistory = new ChatHistory();

            Console.ForegroundColor = ConsoleColor.Green;

            // Add user message to chat history
            var message1 = "Write a hello world program in C#";
            Console.WriteLine(message1);
            chatHistory.AddUserMessage(message1);

            var Answer1 = await ChatCompletionService.GetChatMessageContentAsync(chatHistory);

            Console.WriteLine("Press any key to continue");
            Console.WriteLine(Answer1);
            WriteTokenUsage(Answer1);
            Console.ReadKey();

            //Add the first answer to the history
            chatHistory.AddAssistantMessage(Answer1.ToString());

            Console.ForegroundColor = ConsoleColor.Blue;

            // Add another user message to chat history
            var message2 = "Rewrite previous answer in VB.NET";
            Console.WriteLine(message2);
            chatHistory.AddUserMessage(message2);

            var Answer2 = await ChatCompletionService.GetChatMessageContentAsync(chatHistory);

            Console.WriteLine("Press any key to continue");
            Console.WriteLine(Answer2);
            WriteTokenUsage(Answer2);
            Console.ReadKey();
        }

        private static void WriteTokenUsage(Microsoft.SemanticKernel.ChatMessageContent response)
        {
            if (response?.Metadata == null) return;

            if (response.Metadata.TryGetValue("Usage", out var usage) && usage != null)
            {
                var usageDict = usage as ChatTokenUsage;
                if (usageDict != null)
                {
                    Console.WriteLine($"Input Tokens: {usageDict.InputTokenCount}");
                    Console.WriteLine($"Output Tokens: {usageDict.OutputTokenCount}");
                    Console.WriteLine($"Total Tokens: {usageDict.TotalTokenCount}");
                }
            }
        }

    }
}
