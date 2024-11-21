using PipelinePatternSample.Domain;
using PipelinePatternSample.Pipelines.Interfaces;

namespace PipelinePatternSample.Pipelines
{
    public class ApplyFilterStep : IAsyncPipelineStep<Media>
    {
        public async Task<Media> ProcessAsync(Media input)
        {
            Console.WriteLine($"Applying filter to '{input.Name}'...");

            await Task.Delay(300); // Simula o processamento

            Console.WriteLine($"Filter applied to '{input.Name}'.");

            return input;
        }
    }
}
