using PipelinePatternSample.Pipelines;
using PipelinePatternSample.Pipelines.Builders;
using PipelinePatternSample.Services.Interfaces;
using PipelinePatternSample.UseCases.Contexts;
using PipelinePatternSample.UseCases.Interfaces;
using PipelinePatternSample.UseCases.Requests;
using PipelinePatternSample.UseCases.Responses;

namespace PipelinePatternSample.UseCases
{
    public class RotateCloudImageUseCase : IUseCase<RotateCloudImageRequest, RotateCloudImageResponse>
    {
        private readonly MediaPipelineBuilder<ImageProcessingContext> _pipelineSteps;

        public RotateCloudImageUseCase(ICloudStorageService cloudStorageService, 
            IImageProcessingService imageProcessingService)
        {
            // Define a sequência dos steps
            _pipelineSteps = MediaPipelineBuilder<ImageProcessingContext>
                .Create()
                .AddStep(new DownloadMediaStep(cloudStorageService))
                .AddStep(new RotateImageStep(imageProcessingService))
                .AddStep(new UploadMediaStep(cloudStorageService));
        }

        public async Task<RotateCloudImageResponse> ExecuteAsync(RotateCloudImageRequest request)
        {
            var context = new ImageProcessingContext
            {
                CloudImagePath = request.CloudImagePath,
                RotationAngle = request.Angle,
                TempFolderPath = request.TempFolderPath,
                Image = request.Image
            };

            try
            {
                await _pipelineSteps.ProcessAsync(context);

                return new RotateCloudImageResponse
                {
                    Success = true,
                    RotatedCloudImagePath = context.CloudImagePath
                };
            }
            catch (Exception ex)
            {
                return new RotateCloudImageResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }

}
