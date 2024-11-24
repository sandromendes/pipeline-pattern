using Moq;
using PipelinePatternSample.UseCases.Requests;
using PipelinePatternSample.UseCases;
using PipelinePatternSample.Domain;
using PipelinePatternSample.Pipelines;
using PipelinePatternSample.UseCases.Contexts;
using PipelinePatternSample.Services.Interfaces;

namespace PipelinePatternAutomatedTests.UseCases
{
    public class ApplyFilterToCloudMediaUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_ShouldReturnSuccess_WhenAllStepsSucceed()
        {
            // Arrange
            var cloudStorageServiceMock = new Mock<ICloudStorageService>();

            // Mock do serviço de download e upload
            cloudStorageServiceMock.Setup(service => service.DownloadAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("path/to/downloaded/image");
            cloudStorageServiceMock.Setup(service => service.UploadAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Imagem de exemplo
            var originalImage = new Image("test_image.jpg", "path/to/image", "Old metadata");

            var useCase = new ApplyFilterToCloudMediaUseCase(cloudStorageServiceMock.Object);

            var request = new ApplyFilterToCloudMediaRequest
            {
                CloudImagePath = "cloud/path/image.jpg",
                TempFolderPath = "temp/folder",
                Image = originalImage,
                Metadata = "Some metadata"
            };

            // Act
            var result = await useCase.ExecuteAsync(request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("cloud/path/image.jpg", result.FilteredCloudImagePath);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFailure_WhenDownloadFails()
        {
            // Arrange
            var cloudStorageServiceMock = new Mock<ICloudStorageService>();

            // Simula uma falha no download
            cloudStorageServiceMock.Setup(service => service.DownloadAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Download failed"));

            var useCase = new ApplyFilterToCloudMediaUseCase(cloudStorageServiceMock.Object);
            var request = new ApplyFilterToCloudMediaRequest
            {
                CloudImagePath = "cloud/path/image.jpg",
                TempFolderPath = "temp/folder",
                Image = new Image("test_image.jpg", "path/to/image", "Old metadata"),
                Metadata = "Some metadata"
            };

            // Act
            var result = await useCase.ExecuteAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Download failed", result.ErrorMessage);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFailure_WhenUploadFails()
        {
            // Arrange
            var cloudStorageServiceMock = new Mock<ICloudStorageService>();

            // Mock do serviço de download
            cloudStorageServiceMock.Setup(service => service.DownloadAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("path/to/downloaded/image");

            // Mock do serviço de upload
            cloudStorageServiceMock.Setup(service => service.UploadAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Upload failed"));

            var useCase = new ApplyFilterToCloudMediaUseCase(cloudStorageServiceMock.Object);
            var request = new ApplyFilterToCloudMediaRequest
            {
                CloudImagePath = "cloud/path/image.jpg",
                TempFolderPath = "temp/folder",
                Image = new Image("test_image.jpg", "path/to/image", "Old metadata"),
                Metadata = "Some metadata"
            };

            // Act
            var result = await useCase.ExecuteAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Upload failed", result.ErrorMessage);
        }

    }
}

