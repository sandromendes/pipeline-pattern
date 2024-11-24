using PipelinePatternSample.Domain;

namespace PipelinePatternSample.UseCases.Requests
{
    public class ApplyFilterToCloudMediaRequest
    {
        public string CloudImagePath { get; set; }
        public string TempFolderPath { get; set; }
        public Image Image { get; set; }
        public string Metadata { get; set; }
    }

}
