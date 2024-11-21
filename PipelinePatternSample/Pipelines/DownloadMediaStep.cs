using PipelinePatternSample.Domain;
using PipelinePatternSample.Pipelines.Interfaces;
using PipelinePatternSample.Services;

namespace PipelinePatternSample.Pipelines
{
    public class DownloadMediaStep : IAsyncPipelineStep<Media>
    {
        private readonly CloudStorageService _cloudService;
        private readonly string _tempFolder;

        public DownloadMediaStep(CloudStorageService cloudService, string tempFolder)
        {
            _cloudService = cloudService;
            _tempFolder = tempFolder;
        }

        public async Task<Media> ProcessAsync(Media input)
        {
            Console.WriteLine($"Downloading media '{input.Name}' from cloud...");

            await Task.Delay(500); // Simula o tempo de download

            input.Path = await _cloudService.DownloadAsync(input.Name, _tempFolder);

            Console.WriteLine($"Download complete: {input.Path}");

            return input;
        }
    }
}
