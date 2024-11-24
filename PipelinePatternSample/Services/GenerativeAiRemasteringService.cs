using PipelinePatternSample.Domain;
using PipelinePatternSample.Services.Interfaces;

namespace PipelinePatternSample.Services
{
    public class GenerativeAiRemasteringService : IGenerativeAiRemasteringService
    {

        public async Task<Image> RemasterImageAsync(Image inputImage)
        {
            // Converte a imagem para o formato exigido pelo SDK
            var sdkInput = new SdkImage
            {
                Metadata = inputImage.Metadata
            };

            // Chama o SDK para remasterizar a imagem
            var sdkOutput = await ThirdPartyGenerativeAiSdk.RemasterAsync(sdkInput);

            // Converte o resultado do SDK de volta para a classe Image da aplicação
            return new Image(inputImage.Name, inputImage.Path, sdkOutput.Metadata);
        }
    }

    public static class ThirdPartyGenerativeAiSdk
    {
        public static async Task<SdkImage> RemasterAsync(SdkImage inputImage)
        {
            Console.WriteLine($"[SDK] Starting remastering process for image: {inputImage.Name}");

            // Simula o tempo de processamento da IA
            await Task.Delay(2000);

            Console.WriteLine($"[SDK] Remastering completed for image: {inputImage.Name}");

            // Simula a saída com ajustes na imagem
            var remasteredImage = new SdkImage
            {
                Name = inputImage.Name,
                Data = inputImage.Data, // Aqui você pode simular alterações, mas deixamos os dados intactos
                Metadata = inputImage.Metadata + " | Remastered"
            };

            return remasteredImage;
        }
    }

    public class SdkImage
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public string Metadata { get; set; }
    }
}
