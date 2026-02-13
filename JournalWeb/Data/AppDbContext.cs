using Microsoft.EntityFrameworkCore;
using JournalWeb.Models;

namespace JournalWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Các DbSet chính
        public DbSet<NguoiDung> NguoiDung { get; set; }
        public DbSet<PhanQuyen> PhanQuyen { get; set; }
        public DbSet<CamXuc> CamXuc { get; set; }
        public DbSet<CamXucChiTiet> CamXucChiTiet { get; set; }
        public DbSet<DanhMuc> DanhMuc { get; set; }
        public DbSet<NhatKy> NhatKy { get; set; }
        public DbSet<NhatKy_CamXucChiTiet> NhatKy_CamXucChiTiet { get; set; }
        public DbSet<NhatKy_DanhMuc> NhatKy_DanhMuc { get; set; }
        public DbSet<NhatKyMedia> NhatKyMedia { get; set; }

        // Các bảng phụ (giữ nguyên nếu có)
        public DbSet<Menu> Menu { get; set; }
        public DbSet<Slide> Slide { get; set; }
        public DbSet<TinTuc> TinTuc { get; set; }
        public DbSet<AuditLog> AuditLog { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<NhatKy_Tag> NhatKy_Tag { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình cho NguoiDung
            modelBuilder.Entity<NguoiDung>()
                .HasIndex(x => x.TaiKhoan)
                .IsUnique();

            modelBuilder.Entity<NguoiDung>()
                .HasIndex(x => x.Email)
                .IsUnique();

            // Cấu hình quan hệ NhatKy - NguoiDung
            modelBuilder.Entity<NhatKy>()
                .HasOne(x => x.NguoiDung)
                .WithMany(x => x.NhatKys)
                .HasForeignKey(x => x.NguoiDungId)
                .OnDelete(DeleteBehavior.Cascade);

            // NhatKy - CamXuc
            modelBuilder.Entity<NhatKy>()
                .HasOne(x => x.CamXuc)
                .WithMany(x => x.NhatKys)
                .HasForeignKey(x => x.MucDoId)
                .OnDelete(DeleteBehavior.Cascade);

            // NhatKyMedia
            modelBuilder.Entity<NhatKyMedia>()
                .HasOne(x => x.NhatKy)
                .WithMany(x => x.Medias)
                .HasForeignKey(x => x.NhatKyId)
                .OnDelete(DeleteBehavior.Cascade);

            // NhatKy_CamXucChiTiet
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
                .OnDelete(DeleteBehavior.NoAction);  // tránh multiple cascade

            // NhatKy_DanhMuc
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
                .OnDelete(DeleteBehavior.NoAction);

            // NhatKy_Tag (nếu dùng)
            modelBuilder.Entity<NhatKy_Tag>()
                .HasKey(x => new { x.NhatKyId, x.TagId });

            base.OnModelCreating(modelBuilder);
        }
    }
}