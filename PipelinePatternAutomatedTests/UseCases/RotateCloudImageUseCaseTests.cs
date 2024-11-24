using Moq;
using PipelinePatternSample.Domain;
using PipelinePatternSample.Services.Interfaces;
using PipelinePatternSample.UseCases;
using PipelinePatternSample.UseCases.Requests;

namespace PipelinePatternAutomatedTests.UseCases
{
    public class RotateCloudImageUseCaseTests
    {
        private readonly Mock<ICloudStorageService> _cloudStorageServiceMock;
        private readonly Mock<IImageProcessingService> _imageProcessingServiceMock;

        public RotateCloudImageUseCaseTests()
        {
            _cloudStorageServiceMock = new Mock<ICloudStorageService>();
            _imageProcessingServiceMock = new Mock<IImageProcessingService>();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnSuccess_WhenPipelineSucceeds()
        {
            // Arrange
            var request = new RotateCloudImageRequest
            {
                CloudImagePath = "cloud/image/image.jpg",
                Angle = 90,
                TempFolderPath = "/temp/folder",
                Image = new Image("image.jpg", "/local/path/image.jpg", "Some metadata")
            };

            _cloudStorageServiceMock.Setup(s => s.DownloadAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("/local/path/image.jpg");
            _cloudStorageServiceMock.Setup(s => s.UploadAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            _imageProcessingServiceMock.Setup(s => s.RotateImageAsync(It.IsAny<Image>(), It.IsAny<int>())).ReturnsAsync(new Image("image_rotated.jpg", "/local/path/image_rotated.jpg", "Some metadata"));

            var useCase = new RotateCloudImageUseCase(_cloudStorageServiceMock.Object, _imageProcessingServiceMock.Object);

            // Act
            var response = await useCase.ExecuteAsync(request);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(AddRotatedSuffix(request.CloudImagePath), response.RotatedCloudImagePath);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFailure_WhenDownloadFails()
        {
            // Arrange
            var request = new RotateCloudImageRequest
            {
                CloudImagePath = "cloud/image/path.jpg",
                Angle = 90,
                TempFolderPath = "/temp/folder",
                Image = new Image("image.jpg", "/local/path/image.jpg", "Some metadata")
            };

            _cloudStorageServiceMock.Setup(s => s.DownloadAsync(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new Exception("Download failed"));

            var useCase = new RotateCloudImageUseCase(_cloudStorageServiceMock.Object, _imageProcessingServiceMock.Object);

            // Act
            var response = await useCase.ExecuteAsync(request);

            // Assert
            Assert.False(response.Success);
            Assert.Equal("Download failed", response.ErrorMessage);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFailure_WhenRotationFails()
        {
            // Arrange
            var request = new RotateCloudImageRequest
            {
                CloudImagePath = "cloud/image/path.jpg",
                Angle = 90,
                TempFolderPath = "/temp/folder",
                Image = new Image("image.jpg", "/local/path/image.jpg", "Some metadata")
            };

            _cloudStorageServiceMock.Setup(s => s.DownloadAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("/local/path/image.jpg");
            _cloudStorageServiceMock.Setup(s => s.UploadAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            _imageProcessingServiceMock.Setup(s => s.RotateImageAsync(It.IsAny<Image>(), It.IsAny<int>())).ThrowsAsync(new Exception("Rotation failed"));

            var useCase = new RotateCloudImageUseCase(_cloudStorageServiceMock.Object, _imageProcessingServiceMock.Object);

            // Act
            var response = await useCase.ExecuteAsync(request);

            // Assert
            Assert.False(response.Success);
            Assert.Equal("Rotation failed", response.ErrorMessage);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFailure_WhenUploadFails()
        {
            // Arrange
            var request = new RotateCloudImageRequest
            {
                CloudImagePath = "cloud/image/path.jpg",
                Angle = 90,
                TempFolderPath = "/temp/folder",
                Image = new Image("image.jpg", "/local/path/image.jpg", "Some metadata")
            };

            _cloudStorageServiceMock.Setup(s => s.DownloadAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("/local/path/image.jpg");
            _imageProcessingServiceMock.Setup(s => s.RotateImageAsync(It.IsAny<Image>(), It.IsAny<int>())).ReturnsAsync(new Image("image_rotated.jpg", "/local/path/image_rotated.jpg", "Some metadata"));
            _cloudStorageServiceMock.Setup(s => s.UploadAsync(It.IsAny<string>())).ThrowsAsync(new Exception("Upload failed"));

            var useCase = new RotateCloudImageUseCase(_cloudStorageServiceMock.Object, _imageProcessingServiceMock.Object);

            // Act
            var response = await useCase.ExecuteAsync(request);

            // Assert
            Assert.False(response.Success);
            Assert.Equal("Upload failed", response.ErrorMessage);
        }

        private static string AddRotatedSuffix(string cloudImagePath)
        {
            var extension = Path.GetExtension(cloudImagePath);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(cloudImagePath);
            var directory = Path.GetDirectoryName(cloudImagePath);

            directory = directory?.Replace("\\", "/");

            return Path.Combine(directory, $"{fileNameWithoutExtension}_rotated{extension}");
        }
    }
}
