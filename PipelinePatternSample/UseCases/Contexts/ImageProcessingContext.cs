using PipelinePatternSample.Domain;

namespace PipelinePatternSample.UseCases.Contexts
{
    public class ImageProcessingContext
    {
        public string CloudImagePath { get; set; }
        public string LocalImagePath { get; set; }
        public string RotatedLocalPath { get; set; }
        public string CloudPath { get; set; }
        public string TempFolderPath { get; set; }
        public int RotationAngle { get; set; }
        public string Metadata { get; set; }
        public Image Image { get; set; }
    }
}
