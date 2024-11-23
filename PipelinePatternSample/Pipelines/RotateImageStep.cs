using PipelinePatternSample.Pipelines.Interfaces;
using PipelinePatternSample.Services;
using PipelinePatternSample.UseCases.Contexts;

namespace PipelinePatternSample.Pipelines
{
    public class RotateImageStep : IAsyncPipelineStep<ImageProcessingContext>
    {
        private readonly IImageProcessingService _imageProcessingService;

        public RotateImageStep(IImageProcessingService imageProcessingService)
        {
            _imageProcessingService = imageProcessingService;
        }

        public async Task<ImageProcessingContext> ProcessAsync(ImageProcessingContext input)
        {
            Console.WriteLine($"Rotating '{input.Image.Name}' by {input.RotationAngle} degrees...");

            await _imageProcessingService.RotateImageAsync(input.Image, input.RotationAngle);
            Console.WriteLine($"Rotation complete for '{input.Image.Name}'.");
            
            return input;
        }
    }

}
