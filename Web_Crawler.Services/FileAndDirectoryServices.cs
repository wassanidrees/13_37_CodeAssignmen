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
    public class FileAndDirectoryServices : IFileAndDirectoryServices
    {
        ILogger<FileAndDirectoryServices> _logger;
        public FileAndDirectoryServices(ILogger<FileAndDirectoryServices> logger)
        {
            _logger = logger;
        }
        public void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public async Task CreateFile(string filePath, byte[] fileContent)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            using (var fileStream = new FileStream(filePath, FileMode.CreateNew))
            {
                _logger.LogInformation("Saving file path: {0}", filePath);
                await fileStream.WriteAsync(fileContent, 0, fileContent.Length);
                _logger.LogInformation("Saving file finished");
            }
        }
    }
}
