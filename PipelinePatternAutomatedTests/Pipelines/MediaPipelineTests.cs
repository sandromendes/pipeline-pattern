using Moq;
using PipelinePatternSample.Pipelines.Builders;
using PipelinePatternSample.Pipelines;
using PipelinePatternSample.Services.Interfaces;
using PipelinePatternSample.UseCases.Contexts;
using PipelinePatternSample;
using PipelinePatternSample.Domain;
using PipelinePatternAutomatedTests.Extensions;

namespace PipelinePatternAutomatedTests.Pipelines
{
    public class MediaPipelineTests
    {
        [Fact]
        public async Task MediaPipeline_ShouldExecuteAllStepsInSequenceSuccessfully()
        {
            // Arrange
            var cloudStorageServiceMock = new Mock<ICloudStorageService>();
            var originalImage = new Image("test_image.jpg", "path/to/image", "Original metadata");

            // Mock para o serviço de download
            cloudStorageServiceMock
                .Setup(service => service.DownloadAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("temp/path/test_image.jpg");

            // Mock para o serviço de upload
            cloudStorageServiceMock
                .Setup(service => service.UploadAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Criação do pipeline
            var pipeline = MediaPipelineBuilder<ImageProcessingContext>
                .Create()
                .AddStep(new DownloadMediaStep(cloudStorageServiceMock.Object))
                .AddStep(new ApplyFilterStep())
                .AddStep(new AddMetadataStep())
                .AddStep(new SaveImageStep())
                .AddStep(new UploadMediaStep(cloudStorageServiceMock.Object));

            var context = new ImageProcessingContext
            {
                CloudImagePath = "cloud/path/image.jpg",
                TempFolderPath = "temp/folder/image.jpg",
                LocalImagePath = "local/path/image.jpg",
                Image = originalImage,
                Metadata = "New metadata"
            };

            // Act
            await pipeline.ProcessAsync(context);

            // Assert
            Assert.Contains(context.LocalImagePath, "local/path/image_edited.jpg"); // Após o download
            Assert.Contains("_edited", context.CloudImagePath); // Verifica aplicação do filtro
            Assert.Equal("New metadata", context.Image.Metadata); // Metadados atualizados
            cloudStorageServiceMock.Verify(service => service.DownloadAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            cloudStorageServiceMock.Verify(service => service.UploadAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task PipelineSteps_ShouldExecuteInCorrectSequence()
        {
            // Arrange
            var mockCloudStorageService = new Mock<ICloudStorageService>();
            var mockImageProcessingService = new Mock<IImageProcessingService>();

            // Mockar o comportamento dos serviços
            mockCloudStorageService
                .Setup(service => service.DownloadAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string fileName, string tempFolder) => $"{tempFolder}/{fileName}");

            mockImageProcessingService
                .Setup(service => service.RotateImageAsync(It.IsAny<Image>(), It.IsAny<int>()))
                .ReturnsAsync((Image image, int angle) =>
                {
                    image.Path = image.Path.AddSuffix("rotated");
                    return image;
                });

            mockCloudStorageService
                .Setup(service => service.UploadAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var pipeline = MediaPipelineBuilder<ImageProcessingContext>
                .Create()
                .AddStep(new DownloadMediaStep(mockCloudStorageService.Object))
                .AddStep(new RotateImageStep(mockImageProcessingService.Object))
                .AddStep(new UploadMediaStep(mockCloudStorageService.Object));

            var context = new ImageProcessingContext
            {
                CloudImagePath = "cloud/image/test.jpg",
                TempFolderPath = "temp",
                RotationAngle = 90,
                Image = new Image("test.jpg", null, string.Empty)
            };

            // Act
            await pipeline.ProcessAsync(context);

            // Assert
            Assert.NotNull(context.Image.Path);
            Assert.EndsWith("_rotated.jpg", context.CloudImagePath);
            Assert.Contains("temp/test_rotated.jpg", context.Image.Path);
            mockCloudStorageService.Verify(service => service.DownloadAsync("test.jpg", "temp"), Times.Once);
            mockImageProcessingService.Verify(service => service.RotateImageAsync(It.IsAny<Image>(), 90), Times.Once);
            mockCloudStorageService.Verify(service => service.UploadAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
