using PipelinePatternSample.Domain;

namespace PipelinePatternSample.Services
{
    public class ImageProcessingService : IImageProcessingService
    {
        public async Task<Image> RotateImageAsync(Image image, int angle)
        {
            Console.WriteLine($"Rotating image '{image}' by {angle} degrees...");


            await Task.Delay(500); // Simula o processamento
            Console.WriteLine($"Image rotated and saved to: {image.Path}");

            return image;
        }
    }
}
