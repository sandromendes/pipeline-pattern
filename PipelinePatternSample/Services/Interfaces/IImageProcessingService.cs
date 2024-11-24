using PipelinePatternSample.Domain;

namespace PipelinePatternSample.Services.Interfaces
{
    public interface IImageProcessingService
    {
        Task<Image> RotateImageAsync(Image image, int angle);
    }
}
