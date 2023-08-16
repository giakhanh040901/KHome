using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectInformationShare
{
    public class CreateRstProjectInfoShareDetailDto
    {
        private string _name;
        /// <summary>
        /// Tên đường dẫn file
        /// </summary>
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        private string _fileUrl;
        /// <summary>
        /// Đường dẫn file
        /// </summary>
        [ColumnSnackCase(nameof(FileUrl))]
        public string FileUrl
        {
            get => _fileUrl;
            set => _fileUrl = value?.Trim();
        }
    }
}
