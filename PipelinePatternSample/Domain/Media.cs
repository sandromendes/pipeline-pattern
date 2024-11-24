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

        public void AppendSuffixToPath(string suffix)
        {
            if (string.IsNullOrEmpty(Path))
                throw new InvalidOperationException("Path is null or empty");

            var directory = System.IO.Path.GetDirectoryName(Path);
            var fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(Path);
            var extension = System.IO.Path.GetExtension(Path);

            Path = System.IO.Path.Combine(directory, $"{fileNameWithoutExtension}{suffix}{extension}");
        }
    }
}
