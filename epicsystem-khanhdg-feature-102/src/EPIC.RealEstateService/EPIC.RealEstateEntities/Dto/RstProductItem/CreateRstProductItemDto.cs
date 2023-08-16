using EPIC.RealEstateEntities.Dto.RstProductItemExtend;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItem
{
    public class CreateRstProductItemDto
    {
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Loại trạng thái sổ đỏ không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] { RstRedBookTypes.HasRedBook, RstRedBookTypes.NoRedBook, RstRedBookTypes.HasRedBook50Year, RstRedBookTypes.HasRedBookLongTerm }, 
            ErrorMessage = "Vui lòng chọn 1 trong các loại trạng thái sổ đỏ sau")]
        public int RedBookType { get; set; }

        private string _code;
        [Required(ErrorMessage = "Mã căn/mã sản phẩm không được để trống")]
        [StringLength(256, ErrorMessage = "Mã căn/mã sản phẩm không được dài hơn 256 ký tự")]
        public string Code 
        { 
            get => _code; 
            set => _code = value?.Trim(); 
        }

        private string _name;
        [Required(ErrorMessage = "Số căn/tên không được để trống")]
        [StringLength(256, ErrorMessage = "Số căn/tên không được dài hơn 256 ký tự")]
        public string Name 
        {
            get => _name;
            set => _name = value?.Trim();
        }

        private string _numberFloor;
        /// <summary>
        /// Tầng số bao nhiêu
        /// </summary>
        public string NumberFloor 
        { 
            get => _numberFloor; 
            set => _numberFloor = value?.Trim(); 
        }

        [IntegerRange(AllowableValues = new int[] { RstRoomTypes.OneBedroom, RstRoomTypes.TwoBedroom, RstRoomTypes.ThreeBedroom, RstRoomTypes.FourBedroom, 
            RstRoomTypes.FiveBedroom, RstRoomTypes.SixBedroom, RstRoomTypes.SevenBedroom, RstRoomTypes.EightBedroom, RstRoomTypes.OneBedroomPlus1, 
            RstRoomTypes.TwoBedroomPlus1, RstRoomTypes.ThreeBedroomPlus1, RstRoomTypes.FourBedroomPlus1 }, ErrorMessage = "Vui lòng chọn 1 trong các kiểu phòng ngủ - số phòng sau")]
        public int? RoomType { get; set; }

        [IntegerRange(AllowableValues = new int[] { RstDirections.Dong, RstDirections.Tay, RstDirections.Nam, RstDirections.Bac, RstDirections.DongBac, 
            RstDirections.DongNam, RstDirections.TayBac, RstDirections.TayNam, RstDirections.DongNamTayNam, RstDirections.DongNamDongBac, RstDirections.TayNamTayBac, 
            RstDirections.DongNamTayBac, RstDirections.DongBacTayBac, RstDirections.DongBacTayNam }, ErrorMessage = "Vui lòng chọn 1 trong các hướng cửa sau")]
        public int? DoorDirection { get; set; }
        /// <summary>
        /// Hướng ban công
        /// </summary>
        public int BalconyDirection { get; set; }
        /// <summary>
        /// Vị trí
        /// </summary>
        [IntegerRange(AllowableValues = new int[] { RstProductLocations.CanGiua, RstProductLocations.CanGoc, RstProductLocations.CongChinh, RstProductLocations.ToaRieng, RstProductLocations.CanThongTang },
            ErrorMessage = "Vui lòng chọn 1 trong các vị trí căn/sản phẩm sau")]
        public int? ProductLocation { get; set; }
        /// <summary>
        /// Loại hình
        /// </summary>
        [IntegerRange(AllowableValues = new int[] { RstProductTypes.CanDon, RstProductTypes.CanGhep }, ErrorMessage = "Vui lòng chọn 1 trong các loại hình căn/sản phẩm sau")]
        public int? ProductType { get; set; }

        [IntegerRange(AllowableValues = new int[] { RstHandingTypes.NoiThatLienTuong, RstHandingTypes.NoiThatCoBan, RstHandingTypes.NoiThatCaoCap, RstHandingTypes.BanGiaoTho, RstHandingTypes.FullNoiThat },
            ErrorMessage = "Vui lòng chọn 1 trong các loại bàn giao sau")]
        public int? HandingType { get; set; }

        private string _viewDescription;
        public string ViewDescription 
        {
            get => _viewDescription;
            set => _viewDescription = value?.Trim();
        }

        public decimal CarpetArea { get; set; }
        public decimal BuiltUpArea { get; set; }
        public decimal LandArea { get; set; }
        public decimal ConstructionArea { get; set; }
        public decimal? Price { get; set; }

        private string _compoundRoom;
        public string CompoundRoom 
        {
            get => _compoundRoom;
            set => _compoundRoom = value?.Trim();
        }

        private string _compoundFloor;
        public string CompoundFloor 
        {
            get => _compoundFloor;
            set => _compoundFloor = value?.Trim();
        }

        public int BuildingDensityId { get; set; }

        [Required(ErrorMessage = "Phân loại sản phẩm không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] { RstClassifyType.CanHoThongThuong, RstClassifyType.CanHoStudio, RstClassifyType.CanHoOfficetel, RstClassifyType.CanHoShophouse, RstClassifyType.CanHoPenthouse, 
            RstClassifyType.CanHoDuplex, RstClassifyType.CanHoSkyVilla, RstClassifyType.NhaONongThon, RstClassifyType.BietThuNhaO, RstClassifyType.LienKe, RstClassifyType.ChungCuThapTang, RstClassifyType.CanShophouse, 
            RstClassifyType.BietThuNghiDuong, RstClassifyType.Villa, RstClassifyType.DuplexPool },
            ErrorMessage = "Vui lòng chọn 1 trong các phân loại sản phẩm sau")]
        public int ClassifyType { get; set; }
        public decimal PriceArea { get; set; }
        public decimal UnitPrice { get; set; }
        private string _noFloor;
        public string NoFloor
        {
            get => _noFloor;
            set => _noFloor = value?.Trim();
        }
        public DateTime? HandoverTime { get; set; }
        public decimal? FloorBuildingArea { get; set; }

        /// <summary>
        /// Các thông tin khác của sản phẩm
        /// </summary>
        public List<CreateRstProductItemExtendDto> ProductItemExtends { get; set; }
    }
}
