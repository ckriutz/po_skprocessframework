#pragma warning disable SKEXP0080
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Process.Runtime;

public class PurchaseOrderProcess
{
    public static KernelProcess CreateProcess()
    {
        // Create the process builder, and give the process a name.
        ProcessBuilder processBuilder = new("PurchaseOrderProcess");

        // Add all the steps we're going to need.
        var intakeStep = processBuilder.AddStepFromType<IntakeStep>();
        var poNotOkayStep = processBuilder.AddStepFromType<PoNotOkayStep>();
        var poOkayStep = processBuilder.AddStepFromType<PoOkayStep>();
        var processingStep = processBuilder.AddStepFromType<ProcessingStep>();
        var lastStep = processBuilder.AddStepFromType<LastStep>();

        // Define the workflow
        processBuilder.OnInputEvent(intakeStep.Name).SendEventTo(new ProcessFunctionTargetBuilder(intakeStep));

        intakeStep.OnFunctionResult().SendEventTo(new ProcessFunctionTargetBuilder(processingStep));
        processingStep.OnEvent("PO_AMOUNT_TOO_HIGH").SendEventTo(new ProcessFunctionTargetBuilder(poNotOkayStep));
        processingStep.OnEvent("PO_AMOUNT_OKAY").SendEventTo(new ProcessFunctionTargetBuilder(poOkayStep));

        poNotOkayStep.OnFunctionResult().SendEventTo(new ProcessFunctionTargetBuilder(lastStep, "ExecuteAsyncNotApproved"));
        poOkayStep.OnFunctionResult().SendEventTo(new ProcessFunctionTargetBuilder(lastStep, "ExecuteAsyncApproved"));

        lastStep.OnFunctionResult("ExecuteAsyncNotApproved").StopProcess();
        lastStep.OnFunctionResult("ExecuteAsyncApproved").StopProcess();

        return processBuilder.Build();
    }
}