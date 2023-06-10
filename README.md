# GIỚI THIỆU ĐỒ ÁN 

"Xây dựng Website bán thiết bị công nghệ Apple" áp dụng các kiến thức đã học trong môn Xây dựng hệ thống thông tin trên các Framework, các kiến thức liên quan về nền tảng ASP.NET và các môn học như Phân tích thiết kế hệ thống thông tin, Quản lý dự án. Bên cạnh đó, nhóm tiến hành khảo sát các website tương tự như thegioididong, cellphones,... để xây dựng một Website với các đẩy đủ các chức năng cơ bản cho người dùng.

## CÁC CHỨC NĂNG ĐÃ LÀM

- Chung:

    * Đăng ký, đăng nhập, đổi mật khẩu
    * Xem thông tin sản phẩm

- Chức năng của Admin:

    * Quản lý sản phẩm: thêm sản phẩm, xem chi tiết, cập nhật các thông tin về sản phẩm (số lượng, tình trạng, mô tả,...)
    * Quản lý đơn đặt hàng: xem chi tiết đơn đặt hàng, cập nhật đơn hàng (trạng thái, ngày giao,...)
    * Quản lý khuyến mãi: thêm, cập nhật tình trạng khuyến mãi
    * Quản lý quyền truy cập: cập nhật trạng thái tài khoản
    * Quản lý bài viết: thêm, xóa, sửa bài viết

- Chức năng của khách hàng:

    * Tra cứu sản phẩm theo từ khóa, bộ lọc
    * Thêm sản phẩm vào giỏ hàng
    * Thêm sản phẩm vào yêu thích
    * Đặt hàng
    * Xem tin tức
    * Bình luận
    * Đánh giá

## GIỚI THIỆU CÁC THÀNH VIÊN TRONG NHÓM

| HỌ TÊN | MSSV | NHIỆM VỤ | HOÀN THÀNH |
| ------ | ---- | -------- | ---------- |
| Võ Phạm Thùy Nhung (Nhóm trưởng)| 20520679 | Code chức năng Admin, xây dựng database và quản lý dự án | 100% |
| Trần Thu Thảo| 20520769 | Xây dựng các giao diện phần mềm | 100% |
| Võ Thị Hà Trang | 20522043 | Hỗ trợ xây dựng database và hỗ trợ code một số chức năng khách hàng | 100% |
| Nguyễn Đức Phúc | 17520906 | Code chính các chức năng khách hàng | 100% |
| Võ Nữ Diễm Trang | 20521013 | Phân tích thiết kế các hoạt động cho ứng dụng, viết báo cáo | 100% |

## Các package sử dụng

- AspNetCoreHero.ToastNotification 
- Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.VisualStudio.Web.CodeGeneration.Design
- PagedList.Core.Mvc
   
## HƯỚNG DẪN CÀI ĐẶT

- Cài đặt Sql Server:
    * https://www.microsoft.com/en-us/sql-server/sql-server-downloads

- Cài đặt Visual Studio:
    * https://visualstudio.microsoft.com/downloads/ 

## CÁC BƯỚC CHẠY PHẦN MỀM
B1: Clone project về máy, mở Visual Studio lên và chạy trực tiếp, có chia các View và mô hình 3 lớp

B2: Mở Sql Server chạy và tiến hành chạy file database "db.sql"

B3: Thay đổi tên Server của máy bạn trong file "appsettings.json" 

B4: Chạy project với các tài khoản và mật khẩu sau:
    
    Admin: 
    thuynhung
    123456
    
    Khách hàng:
    banvantrang665
    123456
    
