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

// Create the process builder, and give the process a name.
ProcessBuilder processBuilder = new("PurchaseOrderProcess");

// Add all the steps we're going to need.
var intakeStep = processBuilder.AddStepFromType<IntakeStep>();
var poNotOkayStep = processBuilder.AddStepFromType<PoNotOkayStep>();
var poOkayStep = processBuilder.AddStepFromType<PoOkayStep>();
var processingStep = processBuilder.AddStepFromType<ProcessingStep>();
var lastStep = processBuilder.AddStepFromType<LastStep>();

// First we start by processing the PO with the intake event.
processBuilder.OnInputEvent(intakeStep.Name).SendEventTo(new ProcessFunctionTargetBuilder(intakeStep));

// Now, we use the LLM to help us determine how to handle the PO, and then send that to the correct step.
intakeStep.OnFunctionResult().SendEventTo(new ProcessFunctionTargetBuilder(processingStep));
processingStep.OnEvent("PO_AMOUNT_TOO_HIGH").SendEventTo(new ProcessFunctionTargetBuilder(poNotOkayStep));
processingStep.OnEvent("PO_AMOUNT_OKAY").SendEventTo(new ProcessFunctionTargetBuilder(poOkayStep));

// Either way, the results will be sent to the last step.
// Even though we're going to the same step this time, we need to specify which function to call.
poNotOkayStep.OnFunctionResult().SendEventTo(new ProcessFunctionTargetBuilder(lastStep, "ExecuteAsyncNotApproved"));
poOkayStep.OnFunctionResult().SendEventTo(new ProcessFunctionTargetBuilder(lastStep, "ExecuteAsyncApproved"));

// Once the last step is done, we stop the process.
// Just like when we called it, we need to specify which function result to stop process.
lastStep.OnFunctionResult("ExecuteAsyncNotApproved").StopProcess();
lastStep.OnFunctionResult("ExecuteAsyncApproved").StopProcess();

// Now build!
var process = processBuilder.Build();

// In order to start the process, we need to send a StartAsync.
// We're also going to send a random PO to the system, after all it needs something to process!
await using var runningProcess = await process.StartAsync(kernel, new KernelProcessEvent()
{
    Id = intakeStep.Name,
    Data = PurchaseOrder.CreateRandomPurchaseOrder()
});
