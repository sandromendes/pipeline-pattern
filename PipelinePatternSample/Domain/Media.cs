namespace PipelinePatternSample.Domain
{
    public abstract class Media
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Metadata { get; set; }

        protected Media(string name, string path, string metadata)
        {
            Name = name;
            Path = path;
            Metadata = metadata;
        }

        public abstract void DisplayInfo();
    }
}
