using PipelinePatternSample.Domain;
using PipelinePatternSample.Pipelines;
using PipelinePatternSample.UseCases.Contexts;

namespace PipelinePatternAutomatedTests.Steps
{
    public class SaveImageStepTests
    {
        [Fact]
        public async Task ProcessAsync_ShouldSaveImageWithCorrectPath()
        {
            // Arrange
            var step = new SaveImageStep();
            var context = new ImageProcessingContext
            {
                Image = new Image("example.png", string.Empty, "Sample metadata"),
                LocalImagePath = "/local/temp"
            };

            // Act
            var result = await step.ProcessAsync(context);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("/local/temp/example.png", result.Image.Path);
            Assert.Equal("example.png", result.Image.Name);
        }

        [Fact]
        public async Task ProcessAsync_ShouldHandleEmptyLocalImagePath()
        {
            // Arrange
            var step = new SaveImageStep();
            var context = new ImageProcessingContext
            {
                Image = new Image("example.png", string.Empty, "Sample metadata"),
                LocalImagePath = string.Empty // Caminho local vazio
            };

            // Act
            var result = await step.ProcessAsync(context);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("/example.png", result.Image.Path); // Verifica o comportamento com caminho local vazio
        }

        [Fact]
        public async Task ProcessAsync_ShouldPreserveImageMetadata()
        {
            // Arrange
            var step = new SaveImageStep();
            var context = new ImageProcessingContext
            {
                Image = new Image("example", string.Empty, "Sample metadata"),
                LocalImagePath = "/local/temp"
            };

            // Act
            var result = await step.ProcessAsync(context);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Sample metadata", result.Image.Metadata); // Metadata permanece inalterada
        }

        [Fact]
        public async Task ProcessAsync_ShouldHandleNullImageAppropriately()
        {
            // Arrange
            var step = new SaveImageStep();
            var context = new ImageProcessingContext
            {
                Image = null,
                LocalImagePath = "/local/temp"
            };

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => step.ProcessAsync(context));
        }
    }
}
