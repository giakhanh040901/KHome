# Quy trình sinh file

1. Lấy tất cả mẫu hợp đồng Đặt lệnh ở trạng thái active trong chính sách hiện tại
2. Fill data vào hợp đồng
    - Sinh 3 file, 1 file word, 1 file pdf không có con dấu, 1 file pdf có con dấu
3. Cập nhật hồ sơ
    - Lấy danh sách các hợp đồng đang ở trạng thái active
    - Nếu có hợp đồng trong db, thì sẽ xóa bản ghi cũ và cả file vật lý cũ đi, nếu chưa có sẽ thêm mới (Bỏ qua hợp đồng tái tục) và xóa file tái tục (Vì sinh file xong mới lưu đường dẫn)
    - Tên file: lấy tên file ~ContractDataUtils.GenerateNewFileName(fileName);~
        + Hàm này lấy tên filename là tên file lúc upload lên + với chuỗi thời gian (FileTime)
4. Ký điện tử
    - Lấy file có con dấu (đuôi -Sign.Pdf) đi ký điện tử