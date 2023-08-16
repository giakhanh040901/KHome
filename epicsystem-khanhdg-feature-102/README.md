# EPIC.System

## Run project
1. Docker
    - `cd src`
    - `docker-compose up -d --build`
2. Chạy môi trường local
    1. Dùng dòng lệnh
    - Vào từng project có file `Program.cs` bằng cmd hoặc wt
    - Chạy lệnh `dotnet run Program.cs`
    2. Dùng visual studio
    - Chạy multiple project

## Commit
1. Tổng quan
    - Tách riêng các commit fix bug và commit tính năng mới
    - Nếu cần `pull` code mới về thì sử dụng lệnh `git stash` để cất code đi sau khi pull code từ _branch develop_ về thì dùng lệnh `git stash apply` để đưa code đang làm dở ra lại không mỗi lần `pull code` lại `commit` một lần tạo rất nhiều `commit` thừa
        - Với lệnh `git stash apply` code thể dùng trong giao diện của visual studio
        - Với trường hợp cần bỏ code đã cất đi dùng lênh `git stash drop` lưu ý cẩn thận khi dùng lệnh này sẽ xoá code đã cất đi _**Không thể khôi phục**_
    - Không tự ý merge code phải tạo `merge request` để check lại
    - `message commit` viết có ý nghĩa theo convetion sau:
        - Bug: `[Bug] nguồn bug từ đâu - nội dung bug được fix`
        - Tính năng mới: `Mô tả tính năng`

## Comment code
1. Tổng quan
    - comment càng chi tiết càng tốt
    - các method giải thích đầu vào đầu ra, logic phức tạp
2. Repositories
    - comment tại tất cả các method trong repo giải thích đầu vào đầu ra xử lý bên trong nếu join nhiều bảng
3. Package trong db
    - các procedure mô tả dùng để làm gì càng chi tiết càng tốt, các tham số truyền vào xử lý như nào nếu truyền thì làm gì nếu truyền null thì làm gì (vd: trong trường hợp một số procedure truyền trading_provider_id = số hoặc = null)

## Coding convetion C#
1. Tổng quan
    - Tuân theo convetion **PascalCase** tức viết hoa chữ cái đầu tiên
    - Các biến sẽ viết theo convetion **CamelCase** viết thường chữ cái đầu
2. Dtos
    - Đặt tên Dto nếu là model tương tự với Entity (_các trường dữ liệu giống hệt_) thì đặt tên theo dạng `EntityDto` nếu là một class mở rộng thêm các trường thì đặt tên theo dạng `EntityWith` `GiDo` `Dto`
    - Không dùng chung 1 class Dto ở 2 hàm mà không cùng trả ra các trường giống nhau 
    - Các class Dto thêm mới từ giờ không thêm tiếp vào project `EPIC.Entities` mà phải để ở các project `Entities` của chính microservice đó, tương lại các Dto đặt không đúng microservice sẽ phải được chuyển về đúng chỗ.
3. Services
    - Viết trong project Domain của từng _microservice_
4. Repositories
    - Các method `GetById` thực sự chỉ đơn giản là `GetById` không xử lý thêm các trường bên ngoài entity nếu có đặt tên hàm khác, không `throw Exception` tại các method `GetById` nếu không tìm thấy chỉ trả ra `null`
    - Mỗi một bảng 1 repository trường hợp đã xử lý nhiều bảng theo nghiệp vụ thì comment summary trên class repository là tương tác đến những bảng nào theo dạng `<summary>` `Xử lý bảng:` `EP_ABC, EP_DEF` `</summary>`
5. Cách viết log
    - log information cho các tham số truyền vào trong hàm dạng như sau:
    ```cs
        public PagingResult<ResultDto> MethodName(InputDto input, int? idOrther = null)
        {
            _logger.LogInformation($"{nameof(MethodName)}: input = {JsonSerializer.Serialize(input)}, idOrther = {idOrther}");
        }
    ```
6. Cách mô tả swagger
    - Chèn thêm attribute `ProducesResponseType` theo mẫu như bên dưới
    - Thêm các comment summery vào các trường trong class Dto trả ra
    ```cs
        [HttpGet("find")]
        [ProducesResponseType(typeof(APIResponse<List<AppGarnerDistributionDto>>), (int)HttpStatusCode.OK)]
        public APIResponse AppDistributionGetAll(string keyword) 
        {
            //nội dung api
        }
    ```
    - Config swagger như sau để sinh comment trên swagger
    ```cs
        // Set the comments path for the Swagger JSON and UI.**
        option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
        option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"EPIC.GarnerEntities.xml"));
    ```
7. SignalR
    - test hub: https://gourav-d.github.io/SignalR-Web-Client/dist/
## Coding convetion DB
1. Tổng quan
    - Đặt tên các bảng theo đúng ý nghĩa và liên quan đến mảng nghiệp vụ nào
2. Error Code
    - Số mã lỗi đặt tương ứng với nghiệp vụ đang xử lý vd: bond 3xxx không đặt nhảy cách số phải liên tiếp. các mã lỗi nếu cần xử lý trong C# thì viết tiếp vào class ErrorCodes theo đúng thứ tự tăng dần không đặt linh tinh.
3. Migrations
    - Tạo migrations trong project HostConsole
    ```
        dotnet ef migrations add <MigrationName>
    ```
    - Dùng lệnh sau để tạo script thay vì chạy database update
    ```
        dotnet ef migrations script -o Scripts/changedb.sql <MigrationTruoc> <MigrationHienTai>
    ```
    - Scaffold db
    ```
        dotnet ef dbcontext scaffold Name=ConnectionStrings:EPIC Oracle.EntityFrameworkCore -o Models -f
    ```
    - Lỗi OracleException: ORA-01950: no privileges on tablespace 'USERS' -> gõ lệnh sau vào script trong db:
        GRANT UNLIMITED TABLESPACE TO <Schema Name>;
## Test
    ```console
    dotnet test EPIC.InvestService/EPIC.InvestUnitTest
    ```
## Tìm code
1. regex
    `_logger\.LogError\(.+\,\s{0,1}\$.+\;`
    
## Coding convetion Angular
1. Tổng quan
    - Tạo các component hợp lý không copy lặp lại code
