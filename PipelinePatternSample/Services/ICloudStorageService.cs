namespace PipelinePatternSample.Services
{
    public interface ICloudStorageService
    {
        Task<string> DownloadAsync(string fileName, string tempFolder);
        Task UploadAsync(string localFilePath);
    }
}