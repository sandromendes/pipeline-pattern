using PipelinePatternSample.Domain;
using PipelinePatternSample.Pipelines.Interfaces;

namespace PipelinePatternSample.Pipelines
{
    public class SaveImageStep : IAsyncPipelineStep<Media>
    {
        private readonly string _destinationFolder;

        public SaveImageStep(string destinationFolder) => _destinationFolder = destinationFolder;

        public async Task<Media> ProcessAsync(Media input)
        {
            Console.WriteLine($"Saving image '{input.Name}' to '{_destinationFolder}'...");
            await Task.Delay(200); // Simula a operação

            var savePath = $"{_destinationFolder}/{input.Name}.png";
            input.Path = savePath;

            Console.WriteLine($"Image saved: {input.Path}");
            return input;
        }
    }
}
