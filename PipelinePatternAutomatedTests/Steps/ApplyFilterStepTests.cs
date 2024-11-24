using System.Threading.Tasks;
using PipelinePatternSample.Domain;
using PipelinePatternSample.Pipelines;
using PipelinePatternSample.UseCases.Contexts;
using Xunit;

namespace PipelinePatternAutomatedTests.Steps
{
    public class ApplyFilterStepTests
    {
        [Fact]
        public async Task ProcessAsync_ShouldReturnContextUnchangedExceptForLogMessages()
        {
            // Arrange
            var step = new ApplyFilterStep();
            var initialImage = new Image("test_image.jpg", "/path/to/test_image.jpg", "Test Metadata");
            var context = new ImageProcessingContext
            {
                CloudImagePath = "/cloud/test_image.jpg",
                LocalImagePath = "/local/test_image.jpg",
                TempFolderPath = "/temp",
                Image = initialImage
            };

            // Act
            var result = await step.ProcessAsync(context);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(context.CloudImagePath, result.CloudImagePath);
            Assert.Equal(context.LocalImagePath, result.LocalImagePath);
            Assert.Equal(context.TempFolderPath, result.TempFolderPath);
            Assert.Equal(initialImage, result.Image);
        }

        [Fact]
        public async Task ProcessAsync_ShouldHandleNullImageAppropriately()
        {
            // Arrange
            var step = new ApplyFilterStep();
            var context = new ImageProcessingContext
            {
                CloudImagePath = "/cloud/test_image.jpg",
                LocalImagePath = "/local/test_image.jpg",
                TempFolderPath = "/temp",
                Image = null
            };

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => step.ProcessAsync(context));
        }

        [Fact]
        public async Task ProcessAsync_ShouldSimulateFilterApplication()
        {
            // Arrange
            var step = new ApplyFilterStep();
            var initialImage = new Image("example.jpg", "/path/to/example.jpg", "Original Metadata");
            var context = new ImageProcessingContext
            {
                CloudImagePath = "/cloud/example.jpg",
                LocalImagePath = "/local/example.jpg",
                TempFolderPath = "/temp",
                Image = initialImage
            };

            // Act
            var result = await step.ProcessAsync(context);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("example.jpg", result.Image.Name);
            Assert.Equal("/path/to/example.jpg", result.Image.Path);
            Assert.Equal("Original Metadata", result.Image.Metadata);
        }
    }
}
