namespace PipelinePatternSample.Pipelines.Interfaces
{
    public interface IAsyncPipelineStep<T> 
    {
        Task<T> ProcessAsync(T input);
    }
}
