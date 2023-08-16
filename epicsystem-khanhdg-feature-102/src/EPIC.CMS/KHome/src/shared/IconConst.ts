export class IconConst {
    public static NOI_KHU = 1;
    public static NGOAI_KHU = 2;
    public static TUNG_CAN_HO = 3;

    public static CONG_VIEN = 1;
    public static NOI_DE_XE = 2;
    public static BE_BOI = 3;
    public static CAFE = 4;
    public static BIEN_HO = 5;
    public static KHU_VUI_CHOI_TRE_EM = 6;
    public static GIAO_DUC = 7;
    public static DIEM_VUI_CHOI = 8;
    public static CONG_TRINH_NGHE_THUAT = 9;
    public static THE_THAO = 10;
    public static CUA_HANG = 11;
    public static KHU_VUC_AN_UONG = 12;
    public static DI_LAI = 13;
    public static PHUC_VU_AN_NINH = 14;
    public static PHUC_VU_CU_DAN_VA_KHACH = 15;
    public static CONG_TRINH_CONG_CONG = 16;
    public static CAN_HO_DICH_VU = 17;
    public static PHUONG_TIEN_CONG_CONG = 18;
    public static VE_SINH = 19;
    public static BAO_VE = 20;
    public static DICH_VU_THUONG_QUY = 21;
    public static TIEN_ICH_RIENG_DANH_CHO_CAN_HO = 22;

    public static ListType = [
        {
            name: "Nội khu",
            code: this.NOI_KHU
        },
        {
            name: "Ngoại khu",
            code: this.NGOAI_KHU
        }
    ];

    public static ListGroup = [
        {
            name: "Công viên",
            code: this.CONG_VIEN
        },
        {
            name: "Nơi để xe",
            code: this.NOI_DE_XE
        },       
        {
            name: "Bể bơi",
            code: this.BE_BOI
        },       
        {
            name: "Cafe",
            code: this.CAFE
        },       
        {
            name: "Biển hồ",
            code: this.BIEN_HO
        },       
        {
            name: "Khu vui chơi trẻ em",
            code: this.KHU_VUI_CHOI_TRE_EM
        },       
        {
            name: "Giáo dục",
            code: this.GIAO_DUC
        },       
        {
            name: "Điểm vui chơi",
            code: this.DIEM_VUI_CHOI
        },       
        {
            name: "Công trình nghệ thuật",
            code: this.CONG_TRINH_NGHE_THUAT
        },       
        {
            name: "Thể thao",
            code: this.THE_THAO
        },       
        {
            name: "Cửa hàng",
            code: this.CUA_HANG
        },       
        {
            name: "Khu vực ăn uống",
            code: this.KHU_VUC_AN_UONG
        },       
        {
            name: "Đi lại",
            code: this.DI_LAI
        },       
        {
            name: "Phục vụ an ninh",
            code: this.PHUC_VU_AN_NINH
        },       
        {
            name: "Phục vụ cư dân và khách",
            code: this.PHUC_VU_CU_DAN_VA_KHACH
        },
        {
            name: "Công trình công cộng",
            code: this.CONG_TRINH_CONG_CONG
        },
        {
            name: "Căn hộ dịch vụ",
            code: this.CAN_HO_DICH_VU
        },
        {
            name: "Phương tiện công cộng",
            code: this.PHUONG_TIEN_CONG_CONG
        },
        {
            name: "Vệ sinh",
            code: this.VE_SINH
        },
        {
            name: "Bảo vệ",
            code: this.BAO_VE
        },
        {
            name: "Dịch vụ thường quy",
            code: this.DICH_VU_THUONG_QUY
        },
        {
            name: "Tiện ích riêng dành cho căn hộ",
            code: this.TIEN_ICH_RIENG_DANH_CHO_CAN_HO
        },
    ];

    public static List: any = [
        // Nội khu
        {id: 1, name: "Công viên cây xanh", groupUtilityId: this.CONG_VIEN, type: this.NOI_KHU, icon: "Icon_Congviencayxanh"},
        
        {id: 2, name: "Hầm để xe dưới mặt đất", groupUtilityId: this.NOI_DE_XE, type: this.NOI_KHU, icon: "Icon_Hamdexeduoimatdat"},
        {id: 3, name: "Hầm để ô tô", groupUtilityId: this.NOI_DE_XE, type: this.NOI_KHU, icon: "Icon_Hamdeoto"},
        {id: 4, name: "Hầm để xe máy", groupUtilityId: this.NOI_DE_XE, type: this.NOI_KHU, icon: "Icon_Hamdexemay"},
        {id: 5, name: "Trạm sạc xe điện trong hầm", groupUtilityId: this.NOI_DE_XE, type: this.NOI_KHU, icon: "Icon_Tramsacdientrongham"},
        {id: 6, name: "Trạm sạc xe điện ngoài trời", groupUtilityId: this.NOI_DE_XE, type: this.NOI_KHU, icon: "Icon_Tramsacdienngoaitroi"},

        {id: 7, name: "Bể bơi bốn mùa", groupUtilityId: this.BE_BOI, type: this.NOI_KHU, icon: "Icon_Beboibonmua"},
        {id: 8, name: "Bể bơi trong nhà", groupUtilityId: this.BE_BOI, type: this.NOI_KHU, icon: "Icon_Beboitrongnha"},
        {id: 9, name: "Bể bơi trên cao", groupUtilityId: this.BE_BOI, type: this.NOI_KHU, icon: "Icon_Beboitrencao"},
        {id: 10, name: "Bể bơi ngoài trời", groupUtilityId: this.BE_BOI, type: this.NOI_KHU, icon: "Icon_Beboingoaitroi"},
        
        {id: 11, name: "Cafe sân thượng", groupUtilityId: this.CAFE, type: this.NOI_KHU, icon: "Icon_Cafesanthuong"},
        {id: 12, name: "Cafe mặt đất", groupUtilityId: this.CAFE, type: this.NOI_KHU, icon: "Icon_Cafematdat"},
        {id: 14, name: "Cafe trong nhà", groupUtilityId: this.CAFE, type: this.NOI_KHU, icon: "Icon_Cafetrongnha"},
        
        {id: 15, name: "Hồ điều hòa", groupUtilityId: this.BIEN_HO, type: this.NOI_KHU, icon: "Icon_Hodieuhoa"},
        {id: 16, name: "Biển hồ nước ngọt", groupUtilityId: this.BIEN_HO, type: this.NOI_KHU, icon: "Icon_Bienhonuocngot"},
        {id: 17, name: "Biển hồ nước mặn", groupUtilityId: this.BIEN_HO, type: this.NOI_KHU, icon: "Icon_Bienhonuocman"},
        
        {id: 18, name: "Nhà bóng", groupUtilityId: this.KHU_VUI_CHOI_TRE_EM, type: this.NOI_KHU, icon: "Icon_Nhabong"},
        {id: 19, name: "Cầu trượt", groupUtilityId: this.KHU_VUI_CHOI_TRE_EM, type: this.NOI_KHU, icon: "Icon_Cautruot"},
        {id: 20, name: "Xích đu", groupUtilityId: this.KHU_VUI_CHOI_TRE_EM, type: this.NOI_KHU, icon: "Icon_Xichdu"},

        {id: 21, name: "Trung tâm tiếng anh", groupUtilityId: this.GIAO_DUC, type: this.NOI_KHU, icon: "Icon_Trungtamngoaingu"},
        {id: 22, name: "Trung tâm ôn luyện thi", groupUtilityId: this.GIAO_DUC, type: this.NOI_KHU, icon: "Icon_Trungtamonluyenthi"},
        {id: 23, name: "Trung tâm dạy kỹ năng mềm", groupUtilityId: this.GIAO_DUC, type: this.NOI_KHU, icon: "Icon_Trungtamdaykynangmem"},
        
        {id: 25, name: "Sân trượt patin", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NOI_KHU, icon: "Icon_Santruotpatin"},
        {id: 26, name: "Nhà ma", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NOI_KHU, icon: "Icon_Nhama"},
        {id: 27, name: "Karaoke", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NOI_KHU, icon: "Icon_Karaoke"},
        {id: 28, name: "Game cảm giác mạnh", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NOI_KHU, icon: "Icon_Gamecamgiacmanh"},
        {id: 29, name: "Rạp chiếu phim", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NOI_KHU, icon: "Icon_Rapchieuphim"},
        {id: 30, name: "Khu vui chơi ven biển", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NOI_KHU, icon: "Icon_Khuvuichoivenbien"},
        {id: 32, name: "Phố đi bộ", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NOI_KHU, icon: "Icon_Phodibo"},
        {id: 33, name: "Bowling", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NOI_KHU, icon: "Icon_Bowling"},
        {id: 34, name: "Billiard", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NOI_KHU, icon: "Icon_Billiard"},
        
        {id: 35, name: "Di sản văn hóa", groupUtilityId: this.CONG_TRINH_NGHE_THUAT, type: this.NOI_KHU, icon: "Icon_Disanvanhoa"},
        {id: 36, name: "Di tích lịch sử", groupUtilityId: this.CONG_TRINH_NGHE_THUAT, type: this.NOI_KHU, icon: "Icon_Ditichlichsu"},
        {id: 37, name: "Thủy cung", groupUtilityId: this.CONG_TRINH_NGHE_THUAT, type: this.NOI_KHU, icon: "Icon_Thuycung"},
        {id: 38, name: "Đài phun nước", groupUtilityId: this.CONG_TRINH_NGHE_THUAT, type: this.NOI_KHU, icon: "Icon_Daiphunnuoc"},
        {id: 39, name: "Tượng đài", groupUtilityId: this.CONG_TRINH_NGHE_THUAT, type: this.NOI_KHU, icon: "Icon_Tuongdai"},
        {id: 40, name: "Quảng trường", groupUtilityId: this.CONG_TRINH_NGHE_THUAT, type: this.NOI_KHU, icon: "Icon_Quangtruong"},
        {id: 41, name: "Nhạc nước", groupUtilityId: this.CONG_TRINH_NGHE_THUAT, type: this.NOI_KHU, icon: "Icon_Nhacnuoc"},
        {id: 42, name: "Triển lãm nghệ thuật", groupUtilityId: this.CONG_TRINH_NGHE_THUAT, type: this.NOI_KHU, icon: "Icon_Trienlamnghethuat"},
       
        {id: 44, name: "Sân bóng đá", groupUtilityId: this.THE_THAO, type: this.NOI_KHU, icon: "Icon_Sanbongda"},
        {id: 45, name: "Sân bóng rổ", groupUtilityId: this.THE_THAO, type: this.NOI_KHU, icon: "Icon_Sanbongro"},
        {id: 46, name: "Sân tenis", groupUtilityId: this.THE_THAO, type: this.NOI_KHU, icon: "Icon_Santenis"},
        {id: 47, name: "Sân golf mini", groupUtilityId: this.THE_THAO, type: this.NOI_KHU, icon: "Icon_Sangolfmini"},
        {id: 48, name: "Sân tập võ", groupUtilityId: this.THE_THAO, type: this.NOI_KHU, icon: "Icon_Santapvo"},
        {id: 49, name: "Sân nhảy ngoài trời", groupUtilityId: this.THE_THAO, type: this.NOI_KHU, icon: "Icon_Sannhayngoaitroi"},
        {id: 50, name: "Phòng tập Yoga", groupUtilityId: this.THE_THAO, type: this.NOI_KHU, icon: "Icon_Phongtapyoga"},
        {id: 51, name: "Sân cầu lông", groupUtilityId: this.THE_THAO, type: this.NOI_KHU, icon: "Icon_Sancaulong"},
        {id: 52, name: "Khu chơi bóng bàn", groupUtilityId: this.THE_THAO, type: this.NOI_KHU, icon: "Icon_Khuchoibongban"},
        {id: 53, name: "Phòng Gym", groupUtilityId: this.THE_THAO, type: this.NOI_KHU, icon: "Icon_Phonggym"},
        
        {id: 54, name: "Cửa hàng thời trang", groupUtilityId: this.CUA_HANG, type: this.NOI_KHU, icon: "Icon_Cuahangthoitrang"},
        {id: 55, name: "Cửa hàng bán lẻ", groupUtilityId: this.CUA_HANG, type: this.NOI_KHU, icon: "Icon_Cuahangbanle"},
        {id: 56, name: "Cửa hàng đồ gia dụng", groupUtilityId: this.CUA_HANG, type: this.NOI_KHU, icon: "Icon_Cuahangdogiadung"},
        {id: 57, name: "Cửa hàng nội thất", groupUtilityId: this.CUA_HANG, type: this.NOI_KHU, icon: "Icon_Cuahangnoithat"},
        {id: 58, name: "Cửa hàng giặt là", groupUtilityId: this.CUA_HANG, type: this.NOI_KHU, icon: "Icon_Cuahanggiatla"},
        {id: 59, name: "Cửa hàng cắt tóc", groupUtilityId: this.CUA_HANG, type: this.NOI_KHU, icon: "Icon_Cuahangcattoc"},
        {id: 60, name: "Cửa hàng đồ chơi trẻ em", groupUtilityId: this.CUA_HANG, type: this.NOI_KHU, icon: "Icon_Cuahangdochoitreem"},
        {id: 61, name: "Cửa hàng spa", groupUtilityId: this.CUA_HANG, type: this.NOI_KHU, icon: "Icon_Cuahangspa"},
        {id: 62, name: "Cửa hàng nail", groupUtilityId: this.CUA_HANG, type: this.NOI_KHU, icon: "Icon_Cuahangnail"},
        {id: 63, name: "Cửa hàng đồ thể thao", groupUtilityId: this.CUA_HANG, type: this.NOI_KHU, icon: "Icon_Cuahangdothethao"},
        {id: 64, name: "Cửa hàng đồ công sở", groupUtilityId: this.CUA_HANG, type: this.NOI_KHU, icon: "Icon_Cuahangdocongso"},
        {id: 65, name: "Cửa hàng tiện lợi", groupUtilityId: this.CUA_HANG, type: this.NOI_KHU, icon: "Icon_Cuahangtienloi"},
        
        {id: 66, name: "BBQ ngoài trời", groupUtilityId: this.CUA_HANG, type: this.NOI_KHU, icon: "Icon_BBQngoaitroi"},
        {id: 67, name: "Phố ẩm thực", groupUtilityId: this.CUA_HANG, type: this.NOI_KHU, icon: "Icon_Phoamthuc"},
        {id: 70, name: "Đồ ăn nhanh", groupUtilityId: this.CUA_HANG, type: this.NOI_KHU, icon: "Icon_Doannhanh"},
        {id: 71, name: "Ẩm thực nước ngoài", groupUtilityId: this.CUA_HANG, type: this.NOI_KHU, icon: "Icon_Amthucnuocngoai"},
        {id: 74, name: "Lẩu", groupUtilityId: this.CUA_HANG, type: this.NOI_KHU, icon: "Icon_Lau"},
        {id: 75, name: "Đồ ăn vỉa hè", groupUtilityId: this.CUA_HANG, type: this.NOI_KHU, icon: "Icon_Doanviahe"},
        {id: 76, name: "Đồ uống", groupUtilityId: this.CUA_HANG, type: this.NOI_KHU, icon: "Icon_Douong"},
        
        {id: 77, name: "Cầu thang bộ", groupUtilityId: this.DI_LAI, type: this.NOI_KHU, icon: "Icon_Cauthangbo"},
        {id: 79, name: "Thang máy cho cư dân", groupUtilityId: this.DI_LAI, type: this.NOI_KHU, icon: "Icon_Thangmaychocudan"},
        {id: 80, name: "Thang máy cho khu văn phòng", groupUtilityId: this.DI_LAI, type: this.NOI_KHU, icon: "Icon_Thangmaychokhuvanphong"},
        {id: 81, name: "Cầu thang cuốn", groupUtilityId: this.DI_LAI, type: this.NOI_KHU, icon: "Icon_Cauthangcuon"},
        {id: 82, name: "Thang cuốn", groupUtilityId: this.DI_LAI, type: this.NOI_KHU, icon: "Icon_Thangcuon"},
       
        {id: 83, name: "Hệ thống báo cháy", groupUtilityId: this.PHUC_VU_AN_NINH, type: this.NOI_KHU, icon: "Icon_Hethongbaochay"},
        {id: 84, name: "Hệ thống chữa cháy", groupUtilityId: this.PHUC_VU_AN_NINH, type: this.NOI_KHU, icon: "Icon_Hethongchuachay"},
        {id: 85, name: "Đèn thoát hiểm", groupUtilityId: this.PHUC_VU_AN_NINH, type: this.NOI_KHU, icon: "Icon_Denthoathiem"},
        {id: 86, name: "Hệ thống camera trong tòa nhà", groupUtilityId: this.PHUC_VU_AN_NINH, type: this.NOI_KHU, icon: "Icon_Hethongcameratrongtoanha"},
        {id: 87, name: "Máy phát điện", groupUtilityId: this.PHUC_VU_AN_NINH, type: this.NOI_KHU, icon: "Icon_Mayphatdien"},
        {id: 88, name: "Hệ thống camera ngoài trời", groupUtilityId: this.PHUC_VU_AN_NINH, type: this.NOI_KHU, icon: "Icon_Hethongcamerangoaitroi"},
        {id: 90, name: "Hệ thống thoát nước", groupUtilityId: this.PHUC_VU_AN_NINH, type: this.NOI_KHU, icon: "Icon_Hethongthoatnuoc"},
        {id: 91, name: "Hệ thống dây diện ngầm", groupUtilityId: this.PHUC_VU_AN_NINH, type: this.NOI_KHU, icon: "Icon_Hethongdaydienngam"},
        {id: 92, name: "Hệ thống điều hòa căn hộ", groupUtilityId: this.PHUC_VU_AN_NINH, type: this.NOI_KHU, icon: "Icon_Hethongdieuhoacanho"},
        {id: 93, name: "Hệ thống điều hòa toàn nhà", groupUtilityId: this.PHUC_VU_AN_NINH, type: this.NOI_KHU, icon: "Icon_Hethongdieuhoatoanha"},
        {id: 94, name: "Cáp quang", groupUtilityId: this.PHUC_VU_AN_NINH, type: this.NOI_KHU, icon: "Icon_Capquang"},
       
        {id: 96, name: "Khu vệ sinh công cộng", groupUtilityId: this.PHUC_VU_CU_DAN_VA_KHACH, type: this.NOI_KHU, icon: "Icon_Khuvesinhcongcong"},
        {id: 97, name: "Đèn chiếu sáng ngoài trời", groupUtilityId: this.PHUC_VU_CU_DAN_VA_KHACH, type: this.NOI_KHU, icon: "Icon_Denchieusangngoaitroi"},
        {id: 99, name: "Thẻ ra vào tòa nhà", groupUtilityId: this.PHUC_VU_CU_DAN_VA_KHACH, type: this.NOI_KHU, icon: "Icon_Theravaotoanha"},
        {id: 100, name: "Ra vào bằng nhận diện khuôn mặt", groupUtilityId: this.PHUC_VU_CU_DAN_VA_KHACH, type: this.NOI_KHU, icon: "Icon_Nhandienkhuonmat"},
       
        {id: 101, name: "Bệnh viện", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NOI_KHU, icon: "Icon_Benhvien"},
        {id: 102, name: "Trường mầm non", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NOI_KHU, icon: "Icon_Truongmannon"},
        {id: 103, name: "Trường tiểu học", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NOI_KHU, icon: "Icon_Truongtieuhoc"},
        {id: 104, name: "Trường THCS", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NOI_KHU, icon: "Icon_Truongthcs"},
        {id: 106, name: "Nhà chờ xe bus", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NOI_KHU, icon: "Icon_Nhachoxebus"},
        {id: 107, name: "Nhà thờ", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NOI_KHU, icon: "Icon_Nhatho"},
        {id: 110, name: "Trung tâm thương mại", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NOI_KHU, icon: "Icon_Trungtamthuongmai"},
        {id: 114, name: "Sân vườn", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NOI_KHU, icon: "Icon_Sanvuon"},
        {id: 116, name: "Vườn hoa", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NOI_KHU, icon: "Icon_Vuonhoa"},
        {id: 117, name: "Cây xanh vỉa hè", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NOI_KHU, icon: "Icon_Cayxanhviahe"},
       
        {id: 122, name: "Căn hộ cho thuê để ở", groupUtilityId: this.CAN_HO_DICH_VU, type: this.NOI_KHU, icon: "Icon_Canhochothuedeo"},
        {id: 123, name: "Căn hộ cho thuê làm văn phòng", groupUtilityId: this.CAN_HO_DICH_VU, type: this.NOI_KHU, icon: "Icon_Canhochothuelamvanphong"},

        {id: 125, name: "Xe bus", groupUtilityId: this.PHUONG_TIEN_CONG_CONG, type: this.NOI_KHU, icon: "Icon_Xebus"},
        {id: 126, name: "Xe điện di chuyển trong khu", groupUtilityId: this.PHUONG_TIEN_CONG_CONG, type: this.NOI_KHU, icon: "Icon_Xediendichuyentrongkhu"},
        {id: 127, name: "Xe đạp công cộng", groupUtilityId: this.PHUONG_TIEN_CONG_CONG, type: this.NOI_KHU, icon: "Icon_Xedapcongcong"},
        
        {id: 128, name: "Dọn vệ sinh toà nhà", groupUtilityId: this.PHUONG_TIEN_CONG_CONG, type: this.NOI_KHU, icon: "Icon_Xedapcongcong"},
        {id: 131, name: "Thu rác", groupUtilityId: this.PHUONG_TIEN_CONG_CONG, type: this.NOI_KHU, icon: "Icon_Xedapcongcong"},
        
        {id: 133, name: "Lễ tân tòa nhà", groupUtilityId: this.BAO_VE, type: this.NOI_KHU, icon: "Icon_Letantoanha"},
        {id: 134, name: "Bảo vệ an ninh", groupUtilityId: this.BAO_VE, type: this.NOI_KHU, icon: "Icon_Baoveanninh"},
        {id: 135, name: "Bảo vệ tòa nhà", groupUtilityId: this.BAO_VE, type: this.NOI_KHU, icon: "Icon_Baovetoanha"},
        {id: 136, name: "Bảo vệ hầm xe", groupUtilityId: this.BAO_VE, type: this.NOI_KHU, icon: "Icon_Baovehamxe"},
        {id: 137, name: "Bảo vệ sảnh", groupUtilityId: this.BAO_VE, type: this.NOI_KHU, icon: "Icon_Baovesanh"},
        {id: 138, name: "Lễ tân sảnh", groupUtilityId: this.BAO_VE, type: this.NOI_KHU, icon: "Icon_Letansanh"},
      
        {id: 140, name: "Chăm sóc sắc đẹp", groupUtilityId: this.DICH_VU_THUONG_QUY, type: this.NOI_KHU, icon: "Icon_Chamsocsacdep"},
        {id: 141, name: "Chăm sóc bảo trì xe máy", groupUtilityId: this.DICH_VU_THUONG_QUY, type: this.NOI_KHU, icon: "Icon_Chamsocbaotrixemay"},
        {id: 142, name: "Chăm sóc bảo trì ô tô", groupUtilityId: this.DICH_VU_THUONG_QUY, type: this.NOI_KHU, icon: "Icon_Chamsocbaotrioto"},
        {id: 143, name: "Chăm sóc cơ thể", groupUtilityId: this.DICH_VU_THUONG_QUY, type: this.NOI_KHU, icon: "Icon_Chamsoccothe"},
        {id: 144, name: "Chăm sóc thú cưng", groupUtilityId: this.DICH_VU_THUONG_QUY, type: this.NOI_KHU, icon: "Icon_Chamsocthucung"},
        {id: 145, name: "Chăm sóc người già", groupUtilityId: this.DICH_VU_THUONG_QUY, type: this.NOI_KHU, icon: "Icon_Chamsocnguoigia"},
        {id: 146, name: "Chăm sóc người ốm", groupUtilityId: this.DICH_VU_THUONG_QUY, type: this.NOI_KHU, icon: "Icon_Chamsocnguoiom"},
        {id: 147, name: "Chăm sóc cây cối", groupUtilityId: this.DICH_VU_THUONG_QUY, type: this.NOI_KHU, icon: "Icon_Chamsoccaycoi"},
        {id: 148, name: "Dịch vụ chuyển nhà", groupUtilityId: this.DICH_VU_THUONG_QUY, type: this.NOI_KHU, icon: "Icon_Dichvuchuyennha"},
        {id: 149, name: "Sửa chữa nhà cửa", groupUtilityId: this.DICH_VU_THUONG_QUY, type: this.NOI_KHU, icon: "Icon_Suachuanhacua"},
        {id: 150, name: "Sửa chữa điện nước", groupUtilityId: this.DICH_VU_THUONG_QUY, type: this.NOI_KHU, icon: "Icon_Suachuadiennuoc"},
    
        // Ngoại khu
        {id: 152, name: "Công viên dã ngoại", groupUtilityId: this.CONG_VIEN, type: this.NGOAI_KHU, icon: "Icon_Congviendangoai"},
        {id: 155, name: "Công viên thiên văn học", groupUtilityId: this.CONG_VIEN, type: this.NGOAI_KHU, icon: "Icon_Congvienthienvanhoc"},
        {id: 156, name: "Công viên nước", groupUtilityId: this.CONG_VIEN, type: this.NGOAI_KHU, icon: "Icon_Congviennuoc"},
        
        {id: 160, name: "Bể bơi bốn mùa", groupUtilityId: this.BE_BOI, type: this.NGOAI_KHU, icon: "Icon_Beboibonmua"},
        {id: 161, name: "Bể bơi trong nhà", groupUtilityId: this.BE_BOI, type: this.NGOAI_KHU, icon: "Icon_Beboitrongnha"},
        {id: 162, name: "Bể bơi trên cao", groupUtilityId: this.BE_BOI, type: this.NGOAI_KHU, icon: "Icon_Beboitrencao"},
        {id: 163, name: "Bể bơi ngoài trời", groupUtilityId: this.BE_BOI, type: this.NGOAI_KHU, icon: "Icon_Beboingoaitroi"},
        
        {id: 164, name: "Cafe sân thượng", groupUtilityId: this.CAFE, type: this.NGOAI_KHU, icon: "Icon_Cafesanthuong"},
        {id: 165, name: "Cafe mặt đất", groupUtilityId: this.CAFE, type: this.NGOAI_KHU, icon: "Icon_Cafematdat"},
        {id: 166, name: "Cafe trong nhà", groupUtilityId: this.CAFE, type: this.NGOAI_KHU, icon: "Icon_Cafetrongnha"},
       
        {id: 167, name: "Hồ điều hòa", groupUtilityId: this.BIEN_HO, type: this.NGOAI_KHU, icon: "Icon_Hodieuhoa"},
        {id: 168, name: "Biển hồ nước ngọt", groupUtilityId: this.BIEN_HO, type: this.NGOAI_KHU, icon: "Icon_Bienhonuocngot"},
       
        {id: 170, name: "Nhà bóng", groupUtilityId: this.KHU_VUI_CHOI_TRE_EM, type: this.NGOAI_KHU, icon: "Icon_Nhabong"},
        {id: 171, name: "Cầu trượt", groupUtilityId: this.KHU_VUI_CHOI_TRE_EM, type: this.NGOAI_KHU, icon: "Icon_Cautruot"},
        {id: 172, name: "Xích đu", groupUtilityId: this.KHU_VUI_CHOI_TRE_EM, type: this.NGOAI_KHU, icon: "Icon_Xichdu"},
        
        {id: 173, name: "Trung tâm tiếng anh", groupUtilityId: this.GIAO_DUC, type: this.NGOAI_KHU, icon: "Icon_Trungtamngoaingu"},
        {id: 174, name: "Trung tâm ôn luyện thi", groupUtilityId: this.GIAO_DUC, type: this.NGOAI_KHU, icon: "Icon_Trungtamonluyenthi"},
        {id: 175, name: "Trung tâm dạy kỹ năng mềm", groupUtilityId: this.GIAO_DUC, type: this.NGOAI_KHU, icon: "Icon_Trungtamdaykynangmem"},
      
        {id: 177, name: "Sân trượt băng", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NGOAI_KHU, icon: "Icon_Santruotbang"},
        {id: 178, name: "Sân trượt patin", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NGOAI_KHU, icon: "Icon_Santruotpatin"},
        {id: 179, name: "Khu game thực tế ảo", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NGOAI_KHU, icon: "Icon_Gamethucteao"},
        {id: 180, name: "Nhà ma", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NGOAI_KHU, icon: "Icon_Nhama"},
        {id: 181, name: "Karaoke", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NGOAI_KHU, icon: "Icon_Karaoke"},
        {id: 182, name: "Game cảm giác mạnh", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NGOAI_KHU, icon: "Icon_Gamecamgiacmanh"},
        {id: 183, name: "Karaoke mini", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NGOAI_KHU, icon: "Icon_Karaokemini"},
        {id: 184, name: "Rạp chiếu phim", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NGOAI_KHU, icon: "Icon_Rapchieuphim"},
        {id: 185, name: "Khu vui chơi ven biển", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NGOAI_KHU, icon: "Icon_Khuvuichoivenbien"},
        {id: 187, name: "Phố đi bộ", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NGOAI_KHU, icon: "Icon_Phodibo"},
        {id: 188, name: "Bowling", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NGOAI_KHU, icon: "Icon_Bowling"},
        {id: 189, name: "Billiard", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NGOAI_KHU, icon: "Icon_Billiard"},
        
        {id: 190, name: "Di sản văn hóa", groupUtilityId: this.CONG_TRINH_NGHE_THUAT, type: this.NGOAI_KHU, icon: "Icon_Disanvanhoa"},
        {id: 191, name: "Di tích lịch sử", groupUtilityId: this.CONG_TRINH_NGHE_THUAT, type: this.NGOAI_KHU, icon: "Icon_Ditichlichsu"},
        {id: 192, name: "Thủy cung", groupUtilityId: this.CONG_TRINH_NGHE_THUAT, type: this.NGOAI_KHU, icon: "Icon_Thuycung"},
        {id: 193, name: "Đài phun nước", groupUtilityId: this.CONG_TRINH_NGHE_THUAT, type: this.NGOAI_KHU, icon: "Icon_Daiphunnuoc"},
        {id: 194, name: "Tượng đài", groupUtilityId: this.CONG_TRINH_NGHE_THUAT, type: this.NGOAI_KHU, icon: "Icon_Tuongdai"},
        {id: 195, name: "Quảng trường", groupUtilityId: this.CONG_TRINH_NGHE_THUAT, type: this.NGOAI_KHU, icon: "Icon_Quangtruong"},
        {id: 196, name: "Nhạc nước", groupUtilityId: this.CONG_TRINH_NGHE_THUAT, type: this.NGOAI_KHU, icon: "Icon_Nhacnuoc"},
        {id: 197, name: "Triển lãm nghệ thuật", groupUtilityId: this.CONG_TRINH_NGHE_THUAT, type: this.NGOAI_KHU, icon: "Icon_Trienlamnghethuat"},
        
        {id: 198, name: "Sân bóng đá", groupUtilityId: this.THE_THAO, type: this.NGOAI_KHU, icon: "Icon_Sanbongda"},
        {id: 199, name: "Sân bóng rổ", groupUtilityId: this.THE_THAO, type: this.NGOAI_KHU, icon: "Icon_Sanbongro"},
        {id: 200, name: "Sân tenis", groupUtilityId: this.THE_THAO, type: this.NGOAI_KHU, icon: "Icon_Santenis"},
        {id: 201, name: "Sân golf mini", groupUtilityId: this.THE_THAO, type: this.NGOAI_KHU, icon: "Icon_Sangolfmini"},
        {id: 202, name: "Sân tập võ", groupUtilityId: this.THE_THAO, type: this.NGOAI_KHU, icon: "Icon_Santapvo"},
        {id: 203, name: "Sân nhảy ngoài trời", groupUtilityId: this.THE_THAO, type: this.NGOAI_KHU, icon: "Icon_Sannhayngoaitroi"},
        {id: 204, name: "Phòng tập Yoga", groupUtilityId: this.THE_THAO, type: this.NGOAI_KHU, icon: "Icon_Phongtapyoga"},
        {id: 205, name: "Sân cầu lông", groupUtilityId: this.THE_THAO, type: this.NGOAI_KHU, icon: "Icon_Sancaulong"},
        {id: 206, name: "Khu chơi bóng bàn", groupUtilityId: this.THE_THAO, type: this.NGOAI_KHU, icon: "Icon_Khuchoibongban"},
        {id: 207, name: "Bộ dụng cụ thể thao ngoài trời", groupUtilityId: this.THE_THAO, type: this.NGOAI_KHU, icon: "Icon_Bodungcuthethaongoaitroi"},
        {id: 208, name: "Phòng Gym", groupUtilityId: this.THE_THAO, type: this.NGOAI_KHU, icon: "Icon_Phonggym"},
       
        {id: 209, name: "Cửa hàng thời trang", groupUtilityId: this.CUA_HANG, type: this.NGOAI_KHU, icon: "Icon_Cuahangthoitrang"},
        {id: 210, name: "Cửa hàng bán lẻ", groupUtilityId: this.CUA_HANG, type: this.NGOAI_KHU, icon: "Icon_Cuahangbanle"},
        {id: 211, name: "Cửa hàng đồ gia dụng", groupUtilityId: this.CUA_HANG, type: this.NGOAI_KHU, icon: "Icon_Cuahangdogiadung"},
        {id: 212, name: "Cửa hàng nội thất", groupUtilityId: this.CUA_HANG, type: this.NGOAI_KHU, icon: "Icon_Cuahangnoithat"},
        {id: 213, name: "Cửa hàng giặt là", groupUtilityId: this.CUA_HANG, type: this.NGOAI_KHU, icon: "Icon_Cuahanggiatla"},
        {id: 214, name: "Cửa hàng cắt tóc", groupUtilityId: this.CUA_HANG, type: this.NGOAI_KHU, icon: "Icon_Cuahangcattoc"},
        {id: 215, name: "Cửa hàng đồ chơi trẻ em", groupUtilityId: this.CUA_HANG, type: this.NGOAI_KHU, icon: "Icon_Cuahangdochoitreem"},
        {id: 216, name: "Cửa hàng spa", groupUtilityId: this.CUA_HANG, type: this.NGOAI_KHU, icon: "Icon_Cuahangspa"},
        {id: 217, name: "Cửa hàng nail", groupUtilityId: this.CUA_HANG, type: this.NGOAI_KHU, icon: "Icon_Cuahangnail"},
        {id: 218, name: "Cửa hàng đồ thể thao", groupUtilityId: this.CUA_HANG, type: this.NGOAI_KHU, icon: "Icon_Cuahangdothethao"},
        {id: 219, name: "Cửa hàng đồ công sở", groupUtilityId: this.CUA_HANG, type: this.NGOAI_KHU, icon: "Icon_Cuahangdocongso"},
        {id: 220, name: "Cửa hàng tiện lợi", groupUtilityId: this.CUA_HANG, type: this.NGOAI_KHU, icon: "Icon_Cuahangtienloi"},
      
        {id: 221, name: "Phố ẩm thực", groupUtilityId: this.KHU_VUC_AN_UONG, type: this.NGOAI_KHU, icon: "Icon_Phoamthuc"},
        {id: 224, name: "Bar sân thượng", groupUtilityId: this.KHU_VUC_AN_UONG, type: this.NGOAI_KHU, icon: "Icon_Bar"},
        {id: 223, name: "Đồ ăn nhanh", groupUtilityId: this.KHU_VUC_AN_UONG, type: this.NGOAI_KHU, icon: "Icon_Doannhanh"},
        {id: 225, name: "Ẩm thực nước ngoài", groupUtilityId: this.KHU_VUC_AN_UONG, type: this.NGOAI_KHU, icon: "Icon_Amthucnuocngoai"},
        {id: 228, name: "Lẩu", groupUtilityId: this.KHU_VUC_AN_UONG, type: this.NGOAI_KHU, icon: "Icon_Lau"},
        {id: 229, name: "Đồ ăn vỉa hè", groupUtilityId: this.KHU_VUC_AN_UONG, type: this.NGOAI_KHU, icon: "Icon_Doanviahe"},
        {id: 230, name: "Đồ uống", groupUtilityId: this.KHU_VUC_AN_UONG, type: this.NGOAI_KHU, icon: "Icon_Douong"},
      
        {id: 231, name: "Khu vệ sinh công cộng", groupUtilityId: this.PHUC_VU_CU_DAN_VA_KHACH, type: this.NGOAI_KHU, icon: "Icon_Khuvesinhcongcong"},
        {id: 232, name: "Đèn chiếu sáng ngoài trời", groupUtilityId: this.PHUC_VU_CU_DAN_VA_KHACH, type: this.NGOAI_KHU, icon: "Icon_Denchieusangngoaitroi"},
        
        {id: 233, name: "Bệnh viện", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NGOAI_KHU, icon: "Icon_Benhvien"},
        {id: 234, name: "Trường mầm non", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NGOAI_KHU, icon: "Icon_Truongmannon"},
        {id: 235, name: "Trường tiểu học", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NGOAI_KHU, icon: "Icon_Truongtieuhoc"},
        {id: 236, name: "Trường THCS", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NGOAI_KHU, icon: "Icon_Truongthcs"},
        {id: 238, name: "Nhà chờ xe bus", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NGOAI_KHU, icon: "Icon_Nhachoxebus"},
        {id: 239, name: "Nhà thờ", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NGOAI_KHU, icon: "Icon_Nhatho"},
        {id: 240, name: "Chùa", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NGOAI_KHU, icon: "Icon_Chua"},
        {id: 241, name: "Cơ quan nhà nước", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NGOAI_KHU, icon: "Icon_Coquannhanuoc"},
        {id: 242, name: "Ga tàu", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NGOAI_KHU, icon: "Icon_Gatau"},
        {id: 244, name: "Trung tâm thương mại", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NGOAI_KHU, icon: "Icon_Trungtamthuongmai"},
        {id: 247, name: "Sân vườn", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NGOAI_KHU, icon: "Icon_Sanvuon"},
        {id: 249, name: "Vườn hoa", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NGOAI_KHU, icon: "Icon_Vuonhoa"},
        {id: 250, name: "Cây xanh vỉa hè", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NGOAI_KHU, icon: "Icon_Cayxanhviahe"},
        
        {id: 254, name: "Xe bus", groupUtilityId: this.PHUONG_TIEN_CONG_CONG, type: this.NGOAI_KHU, icon: "Icon_Xebus"},
        {id: 255, name: "Xe điện di chuyển trong khu", groupUtilityId: this.PHUONG_TIEN_CONG_CONG, type: this.NGOAI_KHU, icon: "Icon_Xediendichuyentrongkhu"},
        {id: 256, name: "Xe đạp công cộng", groupUtilityId: this.PHUONG_TIEN_CONG_CONG, type: this.NGOAI_KHU, icon: "Icon_Xedapcongcong"},

        {id: 258, name: "Thu rác", groupUtilityId: this.VE_SINH, type: this.NGOAI_KHU, icon: "Icon_Thurac"},

        {id: 258, name: "Bảo vệ an ninh", groupUtilityId: this.BAO_VE, type: this.NGOAI_KHU, icon: "Icon_Baoveanninh"},

        {id: 262, name: "Chăm sóc sắc đẹp", groupUtilityId: this.DICH_VU_THUONG_QUY, type: this.NGOAI_KHU, icon: "Icon_Chamsocsacdep"},
        {id: 263, name: "Chăm sóc bảo trì xe máy", groupUtilityId: this.DICH_VU_THUONG_QUY, type: this.NGOAI_KHU, icon: "Icon_Chamsocbaotrixemay"},
        {id: 264, name: "Chăm sóc bảo trì ô tô", groupUtilityId: this.DICH_VU_THUONG_QUY, type: this.NGOAI_KHU, icon: "Icon_Chamsocbaotrioto"},
        {id: 265, name: "Chăm sóc cơ thể", groupUtilityId: this.DICH_VU_THUONG_QUY, type: this.NGOAI_KHU, icon: "Icon_Chamsoccothe"},
        {id: 266, name: "Chăm sóc thú cưng", groupUtilityId: this.DICH_VU_THUONG_QUY, type: this.NGOAI_KHU, icon: "Icon_Chamsocthucung"},
        {id: 267, name: "Chăm sóc người già", groupUtilityId: this.DICH_VU_THUONG_QUY, type: this.NGOAI_KHU, icon: "Icon_Chamsocnguoigia"},
        {id: 268, name: "Chăm sóc người ốm", groupUtilityId: this.DICH_VU_THUONG_QUY, type: this.NGOAI_KHU, icon: "Icon_Chamsocnguoiom"},
        {id: 269, name: "Chăm sóc cây cối", groupUtilityId: this.DICH_VU_THUONG_QUY, type: this.NGOAI_KHU, icon: "Icon_Chamsoccaycoi"},
        {id: 270, name: "Dịch vụ chuyển nhà", groupUtilityId: this.DICH_VU_THUONG_QUY, type: this.NGOAI_KHU, icon: "Icon_Dichvuchuyennha"},
        {id: 271, name: "Sửa chữa nhà cửa", groupUtilityId: this.DICH_VU_THUONG_QUY, type: this.NGOAI_KHU, icon: "Icon_Suachuanhacua"},
        {id: 272, name: "Sửa chữa điện nước", groupUtilityId: this.DICH_VU_THUONG_QUY, type: this.NGOAI_KHU, icon: "Icon_Suachuadiennuoc"},
    
        // Bổ sung Nội khu
        {id: 273, name: "Sân trượt băng", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NOI_KHU, icon: "Icon_Santruotbang"},
        {id: 274, name: "Khu game thự tế ảo", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NOI_KHU, icon: "Icon_Gamethucteao"},
        {id: 275, name: "Karaoke mini", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NOI_KHU, icon: "Icon_Karaokemini"},
        {id: 276, name: "Sân nhảy trong nhà", groupUtilityId: this.THE_THAO, type: this.NOI_KHU, icon: "Icon_Sannhaytrongnha"},
        {id: 277, name: "Bộ dụng cụ thể thao ngoài trời", groupUtilityId: this.THE_THAO, type: this.NOI_KHU, icon: "Icon_Bodungcuthethaongoaitroi"},
        {id: 278, name: "Trường quốc tế", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NOI_KHU, icon: "Icon_Truongquocte"},
        {id: 279, name: "Cây xăng", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NOI_KHU, icon: "Icon_Cayxang"},

        // Bổ sung Ngoại khu
        {id: 280, name: "Hồ tự nhiên", groupUtilityId: this.BIEN_HO, type: this.NGOAI_KHU, icon: "Icon_Hotunhien"},
        {id: 281, name: "Sân nhảy trong nhà", groupUtilityId: this.THE_THAO, type: this.NGOAI_KHU, icon: "Icon_Sannhaytrongnha"},
        {id: 282, name: "Cây xăng", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NGOAI_KHU, icon: "Icon_Cayxang"},
        {id: 283, name: "Tàu điện", groupUtilityId: this.CONG_TRINH_CONG_CONG, type: this.NGOAI_KHU, icon: "Icon_Taudien"},


        // Bổ sung lần 2
        // Nội khu
        {id: 284, name: "Phòng hội nghị", groupUtilityId: this.CAN_HO_DICH_VU, type: this.NOI_KHU, icon: "Icon_Phonghoinghi"},
        {id: 285, name: "Phòng tổ chức sự kiện", groupUtilityId: this.CAN_HO_DICH_VU, type: this.NOI_KHU, icon: "Icon_Phongtochucsukien"},

        {id: 286, name: "Nhà hàng", groupUtilityId: this.KHU_VUC_AN_UONG, type: this.NOI_KHU, icon: "Icon_Nhahang"},

        {id: 287, name: "Casino", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NOI_KHU, icon: "Icon_Casino"},
        {id: 288, name: "Bể sục", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NOI_KHU, icon: "Icon_Besuc"},
        {id: 289, name: "Phòng xông hơi", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NOI_KHU, icon: "Icon_Phongxonghoi"},
        {id: 290, name: "Khu tắm nắng", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NOI_KHU, icon: "Icon_Khutamnang"},
        {id: 291, name: "Khu vui chơi trẻ em", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NOI_KHU, icon: "Icon_Khuvuichoitreem"},

        {id: 292, name: "Bãi đỗ xe", groupUtilityId: this.NOI_DE_XE, type: this.NOI_KHU, icon: "Icon_Baidoxe"},
        // Ngoại khu
        {id: 293, name: "Phòng hội nghị", groupUtilityId: this.CAN_HO_DICH_VU, type: this.NGOAI_KHU, icon: "Icon_Phonghoinghi"},
        {id: 294, name: "Phòng tổ chức sự kiện", groupUtilityId: this.CAN_HO_DICH_VU, type: this.NGOAI_KHU, icon: "Icon_Phongtochucsukien"},

        {id: 295, name: "Nhà hàng", groupUtilityId: this.KHU_VUC_AN_UONG, type: this.NGOAI_KHU, icon: "Icon_Nhahang"},

        {id: 296, name: "Casino", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NGOAI_KHU, icon: "Icon_Casino"},
        {id: 297, name: "Bể sục", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NGOAI_KHU, icon: "Icon_Besuc"},
        {id: 298, name: "Phòng xông hơi", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NGOAI_KHU, icon: "Icon_Phongxonghoi"},
        {id: 299, name: "Khu tắm nắng", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NGOAI_KHU, icon: "Icon_Khutamnang"},
        {id: 300, name: "Khu vui chơi trẻ em", groupUtilityId: this.DIEM_VUI_CHOI, type: this.NGOAI_KHU, icon: "Icon_Khuvuichoitreem"},

        {id: 301, name: "Bãi đỗ xe", groupUtilityId: this.NOI_DE_XE, type: this.NGOAI_KHU, icon: "Icon_Baidoxe"},



        // Tiện ích riêng
        // {id: 284, name: "Bể bơi riêng", groupUtilityId: this.TIEN_ICH_RIENG_DANH_CHO_CAN_HO, type: this.TUNG_CAN_HO, icon: "Icon_Beboirieng"},
        // {id: 285, name: "Phòng tập gym riêng", groupUtilityId: this.TIEN_ICH_RIENG_DANH_CHO_CAN_HO, type: this.TUNG_CAN_HO, icon: "Icon_Phongtapgymrieng"},
        // {id: 286, name: "View rộng, thoáng", groupUtilityId: this.TIEN_ICH_RIENG_DANH_CHO_CAN_HO, type: this.TUNG_CAN_HO, icon: "Icon_Viewrong"},
        // {id: 287, name: "Cầu thang riêng trong nhà", groupUtilityId: this.TIEN_ICH_RIENG_DANH_CHO_CAN_HO, type: this.TUNG_CAN_HO, icon: "Icon_Cauthangriengtrongnha"},
        // {id: 288, name: "Tiểu cảnh riêng", groupUtilityId: this.TIEN_ICH_RIENG_DANH_CHO_CAN_HO, type: this.TUNG_CAN_HO, icon: "Icon_Tieucanhrieng"},
        // {id: 289, name: "Không gian sang trọng", groupUtilityId: this.TIEN_ICH_RIENG_DANH_CHO_CAN_HO, type: this.TUNG_CAN_HO, icon: "Icon_Khonggiansangtrong"},
        // {id: 290, name: "Thang máy riêng", groupUtilityId: this.TIEN_ICH_RIENG_DANH_CHO_CAN_HO, type: this.TUNG_CAN_HO, icon: "Icon_Thangmayrieng"},
        // {id: 291, name: "Quầy bar trong nhà", groupUtilityId: this.TIEN_ICH_RIENG_DANH_CHO_CAN_HO, type: this.TUNG_CAN_HO, icon: "Icon_Quaybarrieng"},
        // {id: 292, name: "Vị trí đẹp của tòa nhà", groupUtilityId: this.TIEN_ICH_RIENG_DANH_CHO_CAN_HO, type: this.TUNG_CAN_HO, icon: "Icon_Vitridep"},
        // {id: 293, name: "Diện tích rộng", groupUtilityId: this.TIEN_ICH_RIENG_DANH_CHO_CAN_HO, type: this.TUNG_CAN_HO, icon: "Icon_Dientichrong"},
        // {id: 294, name: "Yên tĩnh", groupUtilityId: this.TIEN_ICH_RIENG_DANH_CHO_CAN_HO, type: this.TUNG_CAN_HO, icon: "Icon_Yentinh"},
    ];

    public static getIcon(filter = {keyword: '', groupUtilityId: [], type: []} ){
        let listResult= [];
        if (filter.keyword == '' && filter.groupUtilityId?.length == 0 && filter?.type.length == 0){
            listResult = this.List;
        } 
        else {
            this.List.forEach( (item) => {
                if ( (item.name.toLowerCase().includes(filter?.keyword.toLowerCase()) || filter?.keyword == '') 
                && (filter?.groupUtilityId?.includes(item.groupUtilityId) || filter.groupUtilityId?.length == 0) 
                && (filter?.type?.includes(item.type) || filter?.type?.length == 0)) {
                    listResult.push(item);
                }
            });
         
        }

        listResult = listResult.map( (item) => {
            item.path = `assets/layout/images/icon/${item.icon}.svg`;
            return item;
        });

        return listResult;
    }

}