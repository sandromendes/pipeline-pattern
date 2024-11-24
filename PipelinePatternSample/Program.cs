using PipelinePatternSample.Domain;
using PipelinePatternSample.Services;
using PipelinePatternSample.UseCases;
using PipelinePatternSample.UseCases.Requests;

public class Program
{
    static async Task Main()
    {
        // Simulação de diretórios
        string cloudFolder = "cloud_storage";
        string tempFolder = "temp_storage";
        Directory.CreateDirectory(cloudFolder);
        Directory.CreateDirectory(tempFolder);

        // Criando um arquivo de exemplo na "nuvem"
        string cloudFilePath = Path.Combine(cloudFolder, "landscape.jpg");

        File.WriteAllText(cloudFilePath, "This is a simulated image file."); // Conteúdo fictício

        // Inicializando serviços
        var cloudStorageService = new CloudStorageService(cloudFolder);
        var imageProcessingService = new ImageProcessingService();
        var generativeAiService = new GenerativeAiRemasteringService();

        const string uri = "cloud://path/to/sunset.jpg";

        var metadata = "Author: Alice | Description: Sunset Landscape";

        var photo = new Image("Sunset", uri, metadata);

        // Pipeline 1: Download -> Rotação -> Upload
        Console.WriteLine("\nPipeline 1: Download -> Rotate -> Upload");

        var rotateUseCase = new RotateCloudImageUseCase(cloudStorageService, imageProcessingService);

        var rotateCloudImageRequest = new RotateCloudImageRequest
        {
            Angle = 90,
            TempFolderPath = tempFolder,
            CloudImagePath = cloudFilePath,
            Image = photo
        };

        await rotateUseCase.ExecuteAsync(rotateCloudImageRequest);

        // Pipeline 2: Download -> Remasterização -> Upload
        Console.WriteLine("\nPipeline 2: Download -> Remaster -> Upload");

        var remasterCloudImageUseCase = new RemasterCloudMediaUseCase(cloudStorageService, generativeAiService);

        var remasterCloudImageRequest = new RemasterCloudImageRequest
        {
            CloudImagePath = cloudFilePath,
            TempFolderPath = tempFolder,
            Image = photo
        };

        await remasterCloudImageUseCase.ExecuteAsync(remasterCloudImageRequest);

        // Pipeline 3: Download -> Aplica filtros -> Adiciona Metadados -> Salva imagem na máquina -> Upload
        Console.WriteLine("\nProcessing Image...");

        var applyFilterCloudImageUseCase = new ApplyFilterToCloudMediaUseCase(cloudStorageService);

        var applyFilterCloudImageRequest = new ApplyFilterToCloudMediaRequest
        {
            Image = photo,
            CloudImagePath = cloudFilePath,
            TempFolderPath = tempFolder,
            Metadata = metadata
        };

        await applyFilterCloudImageUseCase.ExecuteAsync(applyFilterCloudImageRequest);
    }
}
