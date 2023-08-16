using EPIC.RealEstateEntities.Dto.RstProductItemMaterialFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstProductItemMaterialFileService
    {
        /// <summary>
        /// Thêm file
        /// </summary>
        /// <param name="input"></param>
        void AddMaterialFile(CreateRstProductItemMaterialFileDto input);
        /// <summary>
        /// Cập nhật file
        /// </summary>
        /// <param name="input"></param>
        void UpdateMaterialFile(UpdateRstProductItemMaterialFileDto input);
        /// <summary>
        /// Danh sách file
        /// </summary>
        /// <returns></returns>
        IEnumerable<RstProductItemMaterialFileDto> FindAll(int productItemId);
        /// <summary>
        /// Xóa file
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);
    }
}
