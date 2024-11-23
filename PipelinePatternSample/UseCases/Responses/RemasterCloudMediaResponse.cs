namespace PipelinePatternSample.UseCases.Responses
{
    public class RemasterCloudMediaResponse
    {
        public bool Success { get; set; }
        public string RemasteredCloudImagePath { get; set; }
        public string ErrorMessage { get; set; }
    }

}
