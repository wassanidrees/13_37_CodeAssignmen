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
        private Mock<IFileAndDirectoryServices> _fileAndDirectoryServicesMock;
        private FileSaverServices _fileSaverServices;

        public FileSaverServicesTest()
        {
            _logger = new();
            _fileAndDirectoryServicesMock = new();
            _fileSaverServices = new FileSaverServices(_logger.Object, _fileAndDirectoryServicesMock.Object);
                }
        [Fact(DisplayName = "Save files locally create")]
        public async Task Save_files_locally_creat()
        {
            //Arrange 
            _fileAndDirectoryServicesMock.Setup(f => f.CreateDirectory(It.IsAny<String>()));
            _fileAndDirectoryServicesMock.Setup(f => f.CreateFile(It.IsAny<string>(), It.IsAny<byte[]>())).Returns(Task.FromResult(Task.CompletedTask)); ;
            
            //Act
            await _fileSaverServices.SaveLocally(It.IsAny<byte[]>(), "/");
            
            //Assert
            _fileAndDirectoryServicesMock.Verify(f => f.CreateDirectory(It.IsAny<String>()),Times.Once);
            _fileAndDirectoryServicesMock.Verify(f => f.CreateFile(It.IsAny<string>(), It.IsAny<byte[]>()), Times.Once);

        }
    }
}
