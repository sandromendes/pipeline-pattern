using PipelinePatternSample.Core.Builders;
using PipelinePatternSample.Pipelines;
using PipelinePatternSample.Services;
using PipelinePatternSample.UseCases.Contexts;
using PipelinePatternSample.UseCases.Requests;
using PipelinePatternSample.UseCases.Responses;

namespace PipelinePatternSample.UseCases
{
    public class RemasterCloudMediaUseCase : IUseCase<RemasterCloudImageRequest, RemasterCloudMediaResponse>
    {
        private readonly MediaPipelineBuilder<ImageProcessingContext> _pipelineSteps;

        public RemasterCloudMediaUseCase(ICloudStorageService cloudStorageService, IGenerativeAiRemasteringService generativeAiRemasteringService)
        {
            // Define a sequência dos steps
            _pipelineSteps = MediaPipelineBuilder<ImageProcessingContext>
                .Create()
                .AddStep(new DownloadMediaStep(cloudStorageService))
                .AddStep(new RemasterImageStep(generativeAiRemasteringService))
                .AddStep(new UploadMediaStep(cloudStorageService));
        }

        public async Task<RemasterCloudMediaResponse> ExecuteAsync(RemasterCloudImageRequest request)
        {
            var context = new ImageProcessingContext
            {
                CloudImagePath = request.CloudImagePath,
                TempFolderPath = request.TempFolderPath,
                Image = request.Image
            };

            try
            {
                await _pipelineSteps.ProcessAsync(context);

                return new RemasterCloudMediaResponse
                {
                    Success = true,
                    RemasteredCloudImagePath = context.CloudPath
                };
            }
            catch (Exception ex)
            {
                return new RemasterCloudMediaResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
