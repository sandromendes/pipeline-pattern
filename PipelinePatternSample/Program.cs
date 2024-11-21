// See https://aka.ms/new-console-template for more information
using PipelinePatternSample.Core.Builders;
using PipelinePatternSample.Domain;
using PipelinePatternSample.Pipelines;
using PipelinePatternSample.Services;

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
        var downloadService = new CloudStorageService(cloudFolder);

        const string uri = "cloud://path/to/sunset.jpg";

        var metadata = "Author: Alice | Description: Sunset Landscape";

        var photo = new Image("Sunset", uri, metadata);

        // Pipeline 1: Download -> Rotação -> Upload
        Console.WriteLine("\nPipeline 1: Download -> Rotate -> Upload");
        await MediaPipelineBuilder<Media>
            .Create()
            .AddStep(new DownloadMediaStep(downloadService, tempFolder))
            .AddStep(new RotateImageStep(90))
            .AddStep(new UploadMediaStep(downloadService))
            .ProcessAsync(photo);

        // Pipeline 2: Download -> Remasterização -> Upload
        Console.WriteLine("\nPipeline 2: Download -> Remaster -> Upload");
        await MediaPipelineBuilder<Media>
            .Create()
            .AddStep(new DownloadMediaStep(downloadService, tempFolder))
            .AddStep(new RemasterStep())
            .AddStep(new UploadMediaStep(downloadService))
            .ProcessAsync(photo);

        // Pipeline 3: Download -> Rotação -> Adiciona Metadados -> Aplica filtros -> Salva imagem na máquina -> Upload
        Console.WriteLine("\nProcessing Image...");
        await MediaPipelineBuilder<Media>
            .Create()
            .AddStep(new DownloadMediaStep(downloadService, tempFolder))
            .AddStep(new RotateImageStep(90))
            .AddStep(new AddMetadataStep("Author: Alice | Description: Sunset Landscape"))
            .AddStep(new ApplyFilterStep())
            .AddStep(new SaveImageStep(tempFolder))
            .AddStep(new UploadMediaStep(downloadService))
            .ProcessAsync(photo);
    }
}
