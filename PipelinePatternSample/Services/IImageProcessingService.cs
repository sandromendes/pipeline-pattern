using PipelinePatternSample.Domain;

namespace PipelinePatternSample.Services
{
    public interface IImageProcessingService
    {
        Task<Image> RotateImageAsync(Image image, int angle);
    }
}
