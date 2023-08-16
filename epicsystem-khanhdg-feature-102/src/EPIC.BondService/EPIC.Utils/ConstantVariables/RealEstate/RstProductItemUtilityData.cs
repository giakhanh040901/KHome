using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    public class RstProductItemUtilityData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GroupUtilityId { get; set; }
        public int Type { get; set; }
        public string Icon { get; set; }

        public static readonly List<RstProductItemUtilityData> UtilityData = new()
        {
                new RstProductItemUtilityData() {Id = 284, Name = "Bể bơi riêng", GroupUtilityId = 22, Type = RstProjectUtilityTypes.TUNG_CAN_HO, Icon = "Icon_Beboirieng"},
                new RstProductItemUtilityData() {Id = 285, Name = "Phòng tập gym riêng", GroupUtilityId = 22, Type = RstProjectUtilityTypes.TUNG_CAN_HO, Icon = "Icon_Phongtapgymrieng"},
                new RstProductItemUtilityData() {Id = 286, Name = "View rộng, thoáng", GroupUtilityId = 22, Type = RstProjectUtilityTypes.TUNG_CAN_HO, Icon = "Icon_Viewrong"},
                new RstProductItemUtilityData() {Id = 287, Name = "Cầu thang riêng trong nhà", GroupUtilityId = 22, Type = RstProjectUtilityTypes.TUNG_CAN_HO, Icon = "Icon_Cauthangriengtrongnha"},
                new RstProductItemUtilityData() {Id = 288, Name = "Tiểu cảnh riêng", GroupUtilityId = 22, Type = RstProjectUtilityTypes.TUNG_CAN_HO, Icon = "Icon_Tieucanhrieng"},
                new RstProductItemUtilityData() {Id = 289, Name = "Không gian sang trọng", GroupUtilityId = 22, Type = RstProjectUtilityTypes.TUNG_CAN_HO, Icon = "Icon_Khonggiansangtrong"},
                new RstProductItemUtilityData() {Id = 290, Name = "Thang máy riêng", GroupUtilityId = 22, Type = RstProjectUtilityTypes.TUNG_CAN_HO, Icon = "Icon_Thangmayrieng"},
                new RstProductItemUtilityData() {Id = 291, Name = "Quầy bar trong nhà", GroupUtilityId = 22, Type = RstProjectUtilityTypes.TUNG_CAN_HO, Icon = "Icon_Quaybarrieng"},
                new RstProductItemUtilityData() {Id = 292, Name = "Vị trí đẹp của tòa nhà", GroupUtilityId = 22, Type = RstProjectUtilityTypes.TUNG_CAN_HO, Icon = "Icon_Vitridep"},
                new RstProductItemUtilityData() {Id = 293, Name = "Diện tích rộng", GroupUtilityId = 22, Type = RstProjectUtilityTypes.TUNG_CAN_HO, Icon = "Icon_Dientichrong"},
                new RstProductItemUtilityData() {Id = 294, Name = "Yên tĩnh", GroupUtilityId = 22, Type = RstProjectUtilityTypes.TUNG_CAN_HO, Icon = "Icon_Yentinh"},
        };
    }
}
