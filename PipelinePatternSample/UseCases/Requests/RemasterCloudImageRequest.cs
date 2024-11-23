using PipelinePatternSample.Domain;

namespace PipelinePatternSample.UseCases.Requests
{
    public class RemasterCloudImageRequest
    {
        public string CloudImagePath { get; set; }
        public string TempFolderPath { get; set; }
        public Image Image { get; set; }
    }
}
