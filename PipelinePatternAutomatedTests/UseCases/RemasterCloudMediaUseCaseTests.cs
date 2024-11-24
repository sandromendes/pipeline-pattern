using Moq;
using PipelinePatternSample.Domain;
using PipelinePatternSample.Services.Interfaces;
using PipelinePatternSample.UseCases;
using PipelinePatternSample.UseCases.Requests;

namespace PipelinePatternAutomatedTests.UseCases
{
    public class RemasterCloudMediaUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_ShouldReturnSuccess_WhenRemasteringSucceeds()
        {
            // Arrange
            var cloudStorageServiceMock = new Mock<ICloudStorageService>();
            var aiRemasteringServiceMock = new Mock<IGenerativeAiRemasteringService>();

            // Mock da resposta do serviço de remasterização
            var originalImage = new Image("test_image.jpg", "path/image", "Old metadata");
            var remasteredImage = new Image("test_image.jpg", "path/image", "Remastered metadata");
            aiRemasteringServiceMock
                .Setup(service => service.RemasterImageAsync(It.IsAny<Image>()))
                .ReturnsAsync(remasteredImage);

            // Mock do serviço de download e upload
            cloudStorageServiceMock.Setup(service => service.DownloadAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("path/downloaded/image");

            cloudStorageServiceMock.Setup(service => service.UploadAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var useCase = new RemasterCloudMediaUseCase(cloudStorageServiceMock.Object, aiRemasteringServiceMock.Object);
            var request = new RemasterCloudImageRequest
            {
                CloudImagePath = "cloud/path/image.jpg",
                TempFolderPath = "temp/folder",
                Image = originalImage
            };

            // Act
            var result = await useCase.ExecuteAsync(request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(AddRemasteredSuffix("cloud/path/image.jpg"), result.RemasteredCloudImagePath);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFailure_WhenRemasteringFails()
        {
            // Arrange
            var cloudStorageServiceMock = new Mock<ICloudStorageService>();
            var aiRemasteringServiceMock = new Mock<IGenerativeAiRemasteringService>();

            // Simula a falha no processo de remasterização
            aiRemasteringServiceMock
                .Setup(service => service.RemasterImageAsync(It.IsAny<Image>()))
                .ThrowsAsync(new Exception("Remastering service failed"));

            // Mock do serviço de download
            cloudStorageServiceMock.Setup(service => service.DownloadAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("path/to/downloaded/image");

            var useCase = new RemasterCloudMediaUseCase(cloudStorageServiceMock.Object, aiRemasteringServiceMock.Object);
            var request = new RemasterCloudImageRequest
            {
                CloudImagePath = "cloud/path/image.jpg",
                TempFolderPath = "temp/folder",
                Image = new Image("test_image.jpg", "path/to/image", "Old metadata")
            };

            // Act
            var result = await useCase.ExecuteAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Remastering service failed", result.ErrorMessage);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFailure_WhenDownloadFails()
        {
            // Arrange
            var cloudStorageServiceMock = new Mock<ICloudStorageService>();
            var aiRemasteringServiceMock = new Mock<IGenerativeAiRemasteringService>();

            // Simula uma falha no download
            cloudStorageServiceMock.Setup(service => service.DownloadAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Download failed"));

            var useCase = new RemasterCloudMediaUseCase(cloudStorageServiceMock.Object, aiRemasteringServiceMock.Object);
            var request = new RemasterCloudImageRequest
            {
                CloudImagePath = "cloud/path/image.jpg",
                TempFolderPath = "temp/folder",
                Image = new Image("test_image.jpg", "path/to/image", "Old metadata")
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
            var aiRemasteringServiceMock = new Mock<IGenerativeAiRemasteringService>();

            // Simula uma falha no upload
            cloudStorageServiceMock.Setup(service => service.UploadAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Upload failed"));

            // Mock do serviço de remasterização
            var originalImage = new Image("test_image.jpg", "path/to/image", "Old metadata");
            var remasteredImage = new Image("test_image.jpg", "path/to/image", "Remastered metadata");
            aiRemasteringServiceMock
                .Setup(service => service.RemasterImageAsync(It.IsAny<Image>()))
                .ReturnsAsync(remasteredImage);

            // Mock do serviço de download
            cloudStorageServiceMock.Setup(service => service.DownloadAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("path/to/downloaded/image");

            var useCase = new RemasterCloudMediaUseCase(cloudStorageServiceMock.Object, aiRemasteringServiceMock.Object);
            var request = new RemasterCloudImageRequest
            {
                CloudImagePath = "cloud/path/image.jpg",
                TempFolderPath = "temp/folder",
                Image = originalImage
            };

            // Act
            var result = await useCase.ExecuteAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Upload failed", result.ErrorMessage);
        }

        private static string AddRemasteredSuffix(string cloudImagePath)
        {
            var extension = Path.GetExtension(cloudImagePath);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(cloudImagePath);
            var directory = Path.GetDirectoryName(cloudImagePath);

            directory = directory?.Replace("\\", "/");

            return Path.Combine(directory, $"{fileNameWithoutExtension}_remastered{extension}");
        }
    }
}
