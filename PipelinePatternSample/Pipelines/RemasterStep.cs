using PipelinePatternSample.Domain;
using PipelinePatternSample.Pipelines.Interfaces;

namespace PipelinePatternSample.Pipelines
{
    public class RemasterStep : IAsyncPipelineStep<Media>
    {
        public async Task<Media> ProcessAsync(Media input)
        {
            Console.WriteLine($"Remastering media '{input.Name}'");

            await Task.Delay(500); // Simula o tempo de remasterização

            Console.WriteLine($"Remastering process finished");

            return input;
        }
    }
}
