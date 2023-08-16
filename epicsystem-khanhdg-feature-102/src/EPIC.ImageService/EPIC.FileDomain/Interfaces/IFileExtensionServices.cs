using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.FileDomain.Interfaces
{
    public interface IFileExtensionServices
    {
        /// <summary>
        /// Phân loại media theo đuôi file: Image/Video
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        string GetMediaExtensionFile(string file);
    }
}
