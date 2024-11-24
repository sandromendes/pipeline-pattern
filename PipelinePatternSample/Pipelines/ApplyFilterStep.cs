using PipelinePatternAutomatedTests.Extensions;
using PipelinePatternSample.Pipelines.Interfaces;
using PipelinePatternSample.UseCases.Contexts;

namespace PipelinePatternSample.Pipelines
{
    public class ApplyFilterStep : IAsyncPipelineStep<ImageProcessingContext>
    {
        public async Task<ImageProcessingContext> ProcessAsync(ImageProcessingContext input)
        {
            Console.WriteLine($"Applying filter to '{input.Image.Name}'...");

            await Task.Delay(300); // Simula o processamento

            input.CloudImagePath = input.CloudImagePath.AddSuffix("edited");
            input.LocalImagePath = input.LocalImagePath.AddSuffix("edited");

            Console.WriteLine($"Filter applied to '{input.Image.Name}'.");

            return input;
        }
    }
}
