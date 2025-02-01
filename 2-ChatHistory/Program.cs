using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

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

            // Add user message to chat history
            chatHistory.AddUserMessage("Write a hello world program in C#");

            var Answer1 = await ChatCompletionService.GetChatMessageContentAsync(chatHistory);

            Console.WriteLine("Press any key to continue");
            Console.WriteLine(Answer1);
            Console.ReadKey();

            chatHistory.AddAssistantMessage(Answer1.ToString());

            // Add another user message to chat history
            chatHistory.AddUserMessage("Rewrite previous anser in VB.NET");

            var Answer2 = await ChatCompletionService.GetChatMessageContentAsync(chatHistory);

            Console.WriteLine("Press any key to continue");
            Console.WriteLine(Answer2);
            Console.ReadKey();
        }
    }
}
