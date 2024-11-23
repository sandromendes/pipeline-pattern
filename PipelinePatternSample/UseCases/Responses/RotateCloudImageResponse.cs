namespace PipelinePatternSample.UseCases.Responses
{
    public class RotateCloudImageResponse
    {
        public bool Success { get; set; }
        public string RotatedCloudImagePath { get; set; } // Caminho da imagem rotacionada na nuvem
        public string ErrorMessage { get; set; }         // Mensagem de erro, caso ocorra
    }
}
