# Semantic Kernel Examples

## Prerequisites

- .NET 9 SDK
- An OpenAI API key

## Setup

1. Clone the repository
2. Open the solution in Visual Studio 2022
3. Set the `OpenAiTestKey` environment variable with your OpenAI API key

## Projects

### 1. BuildingKernels

#### Introduction

This project demonstrates how to build and use kernels with the Microsoft Semantic Kernel library. The project is built using .NET 9 and showcases various prompts to generate C# console applications with different levels of detail.

#### Project Structure

- `Program.cs`: The main entry point of the application. It demonstrates how to create a kernel, add an OpenAI chat completion model, and invoke prompts to generate C# code.
- `1-BuildingKernels.csproj`: The project file that includes the necessary dependencies and targets .NET 9.

#### Usage

1. Run the application
2. The application will display results for various prompts:
   - Basic prompt: Generates a simple "Hello, World!" program in C#
   - Detailed prompt: Generates a C# console application that displays "Hello, World!" in green text and waits for a key press before closing
   - Very detailed prompt: Generates a C# console application with specific requirements such as changing text color, centering text, and using proper exception handling
   - Most detailed prompt: Generates a C# console application with advanced requirements including top-level statements, setting console title, storing and restoring console colors, and more

### 2. ChatHistory

#### Introduction

This project demonstrates how to use chat history functionality with Microsoft Semantic Kernel. It shows how to:
- Maintain conversation context across multiple interactions
- Track token usage for each interaction
- Convert code between programming languages using chat history context
- Handle chat completion responses with proper error handling

#### Project Structure

- `Program.cs`: Main entry point containing:
  - Kernel initialization with OpenAI chat completion
  - Chat history management implementation
  - Token usage tracking functionality
  - Console output handling with color differentiation
- `2-ChatHistory.csproj`: Project file with required dependencies:
  - Microsoft.SemanticKernel (1.35.0)
  - Targets .NET 9.0

#### Usage

The application demonstrates a two-step conversation flow:

1. First Interaction:
```csharp
// Initialize chat and request C# code
var message1 = "Write a hello world program in C#";
chatHistory.AddUserMessage(message1);
var Answer1 = await ChatCompletionService.GetChatMessageContentAsync(chatHistory);
```

2. Second Interaction:
```csharp
// Request conversion to VB.NET
var message2 = "Rewrite previous answer in VB.NET";
chatHistory.AddUserMessage(message2);
var Answer2 = await ChatCompletionService.GetChatMessageContentAsync(chatHistory);
```

Each response includes:
- The generated code
- Token usage statistics:
  - Input tokens
  - Output tokens
  - Total tokens

The application uses different console colors to distinguish between:
- User prompts (Green)
- AI responses (Blue)
- Token usage information

### 3. KernelFunctions

#### Introduction

This project demonstrates how to work with Kernel Functions in Microsoft Semantic Kernel. It showcases:
- Creating semantic functions from prompt templates
- Building custom plugins with typed functions
- Working with OpenAI prompt execution settings
- Handling different types of function invocations
- Using function descriptions and parameters

#### Project Structure

- `Program.cs`: Main entry point containing:
  - Kernel initialization with OpenAI settings
  - Prompt template function creation
  - Story plugin implementation
  - Function execution examples
  - Custom OpenAI prompt settings configuration
- `StoryPlugin.cs`: Custom plugin class with two kernel functions:
  - `GenerateStory`: Creates stories with specified parameters
  - `AddMoral`: Adds moral endings to stories
- `3-KernelFunctions.csproj`: Project file with dependencies

#### Usage

The application demonstrates three different approaches to using kernel functions:

1. Prompt-based Function:
```csharp
var promptTemplate = @"Write a {{$length}} sentence story about {{$topic}}.";
var promptFunction = kernel.CreateFunctionFromPrompt(promptTemplate, settings);
var result = await kernel.InvokeAsync(promptFunction, new KernelArguments {
    ["length"] = "three",
    ["topic"] = "a brave astronaut"
});
```

2. Plugin-based Story Generation:
```csharp
var storyPlugin = kernel.ImportPluginFromObject(new StoryPlugin(), "StoryPlugin");
var story = await kernel.InvokeAsync(storyPlugin["GenerateStory"], new KernelArguments {
    ["length"] = "three",
    ["topic"] = "a curious robot",
    ["genre"] = "sci-fi"
});
```

3. Adding Morals to Stories:
```csharp
var moral = await kernel.InvokeAsync(storyPlugin["AddMoral"], new KernelArguments {
    ["story"] = story.ToString(),
    ["moralType"] = "funny"
});
```

### 4. KernelPlugins

#### Introduction

This project demonstrates how to create and use plugins with Microsoft Semantic Kernel. It showcases:
- Creating custom plugins using C# classes
- Implementing kernel functions with attributes
- Using function descriptions for better documentation
- Working with strongly-typed parameters
- Importing and using plugins within the kernel

#### Project Structure

- `Program.cs`: Main entry point containing kernel initialization and plugin usage
- `MathPlugin.cs`: Custom plugin implementation
- `4-KernelPlugins.csproj`: Project file with dependencies

#### Usage

Example of creating and using the Math Plugin:

```csharp
// Define the plugin
public class MathPlugin
{
    [KernelFunction]
    public double Add(double a, double b) => a + b;

    [KernelFunction]
    public string Multiply(
        [Description("First number")] double a,
        [Description("Second number")] double b)
    {
        return $"{a} x {b} = {a * b}";
    }
}

// Using the plugin
var mathPlugin = kernel.ImportPluginFromObject(new MathPlugin(), "Math");

// Invoke Add function
var addResult = await kernel.InvokeAsync(mathPlugin["Add"], new() {
    ["a"] = 5,
    ["b"] = 3
});

// Invoke Multiply function
var multiplyResult = await kernel.InvokeAsync(mathPlugin["Multiply"], new() {
    ["a"] = 4,
    ["b"] = 6
});
```

### Using Local Models

#### Introduction

This project demonstrates how to use local Large Language Models (LLMs) with Microsoft Semantic Kernel. It showcases:
- Setting up and configuring local LLM endpoints
- Using models like Ollama or LM Studio
- Working with different model types locally
- Comparing results between local and cloud-based models
- Managing model settings and configurations

#### Project Structure

- `Program.cs`: Main entry point containing model configurations and usage examples
- `Models/`: Directory containing model configurations and settings
- `UsingLocalModels.csproj`: Project file with dependencies

#### Usage

1. Setting up Local Model Connection:
```csharp
// Configure local model endpoint
var kernelBuilder = Kernel.CreateBuilder()
    .AddLocalLLMService(
        "localModel",
        new HttpClient { BaseAddress = new Uri("http://localhost:11434") }
    );
```

2. Using Different Model Types:
```csharp
// Ollama example
var ollamaResults = await kernel.InvokePromptAsync(
    "What is the capital of France?",
    new KernelArguments { ["ModelType"] = "llama2" }
);

// LM Studio example
var lmStudioResults = await kernel.InvokePromptAsync(
    "What is the capital of France?",
    new KernelArguments { ["ModelType"] = "mistral" }
);
```

#### Key Features
- Local model configuration
- Multiple model support
- Response comparison with cloud models
- Performance metrics tracking
- Error handling for offline scenarios

#### Notes
- Requires local LLM server running (Ollama, LM Studio, etc.)
- Supports various local model types
- Provides fallback options for model unavailability
- Includes performance comparison tools