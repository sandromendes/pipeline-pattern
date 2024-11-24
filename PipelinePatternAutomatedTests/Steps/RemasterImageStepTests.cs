using Moq;
using PipelinePatternSample.Domain;
using PipelinePatternSample.Pipelines;
using PipelinePatternSample.Services.Interfaces;
using PipelinePatternSample.UseCases.Contexts;

namespace PipelinePatternAutomatedTests.Steps
{
    public class RemasterImageStepTests
    {
        [Fact]
        public async Task ProcessAsync_ShouldRemasterImageSuccessfully()
        {
            // Arrange
            var mockAiService = new Mock<IGenerativeAiRemasteringService>();
            var originalImage = new Image("example.jpg", "/path/to/example.jpg", "Original Metadata");
            var remasteredImage = new Image("example.jpg", "/path/to/remastered_example.jpg", "Remastered Metadata");

            mockAiService
                .Setup(service => service.RemasterImageAsync(It.IsAny<Image>()))
                .ReturnsAsync(remasteredImage);

            var step = new RemasterImageStep(mockAiService.Object);
            var context = new ImageProcessingContext
            {
                Image = originalImage,
                CloudImagePath = "path/to/example.jpg"
            };

            // Act
            var result = await step.ProcessAsync(context);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Image);
            Assert.Equal(remasteredImage.Path, result.Image.Path);
            Assert.Equal(remasteredImage.Metadata, result.Image.Metadata);

            // Verifica se o serviço de IA foi chamado com a imagem original
            mockAiService.Verify(service => service.RemasterImageAsync(originalImage), Times.Once);
        }

        [Fact]
        public async Task ProcessAsync_ShouldHandleExceptionFromAiService()
        {
            // Arrange
            var mockAiService = new Mock<IGenerativeAiRemasteringService>();
            var originalImage = new Image("example.jpg", "/path/to/example.jpg", "Original Metadata");

            mockAiService
                .Setup(service => service.RemasterImageAsync(It.IsAny<Image>()))
                .ThrowsAsync(new Exception("AI service failed"));

            var step = new RemasterImageStep(mockAiService.Object);
            var context = new ImageProcessingContext
            {
                Image = originalImage
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => step.ProcessAsync(context));
            Assert.Equal("AI service failed", exception.Message);

            // Verifica se o serviço de IA foi chamado com a imagem original
            mockAiService.Verify(service => service.RemasterImageAsync(originalImage), Times.Once);
        }

        [Fact]
        public async Task ProcessAsync_ShouldNotAlterOriginalImageIfRemasteringFails()
        {
            // Arrange
            var mockAiService = new Mock<IGenerativeAiRemasteringService>();
            var originalImage = new Image("example.jpg", "/path/to/example.jpg", "Original Metadata");

            mockAiService
                .Setup(service => service.RemasterImageAsync(It.IsAny<Image>()))
                .ThrowsAsync(new Exception("AI service failed"));

            var step = new RemasterImageStep(mockAiService.Object);
            var context = new ImageProcessingContext
            {
                Image = originalImage
            };

            // Act
            Exception ex = await Assert.ThrowsAsync<Exception>(() => step.ProcessAsync(context));

            // Assert
            Assert.Equal("AI service failed", ex.Message);
            Assert.Equal(originalImage, context.Image); // A imagem original permanece inalterada
        }
    }
}
