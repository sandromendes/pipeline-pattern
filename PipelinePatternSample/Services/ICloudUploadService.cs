namespace PipelinePatternSample.Services
{
    public interface ICloudUploadService
    {
        Task<string> DownloadAsync(string fileName, string tempFolder);
        Task UploadAsync(string localFilePath);
    }
}