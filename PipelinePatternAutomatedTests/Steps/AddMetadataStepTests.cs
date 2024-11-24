using PipelinePatternSample.Domain;
using PipelinePatternSample.Pipelines;
using PipelinePatternSample.UseCases.Contexts;

namespace PipelinePatternAutomatedTests.Steps
{
    public class AddMetadataStepTests
    {
        [Fact]
        public async Task ProcessAsync_ShouldAddMetadataToImage()
        {
            // Arrange
            var step = new AddMetadataStep();
            var context = new ImageProcessingContext
            {
                Image = new Image("example.jpg", "/path/to/example.jpg", string.Empty),
                Metadata = "Author: Alice | Description: Sunset Landscape"
            };

            // Act
            var result = await step.ProcessAsync(context);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("example.jpg", result.Image.Name);
            Assert.Equal("Author: Alice | Description: Sunset Landscape", result.Image.Metadata);
        }

        [Fact]
        public async Task ProcessAsync_ShouldHandleEmptyMetadataAppropriately()
        {
            // Arrange
            var step = new AddMetadataStep();
            var context = new ImageProcessingContext
            {
                Image = new Image("example.jpg", "/path/to/example.jpg", string.Empty),
                Metadata = string.Empty
            };

            // Act
            var result = await step.ProcessAsync(context);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("example.jpg", result.Image.Name);
            Assert.Equal(string.Empty, result.Image.Metadata);
        }

        [Fact]
        public async Task ProcessAsync_ShouldPreserveExistingImageName()
        {
            // Arrange
            var step = new AddMetadataStep();
            var context = new ImageProcessingContext
            {
                Image = new Image("test_image.jpg", "/path/to/test_image.jpg", "Old Metadata"),
                Metadata = "New Metadata"
            };

            // Act
            var result = await step.ProcessAsync(context);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test_image.jpg", result.Image.Name);
            Assert.Equal("New Metadata", result.Image.Metadata);
        }
    }
}
