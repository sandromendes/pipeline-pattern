using PipelinePatternSample.Core.Builders;
using PipelinePatternSample.Pipelines;
using PipelinePatternSample.Services;
using PipelinePatternSample.UseCases.Contexts;
using PipelinePatternSample.UseCases.Requests;
using PipelinePatternSample.UseCases.Responses;

namespace PipelinePatternSample.UseCases
{
    public class ApplyFilterToCloudMediaUseCase : IUseCase<ApplyFilterToCloudMediaRequest, ApplyFilterToCloudMediaResponse>
    {
        private readonly MediaPipelineBuilder<ImageProcessingContext> _pipelineSteps;

        public ApplyFilterToCloudMediaUseCase(ICloudStorageService cloudStorageService)
        {
            // Define a sequência dos steps
            _pipelineSteps = MediaPipelineBuilder<ImageProcessingContext>
                .Create()
                .AddStep(new DownloadMediaStep(cloudStorageService))
                .AddStep(new ApplyFilterStep())
                .AddStep(new AddMetadataStep())
                .AddStep(new SaveImageStep())
                .AddStep(new UploadMediaStep(cloudStorageService));
        }

        public async Task<ApplyFilterToCloudMediaResponse> ExecuteAsync(ApplyFilterToCloudMediaRequest request)
        {
            var context = new ImageProcessingContext
            {
                CloudImagePath = request.CloudImagePath,
                TempFolderPath = request.TempFolderPath,
                Image = request.Image,
                Metadata = request.Metadata
            };

            try
            {
                await _pipelineSteps.ProcessAsync(context);

                return new ApplyFilterToCloudMediaResponse
                {
                    Success = true,
                    FilteredCloudImagePath = context.CloudPath
                };
            }
            catch (Exception ex)
            {
                return new ApplyFilterToCloudMediaResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
