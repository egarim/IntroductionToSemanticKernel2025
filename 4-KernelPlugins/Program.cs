using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.ComponentModel;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

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


                OpenAIPromptExecutionSettings settings = new()
                {
                    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
                    MaxTokens = null,
                    Temperature = 1,
                    ChatSystemPrompt = @"
                Assistant is a large language model. 
                This assistant uses plugins to interact with the software."
                };
                // Add the plugin to the kernel

                var mathPlugins = new MathPlugin();
                kernel.Plugins.AddFromObject(mathPlugins);
                kernel.FunctionInvocationFilters.Add(new MathFilter());

                var Prompt = "If I am 37 years old and my daughter is 7, what is our total combined age multiplied by 2 and the result plus 5";
                KernelArguments kernelArgument = new(settings);
                var result = await kernel.InvokePromptAsync(Prompt, kernelArgument);
                Console.WriteLine(result);

                Console.ReadKey();
            }
        }

        public class MathFilter() : IFunctionInvocationFilter
        {

            public async Task OnFunctionInvocationAsync(FunctionInvocationContext context, Func<FunctionInvocationContext, Task> next)
            {

                // Perform some actions before function invocation
                Debug.WriteLine("Before function invocation: " + context.Function.Name);
                await next(context);
                // Perform some actions after function invocation
                Debug.WriteLine("After function invocation: " + context.Function.Name);

            }
        }



    }
}
