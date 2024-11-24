using PipelinePatternSample.Pipelines.Interfaces;
using PipelinePatternSample.UseCases.Contexts;

namespace PipelinePatternSample.Pipelines
{
    public class AddMetadataStep : IAsyncPipelineStep<ImageProcessingContext>
    {
        public async Task<ImageProcessingContext> ProcessAsync(ImageProcessingContext input)
        {
            Console.WriteLine($"Adding metadata to '{input.Image.Name}'...");

            await Task.Delay(200); // Simula a operação
            input.Image.Metadata = input.Metadata;
            
            Console.WriteLine($"Metadata added: {input.Image.Metadata}");
            
            return input;
        }
    }
}
