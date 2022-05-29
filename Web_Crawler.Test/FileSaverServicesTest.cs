using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Web_Crawler.Services;
using Web_Crawler.Services.Interfaces;
using Xunit;

namespace Web_Crawler.Test
{
    public class FileSaverServicesTest
    {
        private Mock<ILogger<FileSaverServices>> _logger;
        private Mock<IFileAndDirectoryServices> _fileAndDirectoryServices;
        FileSaverServices _directoryAndFileHandlerService;

        public FileSaverServicesTest()
        {
            _logger = new();
            _fileAndDirectoryServices = new();
            _directoryAndFileHandlerService = new FileSaverServices(_logger.Object, _fileAndDirectoryServices.Object);
                }
        [Fact(DisplayName = "Save files locally create")]
        public async Task Save_files_locally_creat()
        {
            //Arrange 
            _fileAndDirectoryServices.Setup(f => f.CreateDirectory(It.IsAny<String>()));
            _fileAndDirectoryServices.Setup(f => f.CreateFile(It.IsAny<string>(), It.IsAny<byte[]>())).Returns(Task.FromResult(Task.CompletedTask)); ;
            
            //Act
            await _directoryAndFileHandlerService.SaveLocally(It.IsAny<byte[]>(), "/");
            
            //Assert
            _fileAndDirectoryServices.Verify(f => f.CreateDirectory(It.IsAny<String>()),Times.Once);
            _fileAndDirectoryServices.Verify(f => f.CreateFile(It.IsAny<string>(), It.IsAny<byte[]>()), Times.Once);

        }
    }
}
