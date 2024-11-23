using PipelinePatternSample.Pipelines.Interfaces;
using PipelinePatternSample.Services;
using PipelinePatternSample.UseCases.Contexts;

namespace PipelinePatternSample.Pipelines
{
    public class DownloadMediaStep : IAsyncPipelineStep<ImageProcessingContext>
    {
        private readonly ICloudStorageService _cloudService;

        public DownloadMediaStep(ICloudStorageService cloudService)
        {
            _cloudService = cloudService;
        }

        public async Task<ImageProcessingContext> ProcessAsync(ImageProcessingContext input)
        {
            Console.WriteLine($"Downloading media '{input.Image.Name}' from cloud...");

            await Task.Delay(500); // Simula o tempo de download

            input.Image.Path = await _cloudService.DownloadAsync(input.Image.Name, input.TempFolderPath);

            Console.WriteLine($"Download complete: {input.Image.Path}");

            return input;
        }
    }
}
