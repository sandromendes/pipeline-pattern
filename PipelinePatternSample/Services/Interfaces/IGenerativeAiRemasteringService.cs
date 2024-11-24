using PipelinePatternSample.Domain;

namespace PipelinePatternSample.Services.Interfaces
{
    public interface IGenerativeAiRemasteringService
    {
        Task<Image> RemasterImageAsync(Image inputImage);
    }

}
