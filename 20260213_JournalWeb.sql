-- ============================================
-- TẠO DATABASE JournalWeb (FIXED)
-- ============================================
USE master
GO

IF DB_ID('JournalWeb') IS NOT NULL
    DROP DATABASE JournalWeb
GO

CREATE DATABASE JournalWeb
GO

USE JournalWeb
GO

-- ============================================
-- BẢNG PHÂN QUYỀN
-- ============================================
CREATE TABLE PhanQuyen (
    QuyenId INT PRIMARY KEY IDENTITY(1,1),
    TenQuyen NVARCHAR(50) NOT NULL
)
GO

INSERT INTO PhanQuyen (TenQuyen) VALUES (N'Admin'), (N'User')
GO

-- ============================================
-- BẢNG NGƯỜI DÙNG
-- ============================================
CREATE TABLE NguoiDung (
    NguoiDungId INT PRIMARY KEY IDENTITY(1,1),
    TaiKhoan VARCHAR(50) NOT NULL UNIQUE,
    MatKhauHash VARCHAR(255) NOT NULL,
    PinHash VARCHAR(255) NULL,
    HoTen NVARCHAR(100) NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    DienThoai VARCHAR(20) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    NgayTao DATETIME NOT NULL DEFAULT GETDATE(),
    QuyenId INT NOT NULL DEFAULT 2,
    FOREIGN KEY (QuyenId) REFERENCES PhanQuyen(QuyenId)
)
GO

-- ============================================
-- BẢNG MENU (dùng sau)
-- ============================================
CREATE TABLE Menu (
    MenuId INT PRIMARY KEY IDENTITY(1,1),
    TenMenu NVARCHAR(100),
    LienKet VARCHAR(255),
    ThuTu INT,
    MenuChaId INT,
    FOREIGN KEY (MenuChaId) REFERENCES Menu(MenuId)
)
GO

-- ============================================
-- BẢNG TIN TỨC (dùng sau)
-- ============================================
CREATE TABLE TinTuc (
    TinTucId INT PRIMARY KEY IDENTITY(1,1),
    TieuDe NVARCHAR(255),
    MoTaNgan NVARCHAR(500),
    NoiDung NVARCHAR(MAX),
    HinhAnh VARCHAR(255),
    NgayDang DATETIME DEFAULT GETDATE()
)
GO

-- ============================================
-- BẢNG SLIDE (dùng sau)
-- ============================================
CREATE TABLE Slide (
    SlideId INT PRIMARY KEY IDENTITY(1,1),
    HinhAnh VARCHAR(255),
    TieuDe NVARCHAR(200),
    LienKet VARCHAR(255),
    ThuTu INT
)
GO

-- ============================================
-- BẢNG AUDIT LOG (dùng sau)
-- ============================================
CREATE TABLE AuditLog (
    LogId INT PRIMARY KEY IDENTITY(1,1),
    NoiDung NVARCHAR(MAX),
    ThoiGian DATETIME DEFAULT GETDATE(),
    NguoiDungId INT,
    FOREIGN KEY (NguoiDungId) REFERENCES NguoiDung(NguoiDungId) ON DELETE CASCADE
)
GO

-- ============================================
-- BẢNG CẢM XÚC (7 mức độ)
-- ============================================
CREATE TABLE CamXuc (
    CamXucId INT PRIMARY KEY IDENTITY(1,1),
    TenCamXuc NVARCHAR(50) NOT NULL,
    MaMauGradient NVARCHAR(255) NOT NULL,
    DuongDanIcon NVARCHAR(255) NULL,
    NhomCamXuc NVARCHAR(50) NULL
)
GO

INSERT INTO CamXuc (TenCamXuc, MaMauGradient, DuongDanIcon, NhomCamXuc)
VALUES
    (N'Rất khó chịu', 'linear-gradient(180deg, #4A4A4A 0%, #2B2B2B 100%)', '/images/moods/lv1.png', N'Muc 1'),
    (N'Khó chịu',    'linear-gradient(180deg, #7E57C2 0%, #512DA8 100%)', '/images/moods/lv2.png', N'Muc 2'),
    (N'Hơi khó chịu', 'linear-gradient(180deg, #90CAF9 0%, #42A5F5 100%)', '/images/moods/lv3.png', N'Muc 3'),
    (N'Bình thường',  'linear-gradient(180deg, #A5D6A7 0%, #66BB6A 100%)', '/images/moods/lv4.png', N'Muc 4'),
    (N'Hơi dễ chịu',  'linear-gradient(180deg, #FFF59D 0%, #FBC02D 100%)', '/images/moods/lv5.png', N'Muc 5'),
    (N'Dễ chịu',      'linear-gradient(180deg, #FFCC80 0%, #F57C00 100%)', '/images/moods/lv6.png', N'Muc 6'),
    (N'Rất dễ chịu',  'linear-gradient(180deg, #EF9A9A 0%, #E53935 100%)', '/images/moods/lv7.png', N'Muc 7')
GO

-- ============================================
-- BẢNG CHI TIẾT CẢM XÚC (chip mô tả)
-- ============================================
CREATE TABLE CamXucChiTiet (
    ChiTietId INT PRIMARY KEY IDENTITY(1,1),
    MucDoId INT NOT NULL,
    TenChiTiet NVARCHAR(100) NOT NULL,
    ThuTu INT NOT NULL DEFAULT 0,
    FOREIGN KEY (MucDoId) REFERENCES CamXuc(CamXucId) ON DELETE CASCADE
)
GO

-- Dữ liệu chip cảm xúc (giữ nguyên)
INSERT INTO CamXucChiTiet (MucDoId, TenChiTiet, ThuTu) VALUES
(1, N'Tức giận', 1), (1, N'Lo lắng', 2), (1, N'Sợ hãi', 3), (1, N'Ngợp', 4),
(1, N'Xấu hổ', 5), (1, N'Chán ghét', 6), (1, N'Bối rối', 7), (1, N'Nản lòng', 8),
(1, N'Khó chịu', 9), (1, N'Ghen tị', 10), (1, N'Căng thẳng', 11), (1, N'Lo âu', 12),
(1, N'Tội lỗi', 13), (1, N'Ngạc nhiên', 14), (1, N'Vô vọng', 15), (1, N'Bực tức', 16),
(1, N'Cô đơn', 17), (1, N'Chán nản', 18), (1, N'Thất vọng', 19), (1, N'Kiệt sức', 20),
(1, N'Buồn', 21)
GO

INSERT INTO CamXucChiTiet (MucDoId, TenChiTiet, ThuTu) VALUES
(2, N'Bực bội', 1), (2, N'Căng thẳng', 2), (2, N'Mệt mỏi', 3), (2, N'Khó chịu', 4),
(2, N'Lúng túng', 5), (2, N'Buồn chán', 6), (2, N'Thiếu động lực', 7), (2, N'Áp lực', 8)
GO

INSERT INTO CamXucChiTiet (MucDoId, TenChiTiet, ThuTu) VALUES
(3, N'Hơi mệt', 1), (3, N'Lăn tăn', 2), (3, N'Không tập trung', 3),
(3, N'Thiếu năng lượng', 4), (3, N'Lo nhẹ', 5), (3, N'Không thoải mái', 6)
GO

INSERT INTO CamXucChiTiet (MucDoId, TenChiTiet, ThuTu) VALUES
(4, N'Hài lòng', 1), (4, N'Bình tĩnh', 2), (4, N'Bình yên', 3),
(4, N'Lãnh đạm', 4), (4, N'Kiệt sức', 5)
GO

INSERT INTO CamXucChiTiet (MucDoId, TenChiTiet, ThuTu) VALUES
(5, N'Nhẹ nhõm', 1), (5, N'Thoải mái', 2), (5, N'Dễ chịu', 3),
(5, N'Khá vui', 4), (5, N'Ổn định', 5), (5, N'Có hy vọng', 6)
GO

INSERT INTO CamXucChiTiet (MucDoId, TenChiTiet, ThuTu) VALUES
(6, N'Vui', 1), (6, N'Lạc quan', 2), (6, N'Hài lòng', 3), (6, N'Tự tin', 4),
(6, N'Yêu đời', 5), (6, N'Tích cực', 6), (6, N'Thoả mãn', 7)
GO

INSERT INTO CamXucChiTiet (MucDoId, TenChiTiet, ThuTu) VALUES
(7, N'Kinh ngạc', 1), (7, N'Phấn khích', 2), (7, N'Ngạc nhiên', 3), (7, N'Sôi nổi', 4),
(7, N'Hạnh phúc', 5), (7, N'Vui vẻ', 6), (7, N'Dũng cảm', 7), (7, N'Tự hào', 8),
(7, N'Tự tin', 9), (7, N'Hy vọng', 10), (7, N'Thích thú', 11), (7, N'Thoả mãn', 12),
(7, N'Nhẹ nhõm', 13), (7, N'Biết ơn', 14), (7, N'Hài lòng', 15), (7, N'Bình tĩnh', 16),
(7, N'Bình yên', 17)
GO

-- ============================================
-- BẢNG DANH MỤC TÁC ĐỘNG
-- ============================================
CREATE TABLE DanhMuc (
    DanhMucId INT PRIMARY KEY IDENTITY(1,1),
    TenDanhMuc NVARCHAR(100) NOT NULL,
    DuongDanIcon NVARCHAR(255) NULL,
    NhomDanhMuc NVARCHAR(50) NULL
)
GO

INSERT INTO DanhMuc (TenDanhMuc, DuongDanIcon, NhomDanhMuc) VALUES
(N'Sức khỏe', '/images/icons/health.png', NULL),
(N'Thể dục', '/images/icons/exercise.png', NULL),
(N'Chăm sóc bản thân', '/images/icons/selfcare.png', NULL),
(N'Sở thích', '/images/icons/hobby.png', NULL),
(N'Danh tính', '/images/icons/identity.png', NULL),
(N'Tâm linh', '/images/icons/spiritual.png', NULL),
(N'Cộng đồng', '/images/icons/community.png', NULL),
(N'Gia đình', '/images/icons/family.png', NULL),
(N'Bạn bè', '/images/icons/friends.png', NULL),
(N'Bạn đời', '/images/icons/partner.png', NULL),
(N'Hẹn hò', '/images/icons/dating.png', NULL),
(N'Nhiệm vụ', '/images/icons/tasks.png', NULL),
(N'Công việc', '/images/icons/work.png', NULL),
(N'Giáo dục', '/images/icons/education.png', NULL),
(N'Du lịch', '/images/icons/travel.png', NULL),
(N'Thời tiết', '/images/icons/weather.png', NULL),
(N'Sự kiện hiện tại', '/images/icons/news.png', NULL),
(N'Tiền', '/images/icons/money.png', NULL)
GO

-- ============================================
-- BẢNG NHẬT KÝ
-- ============================================
CREATE TABLE NhatKy (
    NhatKyId INT PRIMARY KEY IDENTITY(1,1),
    NguoiDungId INT NOT NULL,
    MucDoId INT NOT NULL,
    TieuDe NVARCHAR(255) NULL,
    NoiDung NVARCHAR(MAX) NOT NULL,
    NgayViet DATE NOT NULL,
    NgayTao DATETIME NOT NULL DEFAULT GETDATE(),
    IsRiengTu BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (NguoiDungId) REFERENCES NguoiDung(NguoiDungId) ON DELETE CASCADE,
    FOREIGN KEY (MucDoId) REFERENCES CamXuc(CamXucId) ON DELETE CASCADE
)
GO

-- ============================================
-- BẢNG NHẬT KÝ - CHI TIẾT CẢM XÚC
-- (FIX: chỉ ON DELETE CASCADE trên NhatKyId)
-- ============================================
CREATE TABLE NhatKy_CamXucChiTiet (
    NhatKyId INT NOT NULL,
    ChiTietId INT NOT NULL,
    PRIMARY KEY (NhatKyId, ChiTietId),
    FOREIGN KEY (NhatKyId) REFERENCES NhatKy(NhatKyId) ON DELETE CASCADE,
    FOREIGN KEY (ChiTietId) REFERENCES CamXucChiTiet(ChiTietId) ON DELETE NO ACTION
)
GO

-- ============================================
-- BẢNG NHẬT KÝ - DANH MỤC
-- (FIX: chỉ ON DELETE CASCADE trên NhatKyId)
-- ============================================
CREATE TABLE NhatKy_DanhMuc (
    NhatKyId INT NOT NULL,
    DanhMucId INT NOT NULL,
    PRIMARY KEY (NhatKyId, DanhMucId),
    FOREIGN KEY (NhatKyId) REFERENCES NhatKy(NhatKyId) ON DELETE CASCADE,
    FOREIGN KEY (DanhMucId) REFERENCES DanhMuc(DanhMucId) ON DELETE NO ACTION
)
GO

-- ============================================
-- BẢNG MEDIA của nhật ký
-- ============================================
CREATE TABLE NhatKyMedia (
    MediaId INT PRIMARY KEY IDENTITY(1,1),
    NhatKyId INT NOT NULL,
    DuongDanFile VARCHAR(255) NOT NULL,
    LoaiMedia VARCHAR(20) NOT NULL,
    ThoiLuong VARCHAR(10) NULL,
    NgayTao DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (NhatKyId) REFERENCES NhatKy(NhatKyId) ON DELETE CASCADE
)
GO

-- ============================================
-- TÀI KHOẢN ADMIN MẪU
-- (hash của 'manman@2025' - SHA256)
-- ============================================
INSERT INTO NguoiDung (TaiKhoan, MatKhauHash, HoTen, Email, IsActive, QuyenId)
VALUES ('admin', '/4KDVyQC90xTUN5zoTF9a5+h0rIP5cyTCePTe+GfKQU=', N'Quản trị hệ thống', 'admin@gmail.com', 1, 1)
GO

--UPDATE NguoiDung
--SET PinHash = 'A6xnQhbz4Vx2HuGl4lXwZ5U2I8iziLRFnhP5eNfIRvQ='  -- hash của '1234'
--WHERE TaiKhoan = 'admin'

--UPDATE NguoiDung
--SET MatKhauHash = '/4KDVyQC90xTUN5zoTF9a5+h0rIP5cyTCePTe+GfKQU='
--WHERE TaiKhoan = 'admin'

-- ============================================
-- INDEX
-- ============================================
CREATE INDEX IX_NhatKy_NguoiDungId ON NhatKy(NguoiDungId);
CREATE INDEX IX_NhatKy_NgayViet ON NhatKy(NgayViet DESC);
CREATE INDEX IX_NhatKy_MucDoId ON NhatKy(MucDoId);
CREATE INDEX IX_CamXucChiTiet_MucDoId ON CamXucChiTiet(MucDoId);
GO

PRINT N'Tạo cơ sở dữ liệu JournalWeb hoàn tất!'
GO