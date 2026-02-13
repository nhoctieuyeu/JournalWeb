USE [master]
GO
CREATE DATABASE [JournalWeb]
GO
USE [JournalWeb]
GO

-- =============================================
-- 1. CÁC BẢNG HỆ THỐNG GỐC (GIỮ NGUYÊN & ĐỔI TÊN KHÔNG DẤU)
-- =============================================

CREATE TABLE NguoiDung (
    NguoiDungId INT PRIMARY KEY IDENTITY(1,1),
    TaiKhoan VARCHAR(50) NOT NULL UNIQUE,
    MatKhau VARCHAR(255) NOT NULL,
    HoTen NVARCHAR(100),
    Email VARCHAR(100),
    DienThoai VARCHAR(20),
    IsActive BIT DEFAULT 1,
    NgayTao DATETIME DEFAULT GETDATE()
);

CREATE TABLE PhanQuyen (
    QuyenId INT PRIMARY KEY IDENTITY(1,1),
    TenQuyen NVARCHAR(50)
);

CREATE TABLE Menu (
    MenuId INT PRIMARY KEY IDENTITY(1,1),
    TenMenu NVARCHAR(100),
    LienKet VARCHAR(255),
    ThuTu INT,
    MenuChaId INT
);

CREATE TABLE Slide (
    SlideId INT PRIMARY KEY IDENTITY(1,1),
    HinhAnh VARCHAR(255),
    TieuDe NVARCHAR(255),
    LienKet VARCHAR(255),
    ThuTu INT
);

CREATE TABLE TinTuc (
    TinTucId INT PRIMARY KEY IDENTITY(1,1),
    TieuDe NVARCHAR(255),
    MoTaNgan NVARCHAR(500),
    NoiDung NVARCHAR(MAX),
    HinhAnh VARCHAR(255),
    NgayDang DATETIME DEFAULT GETDATE()
);

CREATE TABLE AuditLog (
    LogId INT PRIMARY KEY IDENTITY(1,1),
    NoiDung NVARCHAR(MAX),
    ThoiGian DATETIME DEFAULT GETDATE(),
    NguoiDungId INT
);

-- =============================================
-- 2. HỆ THỐNG CẢM XÚC 3D & NHẬT KÝ (CẬP NHẬT MỚI)
-- =============================================

-- 7 Mức độ chính (Như video/hình ảnh)
CREATE TABLE MucDoCamXuc (
    MucDoId INT PRIMARY KEY, -- 1 đến 7
    TenMucDo NVARCHAR(50), 
    MauNenGradient VARCHAR(255), -- Để đổi màu Web theo cảm xúc
    HieuUngCSS NVARCHAR(MAX)      -- Thông số đổ bóng cho hoa 3D
);

-- Các từ mô tả cảm xúc chi tiết (Page 1, 3, 5, 7 trong PDF)
CREATE TABLE CamXucChiTiet (
    CamXucId INT PRIMARY KEY IDENTITY(1,1),
    MucDoId INT,
    TenCamXuc NVARCHAR(50), -- Tức giận, Lo âu, Hạnh phúc...
    FOREIGN KEY (MucDoId) REFERENCES MucDoCamXuc(MucDoId)
);

-- Danh mục tác động (Page 2, 4, 6, 8 trong PDF)
CREATE TABLE DanhMucTacDong (
    DanhMucId INT PRIMARY KEY IDENTITY(1,1),
    TenDanhMuc NVARCHAR(100), -- Gia đình, Công việc, Sức khỏe...
    IconSVG TEXT,             -- Mã vẽ Icon bằng code cho sắc nét
    NhomDanhMuc NVARCHAR(50)  -- Sức khỏe, Mối quan hệ...
);

CREATE TABLE NhatKy (
    NhatKyId INT PRIMARY KEY IDENTITY(1,1),
    NguoiDungId INT,
    MucDoId INT,             -- Chọn 1 trong 7 mức chính
    TieuDe NVARCHAR(255),
    NoiDung NVARCHAR(MAX),
    NgayTao DATETIME DEFAULT GETDATE(),
    IsRiengTu BIT DEFAULT 1,
    FOREIGN KEY (NguoiDungId) REFERENCES NguoiDung(NguoiDungId),
    FOREIGN KEY (MucDoId) REFERENCES MucDoCamXuc(MucDoId)
);

-- Lưu nhiều cảm xúc chi tiết cho 1 bài viết
CREATE TABLE NhatKy_CamXuc (
    NhatKyId INT,
    CamXucId INT,
    PRIMARY KEY (NhatKyId, CamXucId),
    FOREIGN KEY (NhatKyId) REFERENCES NhatKy(NhatKyId),
    FOREIGN KEY (CamXucId) REFERENCES CamXucChiTiet(CamXucId)
);

-- Lưu nhiều tác động cho 1 bài viết
CREATE TABLE NhatKy_DanhMuc (
    NhatKyId INT,
    DanhMucId INT,
    PRIMARY KEY (NhatKyId, DanhMucId),
    FOREIGN KEY (NhatKyId) REFERENCES NhatKy(NhatKyId),
    FOREIGN KEY (DanhMucId) REFERENCES DanhMucTacDong(DanhMucId)
);

CREATE TABLE NhatKyMedia (
    MediaId INT PRIMARY KEY IDENTITY(1,1),
    NhatKyId INT,
    DuongDanFile VARCHAR(255),
    LoaiMedia VARCHAR(20), -- 'image' hoặc 'video'
    ThoiLuong VARCHAR(10),
    NgayTao DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (NhatKyId) REFERENCES NhatKy(NhatKyId)
);
GO

-- =============================================
-- 3. NẠP DỮ LIỆU MẪU ĐẦY ĐỦ (MOOD & IMPACTS)
-- =============================================

-- Nạp 7 mức chính
INSERT INTO MucDoCamXuc VALUES 
(1, N'Rất khó chịu', 'linear-gradient(180deg, #424242, #212121)', 'box-shadow: 0 10px 20px #000'),
(4, N'Bình thường', 'linear-gradient(180deg, #A5D6A7, #66BB6A)', 'box-shadow: 0 10px 20px #2E7D32'),
(7, N'Rất dễ chịu', 'linear-gradient(180deg, #EF9A9A, #E53935)', 'box-shadow: 0 10px 20px #C62828');

-- Nạp cảm xúc chi tiết (Ví dụ tiêu biểu)
INSERT INTO CamXucChiTiet (MucDoId, TenCamXuc) VALUES 
(1, N'Tức giận'), (1, N'Kiệt sức'), (1, N'Buồn'),
(7, N'Hạnh phúc'), (7, N'Biết ơn'), (7, N'Phấn khích');

-- Nạp danh mục tác động (Impact Factors)
INSERT INTO DanhMucTacDong (TenDanhMuc, NhomDanhMuc) VALUES 
(N'Sức khỏe', N'Cá nhân'), (N'Gia đình', N'Mối quan hệ'), (N'Công việc', N'Xã hội');