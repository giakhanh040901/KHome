using EPIC.Entities.Dto.CoreApprove;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.Entities.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.ManagerInvestor
{
    /// <summary>
    /// Lịch sử cập nhật investor
    /// </summary>
    public class ViewInvestorUpdateHistoryDto
    {
        public Dictionary<string, HistoryDto> Investor { get; set; }
        public List<ObjectIdentificationDto> ListIdentification { get; set; }
        public List<ObjectBankDto> ListBank { get; set; }
        public List<ObjectContactAddressDto> ListContactAddress { get; set; }
        public List<ObjectStockDto> ListStock { get; set; }
        public UserDto UserRequest { get; set; }
        public UserDto UserApprove { get; set; }
        public ViewCoreApproveDto approveDto { get; set; }
    }

    public class BaseObjectHistoryDto
    {
        public int Id { get; set; }
        public Dictionary<string, HistoryDto> History { get; set; }
    }

    public class HistoryDto
    {
        public object OldValue { get; set; }
        public object NewValue { get; set; }
    }

    /// <summary>
    /// Lịch sử giấy tờ
    /// </summary>
    public class ObjectIdentificationDto : BaseObjectHistoryDto
    {
        public ViewIdentificationDto NewObject { get; set; }
    }

    /// <summary>
    /// Lịch sử lk ngân hàng
    /// </summary>
    public class ObjectBankDto : BaseObjectHistoryDto
    {
        public ViewInvestorBankAccountDto NewObject { get; set; }
        public ViewInvestorBankAccountDto DeletedObject { get; set; }
    }

    /// <summary>
    /// Lịch sử địa chỉ giao dịch
    /// </summary>
    public class ObjectContactAddressDto : BaseObjectHistoryDto
    {
        public ViewInvestorContactAddressDto NewObject { get; set; }
    }

    /// <summary>
    /// Lịch sử thông tin chứng khoán
    /// </summary>
    public class ObjectStockDto : BaseObjectHistoryDto
    {
        public ViewInvestorStockDto NewObject { get; set; }
    }
}
