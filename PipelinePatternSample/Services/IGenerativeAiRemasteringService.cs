using PipelinePatternSample.Domain;

namespace PipelinePatternSample.Services
{
    public interface IGenerativeAiRemasteringService
    {
        Task<Image> RemasterImageAsync(Image inputImage);
    }

}
