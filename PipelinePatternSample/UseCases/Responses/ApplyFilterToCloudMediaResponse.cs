namespace PipelinePatternSample.UseCases.Responses
{
    public class ApplyFilterToCloudMediaResponse
    {
        public bool Success { get; set; }
        public string FilteredCloudImagePath { get; set; }
        public string ErrorMessage { get; set; }
    }

}
