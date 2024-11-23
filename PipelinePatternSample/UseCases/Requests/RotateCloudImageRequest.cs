using PipelinePatternSample.Domain;

namespace PipelinePatternSample.UseCases.Requests
{
    public class RotateCloudImageRequest
    {
        public string CloudImagePath { get; set; }  // Caminho da imagem na nuvem
        public Image Image { get; set; }            // Imagem a ser rotacionada
        public int Angle { get; set; }              // Ângulo de rotação
        public string TempFolderPath { get; set; }  // Pasta temporária onde será salva a imagem
    }
}
