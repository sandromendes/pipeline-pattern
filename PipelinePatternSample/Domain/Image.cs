namespace PipelinePatternSample.Domain
{
    public class Image : Media
    {
        public Image(string name, string path, string metadata) : base(name, path, metadata) { }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Photo: {Name}, Path: {Path}");
        }
    }
}
