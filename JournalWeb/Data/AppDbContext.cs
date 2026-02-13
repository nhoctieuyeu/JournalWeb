using Microsoft.EntityFrameworkCore;
using JournalWeb.Models;

namespace JournalWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSet chính
        public DbSet<NguoiDung> NguoiDung { get; set; }
        public DbSet<PhanQuyen> PhanQuyen { get; set; }
        public DbSet<CamXuc> CamXuc { get; set; }
        public DbSet<CamXucChiTiet> CamXucChiTiet { get; set; }
        public DbSet<DanhMuc> DanhMuc { get; set; }
        public DbSet<NhatKy> NhatKy { get; set; }
        public DbSet<NhatKy_CamXucChiTiet> NhatKy_CamXucChiTiet { get; set; }
        public DbSet<NhatKy_DanhMuc> NhatKy_DanhMuc { get; set; }
        public DbSet<NhatKyMedia> NhatKyMedia { get; set; }

        // Các bảng phụ (giữ nguyên)
        public DbSet<Menu> Menu { get; set; }
        public DbSet<Slide> Slide { get; set; }
        public DbSet<TinTuc> TinTuc { get; set; }
        public DbSet<AuditLog> AuditLog { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<NhatKy_Tag> NhatKy_Tag { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // === CẤU HÌNH NguoiDung ===
            modelBuilder.Entity<NguoiDung>()
                .HasIndex(x => x.TaiKhoan)
                .IsUnique();

            modelBuilder.Entity<NguoiDung>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<NguoiDung>()
                .HasOne(x => x.PhanQuyen)
                .WithMany(x => x.NguoiDungs)
                .HasForeignKey(x => x.QuyenId)
                .OnDelete(DeleteBehavior.Restrict);

            // === CẤU HÌNH NhatKy ===
            modelBuilder.Entity<NhatKy>()
                .HasOne(x => x.NguoiDung)
                .WithMany(x => x.NhatKys)
                .HasForeignKey(x => x.NguoiDungId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NhatKy>()
                .HasOne(x => x.CamXuc)
                .WithMany(x => x.NhatKys)
                .HasForeignKey(x => x.MucDoId)
                .OnDelete(DeleteBehavior.Cascade);

            // === CẤU HÌNH NhatKyMedia ===
            modelBuilder.Entity<NhatKyMedia>()
                .HasOne(x => x.NhatKy)
                .WithMany(x => x.Medias)
                .HasForeignKey(x => x.NhatKyId)
                .OnDelete(DeleteBehavior.Cascade);

            // === CẤU HÌNH CamXucChiTiet ===
            modelBuilder.Entity<CamXucChiTiet>()
                .HasOne(x => x.CamXuc)
                .WithMany(x => x.CamXucChiTiets)
                .HasForeignKey(x => x.MucDoId)
                .OnDelete(DeleteBehavior.Cascade);

            // === CẤU HÌNH NhatKy_CamXucChiTiet (quan hệ nhiều-nhiều) ===
            modelBuilder.Entity<NhatKy_CamXucChiTiet>()
                .HasKey(x => new { x.NhatKyId, x.ChiTietId });

            modelBuilder.Entity<NhatKy_CamXucChiTiet>()
                .HasOne(x => x.NhatKy)
                .WithMany(x => x.NhatKy_CamXucChiTiets)
                .HasForeignKey(x => x.NhatKyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NhatKy_CamXucChiTiet>()
                .HasOne(x => x.CamXucChiTiet)
                .WithMany(x => x.NhatKy_CamXucChiTiets)
                .HasForeignKey(x => x.ChiTietId)
                .OnDelete(DeleteBehavior.NoAction);  // Tránh multiple cascade paths

            // === CẤU HÌNH NhatKy_DanhMuc (quan hệ nhiều-nhiều) ===
            modelBuilder.Entity<NhatKy_DanhMuc>()
                .HasKey(x => new { x.NhatKyId, x.DanhMucId });

            modelBuilder.Entity<NhatKy_DanhMuc>()
                .HasOne(x => x.NhatKy)
                .WithMany(x => x.NhatKy_DanhMucs)
                .HasForeignKey(x => x.NhatKyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NhatKy_DanhMuc>()
                .HasOne(x => x.DanhMuc)
                .WithMany(x => x.NhatKy_DanhMucs)
                .HasForeignKey(x => x.DanhMucId)
                .OnDelete(DeleteBehavior.NoAction);  // Tránh multiple cascade paths

            // === CẤU HÌNH NhatKy_Tag (nếu dùng) ===
            modelBuilder.Entity<NhatKy_Tag>()
                .HasKey(x => new { x.NhatKyId, x.TagId });

            modelBuilder.Entity<NhatKy_Tag>()
                .HasOne(x => x.NhatKy)
                .WithMany()  // Nếu NhatKy chưa có collection Tags thì để trống
                .HasForeignKey(x => x.NhatKyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NhatKy_Tag>()
                .HasOne(x => x.Tag)
                .WithMany(x => x.NhatKy_Tags)
                .HasForeignKey(x => x.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}