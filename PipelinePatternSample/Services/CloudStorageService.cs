namespace PipelinePatternSample.Services
{
    public class CloudStorageService : ICloudStorageService
    {
        private readonly string _cloudFolder;

        public CloudStorageService(string cloudFolder)
        {
            _cloudFolder = cloudFolder;
        }

        public async Task<string> DownloadAsync(string fileName, string tempFolder)
        {
            Console.WriteLine($"Simulating download of '{fileName}' from cloud storage...");
            await Task.Delay(500); // Simula o tempo de download

            var tempPath = Path.Combine(tempFolder, fileName);

            Console.WriteLine($"Downloaded '{fileName}' to '{tempPath}'");
            return tempPath;
        }

        public async Task UploadAsync(string localFilePath)
        {
            Console.WriteLine($"Simulating upload of '{localFilePath}' to cloud storage...");
            await Task.Delay(400); // Simula o tempo de upload

            var fileName = Path.GetFileName(localFilePath);
            var destinationPath = Path.Combine(_cloudFolder, fileName);

            Console.WriteLine($"Uploaded '{localFilePath}' to cloud as '{destinationPath}'");
        }
    }
}
