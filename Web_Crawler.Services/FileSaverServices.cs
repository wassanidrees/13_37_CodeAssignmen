using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Crawler.Services.Interfaces;

namespace Web_Crawler.Services
{

    public class FileSaverServices : IFileSaverServices
    {
        private ILogger _logger;
        IFileAndDirectoryServices _fileAndDirectoryServices;
        public FileSaverServices(ILogger<FileSaverServices> logger, IFileAndDirectoryServices fileAndDirectoryServices)
        {
            _logger = logger;
            _fileAndDirectoryServices = fileAndDirectoryServices;
        }
        public async Task SaveLocally(byte[] ContentStream, string filePath)
        {
            if (Path.DirectorySeparatorChar != '/')
            {
                filePath = filePath.Replace('/', Path.DirectorySeparatorChar);
            }
            if (filePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                filePath = Path.Combine(filePath, "index.html").TrimStart(Path.DirectorySeparatorChar);
            }
            var directoryPath = Path.GetDirectoryName(filePath);
            var filePathOnDrive = Path.GetFileName(filePath);
            if (!Path.HasExtension(filePathOnDrive) && !string.IsNullOrEmpty(filePathOnDrive))
            {
                filePathOnDrive = Path.ChangeExtension(filePathOnDrive, "html");
            }
            var fullFilePath = Path.Combine(Constants.SavingDirectory, directoryPath, filePathOnDrive);
            var directory = Path.GetDirectoryName(fullFilePath);
            _fileAndDirectoryServices.CreateDirectory(directory);
            try
            {
                await _fileAndDirectoryServices.CreateFile(fullFilePath, ContentStream);
            }
            catch (IOException e)
            {
                _logger.LogError("Saving failed, message: {1}", fullFilePath, e.Message);
                return;
            }
        }
    }
}
