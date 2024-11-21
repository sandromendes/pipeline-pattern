using PipelinePatternSample.Domain;
using PipelinePatternSample.Pipelines.Interfaces;

namespace PipelinePatternSample.Pipelines
{
    public class AddMetadataStep : IAsyncPipelineStep<Media>
    {
        private readonly string _metadata;

        public AddMetadataStep(string metadata) => _metadata = metadata;

        public async Task<Media> ProcessAsync(Media input)
        {
            Console.WriteLine($"Adding metadata to '{input.Name}'...");

            await Task.Delay(200); // Simula a operação
            input.Metadata = _metadata;
            
            Console.WriteLine($"Metadata added: {input.Metadata}");
            
            return input;
        }
    }
}
