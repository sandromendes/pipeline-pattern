namespace PipelinePatternAutomatedTests.Extensions
{
    public static class StringExtensions
    {
        public static string AddSuffix(this string imagePath, string suffix)
        {
            var extension = Path.GetExtension(imagePath);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(imagePath);
            var directory = Path.GetDirectoryName(imagePath);

            directory = directory?.Replace("\\", "/");

            return Path.Combine(directory ?? string.Empty, $"{fileNameWithoutExtension}_{suffix}{extension}")
                       .Replace("\\", "/"); // Para garantir consistência no formato
        }
    }
}
