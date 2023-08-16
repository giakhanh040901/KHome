using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    public class RstProjectUtilityData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GroupUtilityId { get; set; }
        public int Type { get; set; }
        public string Icon { get; set; }

        public static readonly List<RstProjectUtilityData> UtilityData = new()
        {
                new RstProjectUtilityData() {Id = 1, Name = "Công viên cây xanh", GroupUtilityId = 1, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Congviencayxanh"},

                new RstProjectUtilityData() {Id = 2, Name = "Hầm để xe dưới mặt đất", GroupUtilityId = 2, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Hamdexeduoimatdat"},
                new RstProjectUtilityData() {Id = 3, Name = "Hầm để ô tô", GroupUtilityId = 2, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Hamdeoto"},
                new RstProjectUtilityData() {Id = 4, Name = "Hầm để xe máy", GroupUtilityId = 2, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Hamdexemay"},
                new RstProjectUtilityData() {Id = 5, Name = "Trạm sạc xe điện trong hầm", GroupUtilityId = 2, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Tramsacdientrongham"},
                new RstProjectUtilityData() {Id = 6, Name = "Trạm sạc xe điện ngoài trời", GroupUtilityId = 2, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Tramsacdienngoaitroi"},

                new RstProjectUtilityData() {Id = 7, Name = "Bể bơi bốn mùa", GroupUtilityId = 3, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Beboibonmua"},
                new RstProjectUtilityData() {Id = 8, Name = "Bể bơi trong nhà", GroupUtilityId = 3, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Beboitrongnha"},
                new RstProjectUtilityData() {Id = 9, Name = "Bể bơi trên cao", GroupUtilityId = 3, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Beboitrencao"},
                new RstProjectUtilityData() {Id = 10, Name = "Bể bơi ngoài trời", GroupUtilityId = 3, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Beboingoaitroi"},

                new RstProjectUtilityData() {Id = 11, Name = "Cafe sân thượng", GroupUtilityId = 4, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Cafesanthuong"},
                new RstProjectUtilityData() {Id = 12, Name = "Cafe mặt đất", GroupUtilityId = 4, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Cafematdat"},
                //new RstProjectUtilityData() {Id = 13, Name = "Cafe sảnh", GroupUtilityId = 4, Type = RstProjectUtilityTypes.NOI_KHU},
                new RstProjectUtilityData() {Id = 14, Name = "Cafe trong nhà", GroupUtilityId = 4, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Cafetrongnha"},

                new RstProjectUtilityData() {Id = 15, Name = "Hồ điều hòa", GroupUtilityId = 5, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Hodieuhoa"},
                new RstProjectUtilityData() {Id = 16, Name = "Biển hồ nước ngọt", GroupUtilityId = 5, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Bienhonuocngot"},
                new RstProjectUtilityData() {Id = 17, Name = "Biển hồ nước mặn", GroupUtilityId = 5, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Bienhonuocman"},

                new RstProjectUtilityData() {Id = 18, Name = "Nhà bóng", GroupUtilityId = 6, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Nhabong"},
                new RstProjectUtilityData() {Id = 19, Name = "Cầu trượt", GroupUtilityId = 6, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Cautruot"},
                new RstProjectUtilityData() {Id = 20, Name = "Xích đu", GroupUtilityId = 6, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Xichdu"},

                new RstProjectUtilityData() {Id = 21, Name = "Trung tâm tiếng anh", GroupUtilityId = 7, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Trungtamngoaingu"},
                new RstProjectUtilityData() {Id = 22, Name = "Trung tâm ôn luyện thi", GroupUtilityId = 7, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Trungtamonluyenthi"},
                new RstProjectUtilityData() {Id = 23, Name = "Trung tâm dạy kỹ năng mềm", GroupUtilityId = 7, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Trungtamdaykynangmem"},
                //new RstProjectUtilityData() {Id = 24, Name = "Trung tâm dã ngoại", GroupUtilityId = 7, Type = RstProjectUtilityTypes.NOI_KHU},

                new RstProjectUtilityData() {Id = 25, Name = "Sân trượt patin", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Santruotpatin"},
                new RstProjectUtilityData() {Id = 26, Name = "Nhà ma", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Nhama"},
                new RstProjectUtilityData() {Id = 27, Name = "Karaoke", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Karaoke"},
                new RstProjectUtilityData() {Id = 28, Name = "Game cảm giác mạnh", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Gamecamgiacmanh"},
                new RstProjectUtilityData() {Id = 29, Name = "Rạp chiếu phim", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Rapchieuphim"},
                new RstProjectUtilityData() {Id = 30, Name = "Khu vui chơi ven biển", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Khuvuichoivenbien"},
                //new RstProjectUtilityData() {Id = 31, Name = "Khu thể thao bãi biển", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NOI_KHU},
                new RstProjectUtilityData() {Id = 32, Name = "Phố đi bộ", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Phodibo"},
                new RstProjectUtilityData() {Id = 33, Name = "Bowling", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Bowling"},
                new RstProjectUtilityData() {Id = 34, Name = "Billiard", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Billiard"},

                new RstProjectUtilityData() {Id = 35, Name = "Di sản văn hóa", GroupUtilityId = 9, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Disanvanhoa"},
                new RstProjectUtilityData() {Id = 36, Name = "Di tích lịch sử", GroupUtilityId = 9, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Ditichlichsu"},
                new RstProjectUtilityData() {Id = 37, Name = "Thủy cung", GroupUtilityId = 9, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Thuycung"},
                new RstProjectUtilityData() {Id = 38, Name = "Đài phun nước", GroupUtilityId = 9, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Daiphunnuoc"},
                new RstProjectUtilityData() {Id = 39, Name = "Tượng đài", GroupUtilityId = 9, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Tuongdai"},
                new RstProjectUtilityData() {Id = 40, Name = "Quảng trường", GroupUtilityId = 9, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Quangtruong"},
                new RstProjectUtilityData() {Id = 41, Name = "Nhạc nước", GroupUtilityId = 9, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Nhacnuoc"},
                new RstProjectUtilityData() {Id = 42, Name = "Triển lãm nghệ thuật", GroupUtilityId = 9, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Trienlamnghethuat"},
                //new RstProjectUtilityData() {Id = 43, Name = "Khu trưng bày", GroupUtilityId = 9, Type = RstProjectUtilityTypes.NOI_KHU},

                new RstProjectUtilityData() {Id = 44, Name = "Sân bóng đá", GroupUtilityId = 10, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Sanbongda"},
                new RstProjectUtilityData() {Id = 45, Name = "Sân bóng rổ", GroupUtilityId = 10, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Sanbongro"},
                new RstProjectUtilityData() {Id = 46, Name = "Sân tenis", GroupUtilityId = 10, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Santenis"},
                new RstProjectUtilityData() {Id = 47, Name = "Sân golf mini", GroupUtilityId = 10, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Sangolfmini"},
                new RstProjectUtilityData() {Id = 48, Name = "Sân tập võ", GroupUtilityId = 10, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Santapvo"},
                new RstProjectUtilityData() {Id = 49, Name = "Sân nhảy ngoài trời", GroupUtilityId = 10, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Sannhayngoaitroi"},
                new RstProjectUtilityData() {Id = 50, Name = "Phòng tập Yoga", GroupUtilityId = 10, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Phongtapyoga"},
                new RstProjectUtilityData() {Id = 51, Name = "Sân cầu lông", GroupUtilityId = 10, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Sancaulong"},
                new RstProjectUtilityData() {Id = 52, Name = "Khu chơi bóng bàn", GroupUtilityId = 10, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Khuchoibongban"},
                new RstProjectUtilityData() {Id = 53, Name = "Phòng Gym", GroupUtilityId = 10, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Phonggym"},

                new RstProjectUtilityData() {Id = 54, Name = "Cửa hàng thời trang", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Cuahangthoitrang"},
                new RstProjectUtilityData() {Id = 55, Name = "Cửa hàng bán lẻ", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Cuahangbanle"},
                new RstProjectUtilityData() {Id = 56, Name = "Cửa hàng đồ gia dụng", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Cuahangdogiadung"},
                new RstProjectUtilityData() {Id = 57, Name = "Cửa hàng nội thất", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Cuahangnoithat"},
                new RstProjectUtilityData() {Id = 58, Name = "Cửa hàng giặt là", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Cuahanggiatla"},
                new RstProjectUtilityData() {Id = 59, Name = "Cửa hàng cắt tóc", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Cuahangcattoc"},
                new RstProjectUtilityData() {Id = 60, Name = "Cửa hàng đồ chơi trẻ em", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Cuahangdochoitreem"},
                new RstProjectUtilityData() {Id = 61, Name = "Cửa hàng spa", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Cuahangspa"},
                new RstProjectUtilityData() {Id = 62, Name = "Cửa hàng nail", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Cuahangnail"},
                new RstProjectUtilityData() {Id = 63, Name = "Cửa hàng đồ thể thao", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Cuahangdothethao"},
                new RstProjectUtilityData() {Id = 64, Name = "Cửa hàng đồ công sở", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Cuahangdocongso"},
                new RstProjectUtilityData() {Id = 65, Name = "Cửa hàng tiện lợi", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Cuahangtienloi"},

                new RstProjectUtilityData() {Id = 66, Name = "BBQ ngoài trời", GroupUtilityId = 12, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_BBQngoaitroi"},
                new RstProjectUtilityData() {Id = 67, Name = "Phố ẩm thực", GroupUtilityId = 12, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Phoamthuc"},
                //new RstProjectUtilityData() {Id = 68, Name = "BBQ sân thượng", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NOI_KHU},
                //new RstProjectUtilityData() {Id = 69, Name = "Bar sân thượng", GroupUtilityId = 12, Type = RstProjectUtilityTypes.NOI_KHU},
                new RstProjectUtilityData() {Id = 70, Name = "Đồ ăn nhanh", GroupUtilityId = 12, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Doannhanh"},
                new RstProjectUtilityData() {Id = 71, Name = "Ẩm thực nước ngoài", GroupUtilityId = 12, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Amthucnuocngoai"},
                //new RstProjectUtilityData() {Id = 72, Name = "Pub sân thượng", GroupUtilityId = 12, Type = RstProjectUtilityTypes.NOI_KHU},
                //new RstProjectUtilityData() {Id = 73, Name = "Đồ ăn nhanh", GroupUtilityId = 12, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Doannhanh"},
                new RstProjectUtilityData() {Id = 74, Name = "Lẩu", GroupUtilityId = 12, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Lau"},
                new RstProjectUtilityData() {Id = 75, Name = "Đồ ăn vỉa hè", GroupUtilityId = 12, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Doanviahe"},
                new RstProjectUtilityData() {Id = 76, Name = "Đồ uống", GroupUtilityId = 12, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Douong"},

                new RstProjectUtilityData() {Id = 77, Name = "Cầu thang bộ", GroupUtilityId = 13, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Cauthangbo"},
                //new RstProjectUtilityData() {Id = 78, Name = "Thang thoát hiểm", GroupUtilityId = 13, Type = RstProjectUtilityTypes.NOI_KHU},
                new RstProjectUtilityData() {Id = 79, Name = "Thang máy cho cư dân", GroupUtilityId = 13, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Thangmaychocudan"},
                new RstProjectUtilityData() {Id = 80, Name = "Thang máy cho khu văn phòng", GroupUtilityId = 13, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Thangmaychokhuvanphong"},
                new RstProjectUtilityData() {Id = 81, Name = "Cầu thang cuốn", GroupUtilityId = 13, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Cauthangcuon"},
                new RstProjectUtilityData() {Id = 82, Name = "Thang cuốn", GroupUtilityId = 13, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Thangcuon"},

                new RstProjectUtilityData() {Id = 83, Name = "Hệ thống báo cháy", GroupUtilityId = 14, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Hethongbaochay"},
                new RstProjectUtilityData() {Id = 84, Name = "Hệ thống chữa cháy", GroupUtilityId = 14, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Hethongchuachay"},
                new RstProjectUtilityData() {Id = 85, Name = "Đèn thoát hiểm", GroupUtilityId = 14, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Denthoathiem"},
                new RstProjectUtilityData() {Id = 86, Name = "Hệ thống camera trong tòa nhà", GroupUtilityId = 14, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Hethongcameratrongtoanha"},
                new RstProjectUtilityData() {Id = 87, Name = "Máy phát điện", GroupUtilityId = 14, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Mayphatdien"},
                new RstProjectUtilityData() {Id = 88, Name = "Hệ thống camera ngoài trời", GroupUtilityId = 14, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Hethongcamerangoaitroi"},
                //new RstProjectUtilityData() {Id = 89, Name = "Hệ thống cảnh báo mất an toàn", GroupUtilityId = 14, Type = RstProjectUtilityTypes.NOI_KHU},
                new RstProjectUtilityData() {Id = 90, Name = "Hệ thống thoát nước", GroupUtilityId = 14, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Hethongthoatnuoc"},
                new RstProjectUtilityData() {Id = 91, Name = "Hệ thống dây diện ngầm", GroupUtilityId = 14, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Hethongdaydienngam"},
                new RstProjectUtilityData() {Id = 92, Name = "Hệ thống điều hòa căn hộ", GroupUtilityId = 14, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Hethongdieuhoacanho"},
                new RstProjectUtilityData() {Id = 93, Name = "Hệ thống điều hòa toàn nhà", GroupUtilityId = 14, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Hethongdieuhoatoanha"},
                new RstProjectUtilityData() {Id = 94, Name = "Cáp quang", GroupUtilityId = 14, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Capquang"},
                //new RstProjectUtilityData() {Id = 95, Name = "Còi báo động", GroupUtilityId = 14, Type = RstProjectUtilityTypes.NOI_KHU},

                new RstProjectUtilityData() {Id = 96, Name = "Khu vệ sinh công cộng", GroupUtilityId = 15, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Khuvesinhcongcong"},
                new RstProjectUtilityData() {Id = 97, Name = "Đèn chiếu sáng ngoài trời", GroupUtilityId = 15, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Denchieusangngoaitroi"},
                //new RstProjectUtilityData() {Id = 98, Name = "Sảnh chờ", GroupUtilityId = 15, Type = RstProjectUtilityTypes.NOI_KHU},
                new RstProjectUtilityData() {Id = 99, Name = "Thẻ ra vào tòa nhà", GroupUtilityId = 15, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Theravaotoanha"},
                new RstProjectUtilityData() {Id = 100, Name = "Ra vào bằng nhận diện khuôn mặt", GroupUtilityId = 15, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Nhandienkhuonmat"},

                new RstProjectUtilityData() {Id = 101, Name = "Bệnh viện", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Benhvien"},
                new RstProjectUtilityData() {Id = 102, Name = "Trường mầm non", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Truongmannon"},
                new RstProjectUtilityData() {Id = 103, Name = "Trường tiểu học", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Truongtieuhoc"},
                new RstProjectUtilityData() {Id = 104, Name = "Trường THCS", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Truongthcs"},
                //new RstProjectUtilityData() {Id = 105, Name = "Trường liên cấp", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NOI_KHU},
                new RstProjectUtilityData() {Id = 106, Name = "Nhà chờ xe bus", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Nhachoxebus"},
                new RstProjectUtilityData() {Id = 107, Name = "Nhà thờ", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Nhatho"},
                //new RstProjectUtilityData() {Id = 108, Name = "Chùa ", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Chua"},
                //new RstProjectUtilityData() {Id = 109, Name = "Cơ quan nhà nước", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Coquannhanuoc"},
                new RstProjectUtilityData() {Id = 110, Name = "Trung tâm thương mại", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Trungtamthuongmai"},
                //new RstProjectUtilityData() {Id = 111, Name = "Trường THPT", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NOI_KHU},
                //new RstProjectUtilityData() {Id = 112, Name = "Trường đại học", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NOI_KHU},
                //new RstProjectUtilityData() {Id = 113, Name = "Cổng ra vào khu nhà ở", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NOI_KHU},
                new RstProjectUtilityData() {Id = 114, Name = "Sân vườn", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Sanvuon"},
                //new RstProjectUtilityData() {Id = 115, Name = "Cây xanh", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NOI_KHU},
                new RstProjectUtilityData() {Id = 116, Name = "Vườn hoa", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Vuonhoa"},
                new RstProjectUtilityData() {Id = 117, Name = "Cây xanh vỉa hè", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Cayxanhviahe"},
                //new RstProjectUtilityData() {Id = 118, Name = "Phòng sinh hoạt cộng động", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NOI_KHU},
                //new RstProjectUtilityData() {Id = 119, Name = "Sảnh đỗ xe trả khách", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NOI_KHU},
                //new RstProjectUtilityData() {Id = 120, Name = "Căn hộ cho thuê để ở", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Canhochothuedeo"},
                //new RstProjectUtilityData() {Id = 121, Name = "Căn hộ cho thuê làm văn phòng", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Canhochothuelamvanphong"},

                new RstProjectUtilityData() {Id = 122, Name = "Căn hộ cho thuê để ở", GroupUtilityId = 17, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Canhochothuedeo"},
                new RstProjectUtilityData() {Id = 123, Name = "Căn hộ cho thuê làm văn phòng", GroupUtilityId = 17, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Canhochothuelamvanphong"},
                //new RstProjectUtilityData() {Id = 124, Name = "Căn hộ cho thuê dịch vụ", GroupUtilityId = 17, Type = RstProjectUtilityTypes.NOI_KHU},

                new RstProjectUtilityData() {Id = 125, Name = "Xe bus", GroupUtilityId = 18, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Xebus"},
                new RstProjectUtilityData() {Id = 126, Name = "Xe điện di chuyển trong khu", GroupUtilityId = 18, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Xediendichuyentrongkhu"},
                new RstProjectUtilityData() {Id = 127, Name = "Xe đạp công cộng", GroupUtilityId = 18, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Xedapcongcong"},

                new RstProjectUtilityData() {Id = 128, Name = "Dọn vệ sinh toà nhà", GroupUtilityId = 19, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Donvesinhtoanha"},
                //new RstProjectUtilityData() {Id = 129, Name = "Dọn vệ sinh hàng lang", GroupUtilityId = 19, Type = RstProjectUtilityTypes.NOI_KHU},
                //new RstProjectUtilityData() {Id = 130, Name = "Dọn vệ sinh đường", GroupUtilityId = 19, Type = RstProjectUtilityTypes.NOI_KHU},
                new RstProjectUtilityData() {Id = 131, Name = "Thu rác", GroupUtilityId = 19, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Thurac"},
                //new RstProjectUtilityData() {Id = 132, Name = "Dọn dẹp nhà cửa", GroupUtilityId = 19, Type = RstProjectUtilityTypes.NOI_KHU},

                new RstProjectUtilityData() {Id = 133, Name = "Lễ tân tòa nhà", GroupUtilityId = 20, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Letantoanha"},
                new RstProjectUtilityData() {Id = 134, Name = "Bảo vệ an ninh", GroupUtilityId = 20, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Baoveanninh"},
                new RstProjectUtilityData() {Id = 135, Name = "Bảo vệ tòa nhà", GroupUtilityId = 20, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Baovetoanha"},
                new RstProjectUtilityData() {Id = 136, Name = "Bảo vệ hầm xe", GroupUtilityId = 20, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Baovehamxe"},
                new RstProjectUtilityData() {Id = 137, Name = "Bảo vệ sảnh ", GroupUtilityId = 20, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Baovesanh"},
                new RstProjectUtilityData() {Id = 138, Name = "Lễ tân sảnh", GroupUtilityId = 20, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Letansanh"},
                //new RstProjectUtilityData() {Id = 139, Name = "Bảo vệ khu vực", GroupUtilityId = 20, Type = RstProjectUtilityTypes.NOI_KHU},

                new RstProjectUtilityData() {Id = 140, Name = "Chăm sóc sắc đẹp", GroupUtilityId = 21, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Chamsocsacdep"},
                new RstProjectUtilityData() {Id = 141, Name = "Chăm sóc bảo trì xe máy", GroupUtilityId = 21, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Chamsocbaotrixemay"},
                new RstProjectUtilityData() {Id = 142, Name = "Chăm sóc bảo trì ô tô", GroupUtilityId = 21, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Chamsocbaotrioto"},
                new RstProjectUtilityData() {Id = 143, Name = "Chăm sóc cơ thể", GroupUtilityId = 21, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Chamsoccothe"},
                new RstProjectUtilityData() {Id = 144, Name = "Chăm sóc thú cưng", GroupUtilityId = 21, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Chamsocthucung"},
                new RstProjectUtilityData() {Id = 145, Name = "Chăm sóc người già", GroupUtilityId = 21, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Chamsocnguoigia"},
                new RstProjectUtilityData() {Id = 146, Name = "Chăm sóc người ốm", GroupUtilityId = 21, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Chamsocnguoiom"},
                new RstProjectUtilityData() {Id = 147, Name = "Chăm sóc cây cối", GroupUtilityId = 21, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Chamsoccaycoi"},
                new RstProjectUtilityData() {Id = 148, Name = "Dịch vụ chuyển nhà", GroupUtilityId = 21, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Dichvuchuyennha"},
                new RstProjectUtilityData() {Id = 149, Name = "Sửa chữa nhà cửa", GroupUtilityId = 21, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Suachuanhacua"},
                new RstProjectUtilityData() {Id = 150, Name = "Sửa chữa điện nước", GroupUtilityId = 21, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Suachuadiennuoc"},
                
                //Ngoại khu
                //new RstProjectUtilityData() {Id = 151, Name = "Công viên ven biển", GroupUtilityId = 1, Type = RstProjectUtilityTypes.NGOAI_KHU},
                new RstProjectUtilityData() {Id = 152, Name = "Công viên dã ngoại", GroupUtilityId = 1, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Congviendangoai"},
                //new RstProjectUtilityData() {Id = 153, Name = "Công viên san hô", GroupUtilityId = 1, Type = RstProjectUtilityTypes.NGOAI_KHU},
                //new RstProjectUtilityData() {Id = 154, Name = "Công viên giải trí", GroupUtilityId = 1, Type = RstProjectUtilityTypes.NGOAI_KHU},
                new RstProjectUtilityData() {Id = 155, Name = "Công viên thiên văn học", GroupUtilityId = 1, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Congvienthienvanhoc"},
                new RstProjectUtilityData() {Id = 156, Name = "Công viên nước", GroupUtilityId = 1, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Congviennuoc"},

                //new RstProjectUtilityData() {Id = 157, Name = "Chỗ đỗ xe ngoài trời", GroupUtilityId = 2, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Chodoxengoaitroi "},
                //new RstProjectUtilityData() {Id = 158, Name = "Nhà để xe", GroupUtilityId = 2, Type = RstProjectUtilityTypes.NGOAI_KHU},
                //new RstProjectUtilityData() {Id = 159, Name = "Bãi đỗ xe", GroupUtilityId = 2, Type = RstProjectUtilityTypes.NGOAI_KHU},

                new RstProjectUtilityData() {Id = 160, Name = "Bể bơi bốn mùa", GroupUtilityId = 3, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Beboibonmua"},
                new RstProjectUtilityData() {Id = 161, Name = "Bể bơi trong nhà", GroupUtilityId = 3, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Beboitrongnha"},
                new RstProjectUtilityData() {Id = 162, Name = "Bể bơi trên cao", GroupUtilityId = 3, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Beboitrencao"},
                new RstProjectUtilityData() {Id = 163, Name = "Bể bơi ngoài trời", GroupUtilityId = 3, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Beboingoaitroi"},

                new RstProjectUtilityData() {Id = 164, Name = "Cafe sân thượng", GroupUtilityId = 4, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Cafesanthuong"},
                new RstProjectUtilityData() {Id = 165, Name = "Cafe mặt đất", GroupUtilityId = 4, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Cafematdat"},
                new RstProjectUtilityData() {Id = 166, Name = "Cafe trong nhà", GroupUtilityId = 4, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Cafetrongnha"},

                new RstProjectUtilityData() {Id = 167, Name = "Hồ điều hòa", GroupUtilityId = 5, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Hodieuhoa"},
                new RstProjectUtilityData() {Id = 168, Name = "Biển hồ nước ngọt", GroupUtilityId = 5, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Bienhonuocngot"},
                //new RstProjectUtilityData() {Id = 169, Name = "Biển hồ nước mặn", GroupUtilityId = 5, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Bienhonuocman"},

                new RstProjectUtilityData() {Id = 170, Name = "Nhà bóng", GroupUtilityId = 6, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Nhabong"},
                new RstProjectUtilityData() {Id = 171, Name = "Cầu trượt", GroupUtilityId = 6, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Cautruot"},
                new RstProjectUtilityData() {Id = 172, Name = "Xích đu", GroupUtilityId = 6, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Xichdu"},

                new RstProjectUtilityData() {Id = 173, Name = "Trung tâm tiếng anh", GroupUtilityId = 7, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Trungtamngoaingu"},
                new RstProjectUtilityData() {Id = 174, Name = "Trung tâm ôn luyện thi", GroupUtilityId = 7, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Trungtamonluyenthi"},
                new RstProjectUtilityData() {Id = 175, Name = "Trung tâm dạy kỹ năng mềm", GroupUtilityId = 7, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Trungtamdaykynangmem"},
                //new RstProjectUtilityData() {Id = 176, Name = "Trung tâm dã ngoại", GroupUtilityId = 7, Type = RstProjectUtilityTypes.NGOAI_KHU},

                new RstProjectUtilityData() {Id = 177, Name = "Sân trượt băng", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Santruotbang"},
                new RstProjectUtilityData() {Id = 178, Name = "Sân trượt patin", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Santruotpatin"},
                new RstProjectUtilityData() {Id = 179, Name = "Khu game thực tế ảo", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Gamethucteao"},
                new RstProjectUtilityData() {Id = 180, Name = "Nhà ma", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Nhama"},
                new RstProjectUtilityData() {Id = 181, Name = "Karaoke", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Karaoke"},
                new RstProjectUtilityData() {Id = 182, Name = "Game cảm giác mạnh", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Gamecamgiacmanh"},
                new RstProjectUtilityData() {Id = 183, Name = "Karaoke mini", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_karaokemini"},
                new RstProjectUtilityData() {Id = 184, Name = "Rạp chiếu phim", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Rapchieuphim"},
                new RstProjectUtilityData() {Id = 185, Name = "Khu vui chơi ven biển", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Khuvuichoivenbien"},
                //new RstProjectUtilityData() {Id = 186, Name = "Khu thể thao bãi biển", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NGOAI_KHU},
                new RstProjectUtilityData() {Id = 187, Name = "Phố đi bộ", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Phodibo"},
                new RstProjectUtilityData() {Id = 188, Name = "Bowling", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Bowling"},
                new RstProjectUtilityData() {Id = 189, Name = "Billiard", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Billiard"},

                new RstProjectUtilityData() {Id = 190, Name = "Di sản văn hóa", GroupUtilityId = 9, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Disanvanhoa"},
                new RstProjectUtilityData() {Id = 191, Name = "Di tích lịch sử", GroupUtilityId = 9, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Ditichlichsu"},
                new RstProjectUtilityData() {Id = 192, Name = "Thủy cung", GroupUtilityId = 9, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Thuycung"},
                new RstProjectUtilityData() {Id = 193, Name = "Đài phun nước", GroupUtilityId = 9, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Daiphunnuoc"},
                new RstProjectUtilityData() {Id = 194, Name = "Tượng đài", GroupUtilityId = 9, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Tuongdai"},
                new RstProjectUtilityData() {Id = 195, Name = "Quảng trường", GroupUtilityId = 9, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Quangtruong"},
                new RstProjectUtilityData() {Id = 196, Name = "Nhạc nước", GroupUtilityId = 9, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Nhacnuoc"},
                new RstProjectUtilityData() {Id = 197, Name = "Triển lãm nghệ thuật", GroupUtilityId = 9, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Trienlamnghethuat"},
                //new RstProjectUtilityData() {Id = 197, Name = "Khu trưng bày", GroupUtilityId = 9, Type = RstProjectUtilityTypes.NGOAI_KHU},

                new RstProjectUtilityData() {Id = 198, Name = "Sân bóng đá", GroupUtilityId = 10, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Sanbongda"},
                new RstProjectUtilityData() {Id = 199, Name = "Sân bóng rổ", GroupUtilityId = 10, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Sanbongro"},
                new RstProjectUtilityData() {Id = 200, Name = "Sân tenis", GroupUtilityId = 10, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Santenis"},
                new RstProjectUtilityData() {Id = 201, Name = "Sân golf mini", GroupUtilityId = 10, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Sangolfmini"},
                new RstProjectUtilityData() {Id = 202, Name = "Sân tập võ", GroupUtilityId = 10, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Santapvo"},
                new RstProjectUtilityData() {Id = 203, Name = "Sân nhảy ngoài trời", GroupUtilityId = 10, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Sannhayngoaitroi"},
                new RstProjectUtilityData() {Id = 204, Name = "Phòng tập Yoga", GroupUtilityId = 10, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Phongtapyoga"},
                new RstProjectUtilityData() {Id = 205, Name = "Sân cầu lông", GroupUtilityId = 10, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Sancaulong"},
                new RstProjectUtilityData() {Id = 206, Name = "Khu chơi bóng bàn", GroupUtilityId = 10, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Khuchoibongban"},
                new RstProjectUtilityData() {Id = 207, Name = "Bộ dụng cụ thể thao ngoài trời", GroupUtilityId = 10, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Bodungcuthethaongoaitroi"},
                new RstProjectUtilityData() {Id = 208, Name = "Phòng Gym", GroupUtilityId = 10, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Phonggym"},

                new RstProjectUtilityData() {Id = 209, Name = "Cửa hàng thời trang", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Cuahangthoitrang"},
                new RstProjectUtilityData() {Id = 210, Name = "Cửa hàng bán lẻ", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Cuahangbanle"},
                new RstProjectUtilityData() {Id = 211, Name = "Cửa hàng đồ gia dụng", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Cuahangdogiadung"},
                new RstProjectUtilityData() {Id = 212, Name = "Cửa hàng nội thất", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Cuahangnoithat"},
                new RstProjectUtilityData() {Id = 213, Name = "Cửa hàng giặt là", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Cuahanggiatla"},
                new RstProjectUtilityData() {Id = 214, Name = "Cửa hàng cắt tóc", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Cuahangcattoc"},
                new RstProjectUtilityData() {Id = 215, Name = "Cửa hàng đồ chơi trẻ em", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Cuahangdochoitreem"},
                new RstProjectUtilityData() {Id = 216, Name = "Cửa hàng spa", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Cuahangspa"},
                new RstProjectUtilityData() {Id = 217, Name = "Cửa hàng nail", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Cuahangnail"},
                new RstProjectUtilityData() {Id = 218, Name = "Cửa hàng đồ thể thao", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Cuahangdothethao"},
                new RstProjectUtilityData() {Id = 219, Name = "Cửa hàng đồ công sở", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Cuahangdocongso"},
                new RstProjectUtilityData() {Id = 220, Name = "Cửa hàng tiện lợi", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Cuahangtienloi" },

                new RstProjectUtilityData() {Id = 221, Name = "Phố ẩm thực", GroupUtilityId = 12, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Phoamthuc"},
                //new RstProjectUtilityData() {Id = 222, Name = "Bar sân thượng", GroupUtilityId = 11, Type = RstProjectUtilityTypes.NGOAI_KHU},
                new RstProjectUtilityData() {Id = 224, Name = "Bar sân thượng", GroupUtilityId = 12, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Bar"},
                new RstProjectUtilityData() {Id = 223, Name = "Đồ ăn nhanh", GroupUtilityId = 12, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Doannhanh"},
                new RstProjectUtilityData() {Id = 225, Name = "Ẩm thực nước ngoài", GroupUtilityId = 12, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Amthucnuocngoai"},
                //new RstProjectUtilityData() {Id = 226, Name = "Pub sân thượng", GroupUtilityId = 12, Type = RstProjectUtilityTypes.NGOAI_KHU},
                //new RstProjectUtilityData() {Id = 227, Name = "Đồ ăn nhanh", GroupUtilityId = 12, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Doannhanh"},
                new RstProjectUtilityData() {Id = 228, Name = "Lẩu", GroupUtilityId = 12, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Lau"},
                new RstProjectUtilityData() {Id = 229, Name = "Đồ ăn vỉa hè", GroupUtilityId = 12, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Doanviahe"},
                new RstProjectUtilityData() {Id = 230, Name = "Đồ uống", GroupUtilityId = 12, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Douong"},

                new RstProjectUtilityData() {Id = 231, Name = "Khu vệ sinh công cộng", GroupUtilityId = 15, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Khuvesinhcongcong"},
                new RstProjectUtilityData() {Id = 232, Name = "Đèn chiếu sáng ngoài trời", GroupUtilityId = 15, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Denchieusangngoaitroi"},

                new RstProjectUtilityData() {Id = 233, Name = "Bệnh viện", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Benhvien"},
                new RstProjectUtilityData() {Id = 234, Name = "Trường mầm non", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Truongmannon"},
                new RstProjectUtilityData() {Id = 235, Name = "Trường tiểu học", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Truongtieuhoc"},
                new RstProjectUtilityData() {Id = 236, Name = "Trường THCS", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Truongthcs"},
                //new RstProjectUtilityData() {Id = 237, Name = "Trường liên cấp", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NGOAI_KHU},
                new RstProjectUtilityData() {Id = 238, Name = "Nhà chờ xe bus", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Nhachoxebus"},
                new RstProjectUtilityData() {Id = 239, Name = "Nhà thờ", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Nhatho"},
                new RstProjectUtilityData() {Id = 240, Name = "Chùa ", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Chua"},
                new RstProjectUtilityData() {Id = 241, Name = "Cơ quan nhà nước", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Coquannhanuoc"},
                new RstProjectUtilityData() {Id = 242, Name = "Ga tàu", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Gatau"},
                //new RstProjectUtilityData() {Id = 243, Name = "Bến xe", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NGOAI_KHU},
                new RstProjectUtilityData() {Id = 244, Name = "Trung tâm thương mại", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Trungtamthuongmai"},
                //new RstProjectUtilityData() {Id = 245, Name = "Trường THPT", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NGOAI_KHU},
                //new RstProjectUtilityData() {Id = 246, Name = "Trường đại học", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NGOAI_KHU},
                new RstProjectUtilityData() {Id = 247, Name = "Sân vườn", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Sanvuon"},
                //new RstProjectUtilityData() {Id = 248, Name = "Cây xanh", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NGOAI_KHU},
                new RstProjectUtilityData() {Id = 249, Name = "Vườn hoa", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Vuonhoa"},
                new RstProjectUtilityData() {Id = 250, Name = "Cây xanh vỉa hè", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Cayxanhviahe"},
                //new RstProjectUtilityData() {Id = 251, Name = "Phòng sinh hoạt cộng đồng", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NGOAI_KHU},

                //new RstProjectUtilityData() {Id = 252, Name = "Phòng cho thuê làm Workshop ngoài trời", GroupUtilityId = 17, Type = RstProjectUtilityTypes.NGOAI_KHU},
                //new RstProjectUtilityData() {Id = 253, Name = "Phòng cho thuê làm Workshop trong nhà", GroupUtilityId = 17, Type = RstProjectUtilityTypes.NGOAI_KHU},

                new RstProjectUtilityData() {Id = 254, Name = "Xe bus", GroupUtilityId = 18, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Xebus"},
                new RstProjectUtilityData() {Id = 255, Name = "Xe điện di chuyển trong khu", GroupUtilityId = 18, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Xediendichuyentrongkhu"},
                new RstProjectUtilityData() {Id = 256, Name = "Xe đạp công cộng", GroupUtilityId = 18, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Xedapcongcong"},

                //new RstProjectUtilityData() {Id = 257, Name = "Dọn vệ sinh đường", GroupUtilityId = 19, Type = RstProjectUtilityTypes.NGOAI_KHU},
                new RstProjectUtilityData() {Id = 258, Name = "Thu rác", GroupUtilityId = 19, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Thurac"},
                //new RstProjectUtilityData() {Id = 259, Name = "Dọn dẹp nhà cửa", GroupUtilityId = 19, Type = RstProjectUtilityTypes.NGOAI_KHU},

                new RstProjectUtilityData() {Id = 260, Name = "Bảo vệ an ninh", GroupUtilityId = 20, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Baoveanninh"},
                //new RstProjectUtilityData() {Id = 261, Name = "Bảo vệ khu vực", GroupUtilityId = 20, Type = RstProjectUtilityTypes.NGOAI_KHU},

                new RstProjectUtilityData() {Id = 262, Name = "Chăm sóc sắc đẹp", GroupUtilityId = 21, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Chamsocsacdep"},
                new RstProjectUtilityData() {Id = 263, Name = "Chăm sóc bảo trì xe máy", GroupUtilityId = 21, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Chamsocbaotrixemay"},
                new RstProjectUtilityData() {Id = 264, Name = "Chăm sóc bảo trì ô tô", GroupUtilityId = 21, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Chamsocbaotrioto"},
                new RstProjectUtilityData() {Id = 265, Name = "Chăm sóc cơ thể", GroupUtilityId = 21, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Chamsoccothe"},
                new RstProjectUtilityData() {Id = 266, Name = "Chăm sóc thú cưng", GroupUtilityId = 21, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Chamsocthucung"},
                new RstProjectUtilityData() {Id = 267, Name = "Chăm sóc người già", GroupUtilityId = 21, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Chamsocnguoigia"},
                new RstProjectUtilityData() {Id = 268, Name = "Chăm sóc người ốm", GroupUtilityId = 21, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Chamsocnguoiom"},
                new RstProjectUtilityData() {Id = 269, Name = "Chăm sóc cây cối", GroupUtilityId = 21, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Chamsoccaycoi"},
                new RstProjectUtilityData() {Id = 270, Name = "Dịch vụ chuyển nhà", GroupUtilityId = 21, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Dichvuchuyennha"},
                new RstProjectUtilityData() {Id = 271, Name = "Sửa chữa nhà cửa", GroupUtilityId = 21, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Suachuanhacua"},
                new RstProjectUtilityData() {Id = 272, Name = "Sửa chữa điện nước", GroupUtilityId = 21, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Suachuadiennuoc"},

                // Bổ sung Nội khu
                new RstProjectUtilityData() {Id = 273, Name = "Sân trượt băng", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Santruotbang"},
                new RstProjectUtilityData() {Id = 274, Name = "Khu game thự tế ảo", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Gamethucteao"},
                new RstProjectUtilityData() {Id = 275, Name = "Karaoke mini", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Karaokemini"},
                new RstProjectUtilityData() {Id = 276, Name = "Sân nhảy trong nhà", GroupUtilityId = 10, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Sannhaytrongnha"},
                new RstProjectUtilityData() {Id = 277, Name = "Bộ dụng cụ thể thao ngoài trời", GroupUtilityId = 10, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Bodungcuthethaongoaitroi"},
                new RstProjectUtilityData() {Id = 278, Name = "Trường quốc tế", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Truongquocte"},
                new RstProjectUtilityData() {Id = 279, Name = "Cây xăng", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Cayxang"},

                // Bổ sung Ngoại khu
                new RstProjectUtilityData() {Id = 280, Name = "Hồ tự nhiên", GroupUtilityId = 5, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Hotunhien"},
                new RstProjectUtilityData() {Id = 281, Name = "Sân nhảy trong nhà", GroupUtilityId = 10, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Sannhaytrongnha"},
                new RstProjectUtilityData() {Id = 282, Name = "Cây xăng", GroupUtilityId = 16, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Cayxang"},
                new RstProjectUtilityData() {Id = 283, Name = "Tàu điện", GroupUtilityId = 18, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Taudien"},

                // Bổ sung lần 2 
                // Nội khu
                new RstProjectUtilityData() {Id = 284, Name = "Phòng hội nghị", GroupUtilityId = 17, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Phonghoinghi"},
                new RstProjectUtilityData() {Id = 285, Name = "Phòng tổ chức sự kiện", GroupUtilityId = 17, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Phongtochucsukien"},

                new RstProjectUtilityData() {Id = 286, Name = "Nhà hàng", GroupUtilityId = 12, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Nhahang"},

                new RstProjectUtilityData() {Id = 287, Name = "Casino", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Casino"},
                new RstProjectUtilityData() {Id = 288, Name = "Bể sục", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Besuc"},
                new RstProjectUtilityData() {Id = 289, Name = "Phòng xông hơi", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Phongxonghoi"},
                new RstProjectUtilityData() {Id = 290, Name = "Khu tắm nắng", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Khutamnang"},
                new RstProjectUtilityData() {Id = 291, Name = "Khu vui chơi trẻ em", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Khuvuichoitreem"},

                new RstProjectUtilityData() {Id = 292, Name = "Bãi đỗ xe", GroupUtilityId = 2, Type = RstProjectUtilityTypes.NOI_KHU, Icon = "Icon_Baidoxe"},

                // Ngoại khu
                new RstProjectUtilityData() {Id = 293, Name = "Phòng hội nghị", GroupUtilityId = 17, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Phonghoinghi"},
                new RstProjectUtilityData() {Id = 294, Name = "Phòng tổ chức sự kiện", GroupUtilityId = 17, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Phongtochucsukien"},

                new RstProjectUtilityData() {Id = 295, Name = "Nhà hàng", GroupUtilityId = 12, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Nhahang"},

                new RstProjectUtilityData() {Id = 296, Name = "Casino", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Casino"},
                new RstProjectUtilityData() {Id = 297, Name = "Bể sục", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Besuc"},
                new RstProjectUtilityData() {Id = 298, Name = "Phòng xông hơi", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Phongxonghoi"},
                new RstProjectUtilityData() {Id = 299, Name = "Khu tắm nắng", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Khutamnang"},
                new RstProjectUtilityData() {Id = 300, Name = "Khu vui chơi trẻ em", GroupUtilityId = 8, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Khuvuichoitreem"},

                new RstProjectUtilityData() {Id = 301, Name = "Bãi đỗ xe", GroupUtilityId = 2, Type = RstProjectUtilityTypes.NGOAI_KHU, Icon = "Icon_Baidoxe"},

                // Tiện Ích Từng Căn
                //new RstProjectUtilityData() {Id = 284, Name = "Bể bơi riêng", GroupUtilityId = 22, Type = RstProjectUtilityTypes.TUNG_CAN_HO, Icon = "Icon_Beboirieng"},
                //new RstProjectUtilityData() {Id = 285, Name = "Phòng tập gym riêng", GroupUtilityId = 22, Type = RstProjectUtilityTypes.TUNG_CAN_HO, Icon = "Icon_Phongtapgymrieng"},
                //new RstProjectUtilityData() {Id = 286, Name = "View rộng, thoáng", GroupUtilityId = 22, Type = RstProjectUtilityTypes.TUNG_CAN_HO, Icon = "Icon_Viewrong"},
                //new RstProjectUtilityData() {Id = 287, Name = "Cầu thang riêng trong nhà", GroupUtilityId = 22, Type = RstProjectUtilityTypes.TUNG_CAN_HO, Icon = "Icon_Cauthangriengtrongnha"},
                //new RstProjectUtilityData() {Id = 288, Name = "Tiểu cảnh riêng", GroupUtilityId = 22, Type = RstProjectUtilityTypes.TUNG_CAN_HO, Icon = "Icon_Tieucanhrieng"},
                //new RstProjectUtilityData() {Id = 289, Name = "Không gian sang trọng", GroupUtilityId = 22, Type = RstProjectUtilityTypes.TUNG_CAN_HO, Icon = "Icon_Khonggiansangtrong"},
                //new RstProjectUtilityData() {Id = 290, Name = "Thang máy riêng", GroupUtilityId = 22, Type = RstProjectUtilityTypes.TUNG_CAN_HO, Icon = "Icon_Thangmayrieng"},
                //new RstProjectUtilityData() {Id = 291, Name = "Quầy bar trong nhà", GroupUtilityId = 22, Type = RstProjectUtilityTypes.TUNG_CAN_HO, Icon = "Icon_Quaybarrieng"},
                //new RstProjectUtilityData() {Id = 292, Name = "Vị trí đẹp của tòa nhà", GroupUtilityId = 22, Type = RstProjectUtilityTypes.TUNG_CAN_HO, Icon = "Icon_Vitridep"},
                //new RstProjectUtilityData() {Id = 293, Name = "Diện tích rộng", GroupUtilityId = 22, Type = RstProjectUtilityTypes.TUNG_CAN_HO, Icon = "Icon_Dientichrong"},
                //new RstProjectUtilityData() {Id = 294, Name = "Yên tĩnh", GroupUtilityId = 22, Type = RstProjectUtilityTypes.TUNG_CAN_HO, Icon = "Icon_Yentinh"},

        };
    }

    public class GroupRstProjectUtility
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static readonly List<GroupRstProjectUtility> GroupData = new()
        {
                new GroupRstProjectUtility() {Id = 1, Name = "Công viên"},
                new GroupRstProjectUtility() {Id = 2, Name = "Nơi để xe"},
                new GroupRstProjectUtility() {Id = 3, Name = "Bể bơi"},
                new GroupRstProjectUtility() {Id = 4, Name = "Cafe"},
                new GroupRstProjectUtility() {Id = 5, Name = "Biển hồ"},
                new GroupRstProjectUtility() {Id = 6, Name = "Khu vui chơi trẻ em"},
                new GroupRstProjectUtility() {Id = 7, Name = "Giáo dục"},
                new GroupRstProjectUtility() {Id = 8, Name = "Điểm vui chơi"},
                new GroupRstProjectUtility() {Id = 9, Name = "Công trình nghệ thuật"},
                new GroupRstProjectUtility() {Id = 10, Name = "Thể thao"},
                new GroupRstProjectUtility() {Id = 11, Name = "Cửa hàng"},
                new GroupRstProjectUtility() {Id = 12, Name = "Khu vực ăn uống"},
                new GroupRstProjectUtility() {Id = 13, Name = "Đi lại"},
                new GroupRstProjectUtility() {Id = 14, Name = "Hệ thống phục vụ an ninh"},
                new GroupRstProjectUtility() {Id = 15, Name = "Hệ thống phục vụ cho cư dân và khách"},
                new GroupRstProjectUtility() {Id = 16, Name = "Công trình công cộng"},
                new GroupRstProjectUtility() {Id = 17, Name = "Căn hộ dịch vụ"},
                new GroupRstProjectUtility() {Id = 18, Name = "Phương tiện công cộng"},
                new GroupRstProjectUtility() {Id = 19, Name = "Vệ sinh"},
                new GroupRstProjectUtility() {Id = 20, Name = "Bảo vệ"},
                new GroupRstProjectUtility() {Id = 21, Name = "Dịch vụ thường quy"},
                new GroupRstProjectUtility() {Id = 22, Name = "Tiện ích riêng dành cho căn hộ"},
        };
    }
}
