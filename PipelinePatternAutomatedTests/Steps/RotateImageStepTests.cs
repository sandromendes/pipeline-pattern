using Moq;
using PipelinePatternSample.Domain;
using PipelinePatternSample.Pipelines;
using PipelinePatternSample.Services.Interfaces;
using PipelinePatternSample.UseCases.Contexts;

namespace PipelinePatternAutomatedTests.Steps
{
    public class RotateImageStepTests
    {
        [Fact]
        public async Task ProcessAsync_ShouldRotateImageSuccessfully()
        {
            // Arrange
            var mockImageService = new Mock<IImageProcessingService>();
            var originalImage = new Image("example.jpg", "/path/to/example.jpg", "Metadata");
            var rotatedImage = new Image("example.jpg", "/path/to/example_rotated.jpg", "Metadata");

            mockImageService
                .Setup(service => service.RotateImageAsync(It.IsAny<Image>(), It.IsAny<int>()))
                .ReturnsAsync(rotatedImage);

            var step = new RotateImageStep(mockImageService.Object);
            var context = new ImageProcessingContext
            {
                Image = originalImage,
                CloudImagePath = "path/to/example.jpg",
                RotationAngle = 90
            };

            // Act
            var result = await step.ProcessAsync(context);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(rotatedImage.Path, result.Image.Path);
            Assert.Equal(rotatedImage.Metadata, result.Image.Metadata);
            Assert.Equal(rotatedImage.Name, result.Image.Name);

            // Verifica se o serviço foi chamado corretamente
            mockImageService.Verify(service => service.RotateImageAsync(originalImage, 90), Times.Once);
        }

        [Fact]
        public async Task ProcessAsync_ShouldHandleInvalidRotationAngleAppropriately()
        {
            // Arrange
            var mockImageService = new Mock<IImageProcessingService>();
            var originalImage = new Image("example.jpg", "/path/to/example.jpg", "Metadata");

            mockImageService
                .Setup(service => service.RotateImageAsync(It.IsAny<Image>(), It.IsAny<int>()))
                .ThrowsAsync(new ArgumentException("Invalid rotation angle"));

            var step = new RotateImageStep(mockImageService.Object);
            var context = new ImageProcessingContext
            {
                Image = originalImage,
                RotationAngle = -45 // Ângulo inválido
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => step.ProcessAsync(context));
            Assert.Equal("Invalid rotation angle", exception.Message);

            // Verifica se o serviço foi chamado com os parâmetros esperados
            mockImageService.Verify(service => service.RotateImageAsync(originalImage, -45), Times.Once);
        }

        [Fact]
        public async Task ProcessAsync_ShouldNotAlterOriginalImageOnError()
        {
            // Arrange
            var mockImageService = new Mock<IImageProcessingService>();
            var originalImage = new Image("example.jpg", "/path/to/example.jpg", "Metadata");

            mockImageService
                .Setup(service => service.RotateImageAsync(It.IsAny<Image>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Rotation failed"));

            var step = new RotateImageStep(mockImageService.Object);
            var context = new ImageProcessingContext
            {
                Image = originalImage,
                RotationAngle = 90
            };

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => step.ProcessAsync(context));

            // Assert
            Assert.Equal("Rotation failed", exception.Message);
            Assert.Equal(originalImage, context.Image); // A imagem original não deve ser alterada
        }
    }
}
