using PipelinePatternSample.Pipelines.Interfaces;
using PipelinePatternSample.Services.Interfaces;
using PipelinePatternSample.UseCases.Contexts;

namespace PipelinePatternSample.Pipelines
{
    public class UploadMediaStep : IAsyncPipelineStep<ImageProcessingContext>
    {
        private readonly ICloudStorageService _cloudService;

        public UploadMediaStep(ICloudStorageService cloudService)
        {
            _cloudService = cloudService;
        }

        public async Task<ImageProcessingContext> ProcessAsync(ImageProcessingContext input)
        {
            await _cloudService.UploadAsync(input.Image.Path);

            return input;
        }
    }

}
