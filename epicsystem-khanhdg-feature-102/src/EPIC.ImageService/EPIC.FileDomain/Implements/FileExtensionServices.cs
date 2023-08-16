using EPIC.DataAccess.Base;
using EPIC.FileDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Media;
using EPIC.Utils.Net.File;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.FileDomain.Implements
{
    public class FileExtensionServices : IFileExtensionServices
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<FileExtensionServices> _logger;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly EpicSchemaDbContext _dbContext;
        public FileExtensionServices(EpicSchemaDbContext dbContext,
            IConfiguration configuration,
            ILogger<FileExtensionServices> logger)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _logger = logger;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string GetMediaExtensionFile(string file)
        {
            string ext = Path.GetExtension(file)?.ToLower();
            string mediaType = null;
            if (ext == FileTypes.JPG || ext == FileTypes.PNG || ext == FileTypes.JPEG || ext == FileTypes.TIFF || ext == FileTypes.PDF 
                || ext == FileTypes.AI || ext == FileTypes.SVG)
            {
                mediaType = MediaTypes.IMAGE;
            }
            else if (ext == FileTypes.MP4 || ext == FileTypes.MOV)
            {
                mediaType = MediaTypes.VIDEO;
            }
            else
            {
                _defErrorEFRepository.ThrowException(ErrorCode.FileExtensionNoAllow);
            }

            return mediaType;
        }
    }
}
