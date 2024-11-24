using PipelinePatternSample.Pipelines.Interfaces;
using PipelinePatternSample.Services.Interfaces;
using PipelinePatternSample.UseCases.Contexts;

namespace PipelinePatternSample.Pipelines
{
    public class RemasterImageStep : IAsyncPipelineStep<ImageProcessingContext>
    {
        private readonly IGenerativeAiRemasteringService _aiRemasteringService;

        public RemasterImageStep(IGenerativeAiRemasteringService aiRemasteringService)
        {
            _aiRemasteringService = aiRemasteringService;
        }

        public async Task<ImageProcessingContext> ProcessAsync(ImageProcessingContext input)
        {
            Console.WriteLine($"Starting remastering process for media '{input.Image.Name}'...");

            try
            {
                var remasteredImage = await _aiRemasteringService.RemasterImageAsync(input.Image);

                input.Image = remasteredImage;
                input.CloudImagePath = AddRemasteredSuffix(input.CloudImagePath);

                Console.WriteLine($"Remastering completed for media '{input.Image.Name}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to remaster media '{input.Image.Name}': {ex.Message}");
                throw; 
            }

            return input;
        }

        private static string AddRemasteredSuffix(string cloudImagePath)
        {
            var extension = Path.GetExtension(cloudImagePath);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(cloudImagePath);
            var directory = Path.GetDirectoryName(cloudImagePath);

            directory = directory?.Replace("\\", "/");

            return Path.Combine(directory, $"{fileNameWithoutExtension}_remastered{extension}");
        }
    }
}
