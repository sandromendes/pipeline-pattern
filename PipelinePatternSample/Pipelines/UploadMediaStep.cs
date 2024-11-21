using PipelinePatternSample.Domain;
using PipelinePatternSample.Pipelines.Interfaces;
using PipelinePatternSample.Services;

namespace PipelinePatternSample.Pipelines
{
    public class UploadMediaStep : IAsyncPipelineStep<Media>
    {
        private readonly CloudStorageService _cloudService;

        public UploadMediaStep(CloudStorageService cloudService)
        {
            _cloudService = cloudService;
        }

        public async Task<Media> ProcessAsync(Media input)
        {
            await _cloudService.UploadAsync(input.Path);

            return input;
        }
    }

}
