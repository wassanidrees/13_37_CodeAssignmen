﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Crawler.Services.Interfaces;

namespace Web_Crawler.Services
{
    public class DirectoryAndFileHandlerService : IDirectoryAndFileHandlerService
    {
        public async Task WriteFileToDisk(string filePath, byte[] fileContent)
        {
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            using (var fileStream = new FileStream(filePath, FileMode.CreateNew))
            {
                await fileStream.WriteAsync(fileContent, 0, fileContent.Length);
            }
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
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            try
            {
                await WriteFileToDisk(fullFilePath, ContentStream);
            }
            catch (IOException e)
            {
                return;
            }
        }   
    }
}