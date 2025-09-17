#pragma warning disable SKEXP0080

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Process.Runtime;
using System;

Console.WriteLine("Testing the Semantic Kernel PO process.");

// Read Azure OpenAI settings from environment variables
var model = Environment.GetEnvironmentVariable("model") ?? throw new InvalidOperationException("Environment variable model is not set.");
var endpoint = Environment.GetEnvironmentVariable("endpoint") ?? throw new InvalidOperationException("Environment variable endpoint is not set.");
var key = Environment.GetEnvironmentVariable("apikey") ?? throw new InvalidOperationException("Environment variable apikey is not set.");


Console.WriteLine($"Using model={model}, endpoint={endpoint}");

Kernel kernel = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(model, endpoint, key)
    .Build();

var process = PurchaseOrderProcess.CreateProcess();

// In order to start the process, we need to send a StartAsync.
// We're also going to send an IT PO to the system, after all it needs something to process!
await using var ItPoProcess = await process.StartAsync(kernel, new KernelProcessEvent()
{
    Id = "IntakeStep", // Use the step name
    Data = "PurchaseOrders/AdventureWorksPO_ITOrder.png"
});

// We're also going to send an HR PO to the system, after all it needs something to process!
await using var HrPoProcess = await process.StartAsync(kernel, new KernelProcessEvent()
{
    Id = "IntakeStep", // Use the step name
    Data = "PurchaseOrders/AdventureWorksPO_HROrder.png"
});