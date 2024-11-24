using PipelinePatternSample.Pipelines.Interfaces;
using PipelinePatternSample.Services.Interfaces;
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

            input.Image = await _imageProcessingService.RotateImageAsync(input.Image, input.RotationAngle);
            input.CloudImagePath = AddRotatedSuffix(input.CloudImagePath);

            Console.WriteLine($"Rotation complete for '{input.Image.Name}'.");
            
            return input;
        }

        private static string AddRotatedSuffix(string cloudImagePath)
        {
            var extension = Path.GetExtension(cloudImagePath);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(cloudImagePath);
            var directory = Path.GetDirectoryName(cloudImagePath);

            directory = directory?.Replace("\\", "/");

            return Path.Combine(directory, $"{fileNameWithoutExtension}_rotated{extension}");
        }
    }

}
