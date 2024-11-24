using PipelinePatternSample.Pipelines.Interfaces;

namespace PipelinePatternSample.Pipelines.Builders
{
    public class MediaPipelineBuilder<T>
    {
        private readonly List<IAsyncPipelineStep<T>> _steps = new();

        public static MediaPipelineBuilder<T> Create() => new();

        public MediaPipelineBuilder<T> AddStep(IAsyncPipelineStep<T> step)
        {
            _steps.Add(step);

            return this;
        }

        public async Task<T> ProcessAsync(T input)
        {
            foreach (var step in _steps)
            {
                input = await step.ProcessAsync(input);
            }
            return input;
        }
    }
}
