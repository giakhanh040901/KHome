using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Validation;
using System.Collections.Generic;

namespace EPIC.IdentityEntities.Dto.WhiteListIp
{
    public class CreateWhiteListDto
    {
        public string Name { get; set; }

        [IntegerRange(AllowableValues = new int[] { 
            WhiteListIpTypes.ThongTinKhachDauTuTps, WhiteListIpTypes.ThongTinKhachDauTuTelesale, 
            WhiteListIpTypes.InvestDuyetHopDong, WhiteListIpTypes.InvestDuyetChiTien, WhiteListIpTypes.InvestDuyetVaoTien, 
            WhiteListIpTypes.GarnerDuyetHopDong, WhiteListIpTypes.GarnerDuyetChiTien, WhiteListIpTypes.GarnerDuyetVaoTien, 
            WhiteListIpTypes.RstDuyetHopDong, WhiteListIpTypes.RstDuyetVaoTien,
        })]
        public int Type { get; set; }
        public int? TradingProviderId { get; set; }
        public List<CreateWhiteListIPDetailDto> WhiteListIPDetails { get; set; }
    }

    public class CreateWhiteListIPDetailDto
    {
        public string IpAddressStart { get; set; }
        public string IpAddressEnd { get; set; }
    }
}
