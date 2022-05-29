using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_Crawler.Services.Interfaces
{
   public interface IFileAndDirectoryServices
    {
        void CreateDirectory(string path);
        Task CreateFile(string filePath, byte[] fileContent);
    }
}
