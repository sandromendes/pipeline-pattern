using PipelinePatternSample.Pipelines.Interfaces;
using PipelinePatternSample.Services;
using PipelinePatternSample.UseCases.Contexts;

namespace PipelinePatternSample.Pipelines
{
    public class RemasterImageStep : IAsyncPipelineStep<ImageProcessingContext>
    {
        private readonly IGenerativeAiRemasteringService _aiRemasteringService;

        public RemasterImageStep(IGenerativeAiRemasteringService aiRemasteringService)
        {
            _aiRemasteringService = aiRemasteringService;
        }

        public async Task<ImageProcessingContext> ProcessAsync(ImageProcessingContext input)
        {
            Console.WriteLine($"Starting remastering process for media '{input.Image.Name}'...");

            try
            {
                // Chamada para o serviço de IA para remasterizar a imagem
                var remasteredImage = await _aiRemasteringService.RemasterImageAsync(input.Image);

                // Atualiza o contexto com a imagem remasterizada
                input.Image = remasteredImage;

                Console.WriteLine($"Remastering completed for media '{input.Image.Name}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to remaster media '{input.Image.Name}': {ex.Message}");
                throw; // Propaga a exceção para ser tratada no pipeline
            }

            return input;
        }
    }
}
