using PipelinePatternSample.Domain;
using PipelinePatternSample.Pipelines.Interfaces;

namespace PipelinePatternSample.Pipelines
{
    public class RotateImageStep : IAsyncPipelineStep<Media>
    {
        private readonly int _angle;

        public RotateImageStep(int angle) => _angle = angle;

        public async Task<Media> ProcessAsync(Media input)
        {
            Console.WriteLine($"Rotating '{input.Name}' by {_angle} degrees...");
           
            await Task.Delay(200); // Simula o processamento
            
            Console.WriteLine($"Rotation complete for '{input.Name}'.");
            
            return input;
        }
    }

}
