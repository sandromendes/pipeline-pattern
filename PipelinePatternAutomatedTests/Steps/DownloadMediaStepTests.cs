using Moq;
using PipelinePatternSample;
using PipelinePatternSample.Domain;
using PipelinePatternSample.Services.Interfaces;
using PipelinePatternSample.UseCases.Contexts;

namespace PipelinePatternAutomatedTests.Steps
{
    public class DownloadMediaStepTests
    {
        [Fact]
        public async Task ProcessAsync_ShouldDownloadMediaAndSetPath()
        {
            // Arrange
            var mockCloudService = new Mock<ICloudStorageService>();
            mockCloudService
                .Setup(service => service.DownloadAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("/temp/example.jpg"); // Simula o caminho do arquivo baixado

            var step = new DownloadMediaStep(mockCloudService.Object);
            var context = new ImageProcessingContext
            {
                Image = new Image("example.jpg", null, "Test Metadata"),
                TempFolderPath = "/temp"
            };

            // Act
            var result = await step.ProcessAsync(context);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Image.Path);
            Assert.Equal("/temp/example.jpg", result.Image.Path);

            // Verifica se o serviço de download foi chamado com os argumentos corretos
            mockCloudService.Verify(service => service.DownloadAsync("example.jpg", "/temp"), Times.Once);
        }

        [Fact]
        public async Task ProcessAsync_ShouldHandleEmptyTempFolderAppropriately()
        {
            // Arrange
            var mockCloudService = new Mock<ICloudStorageService>();
            mockCloudService
                .Setup(service => service.DownloadAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("/default/example.jpg");

            var step = new DownloadMediaStep(mockCloudService.Object);
            var context = new ImageProcessingContext
            {
                Image = new Image("example.jpg", null, "Test Metadata"),
                TempFolderPath = string.Empty // Caminho vazio
            };

            // Act
            var result = await step.ProcessAsync(context);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Image.Path);
            Assert.Equal("/default/example.jpg", result.Image.Path);

            // Verifica se o serviço de download foi chamado
            mockCloudService.Verify(service => service.DownloadAsync("example.jpg", string.Empty), Times.Once);
        }

        [Fact]
        public async Task ProcessAsync_ShouldThrowExceptionIfCloudServiceFails()
        {
            // Arrange
            var mockCloudService = new Mock<ICloudStorageService>();
            mockCloudService
                .Setup(service => service.DownloadAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Cloud service unavailable"));

            var step = new DownloadMediaStep(mockCloudService.Object);
            var context = new ImageProcessingContext
            {
                Image = new Image("example.jpg", null, "Test Metadata"),
                TempFolderPath = "/temp"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => step.ProcessAsync(context));
            Assert.Equal("Cloud service unavailable", exception.Message);

            // Verifica se o serviço de download foi chamado
            mockCloudService.Verify(service => service.DownloadAsync("example.jpg", "/temp"), Times.Once);
        }
    }
}
