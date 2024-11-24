using PipelinePatternSample.Pipelines.Interfaces;
using PipelinePatternSample.UseCases.Contexts;

namespace PipelinePatternSample.Pipelines
{
    public class SaveImageStep : IAsyncPipelineStep<ImageProcessingContext>
    {
        public async Task<ImageProcessingContext> ProcessAsync(ImageProcessingContext input)
        {
            Console.WriteLine($"Saving image '{input.Image.Name}' to '{input.LocalImagePath}'...");
            
            await Task.Delay(200); // Simula a operação

            var savePath = $"{input.LocalImagePath}/{input.Image.Name}";
            input.Image.Path = savePath;

            Console.WriteLine($"Image saved: {input.Image.Path}");
            return input;
        }
    }
}
