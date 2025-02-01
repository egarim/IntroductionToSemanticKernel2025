using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace BuildingKernels
{
    namespace BuildingKernels
    {
        // Define a plugin class
        public class MathPlugin
        {
            [KernelFunction]
            public double Add(double a, double b) => a + b;

            [KernelFunction]
            public string Multiply([Description("First number")] double a,
                                 [Description("Second number")] double b)
            {
                return $"{a} x {b} = {a * b}";
            }
        }

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

                // Add the plugin to the kernel
                var mathPlugin = kernel.ImportPluginFromObject(new MathPlugin(), "Math");

                // Use the plugin functions
                var addResult = await kernel.InvokeAsync(mathPlugin["Add"], new() { ["a"] = 5, ["b"] = 3 });
                Console.WriteLine($"Addition Result: {addResult}");

                var multiplyResult = await kernel.InvokeAsync(mathPlugin["Multiply"], new() { ["a"] = 4, ["b"] = 6 });
                Console.WriteLine($"Multiplication Result: {multiplyResult}");

                Console.ReadKey();
            }
        }
    }
}
