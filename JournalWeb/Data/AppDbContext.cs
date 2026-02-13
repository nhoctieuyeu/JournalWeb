using Microsoft.EntityFrameworkCore;
using JournalWeb.Models;

namespace JournalWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<NguoiDung> NguoiDung { get; set; }
        public DbSet<PhanQuyen> PhanQuyen { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<Slide> Slide { get; set; }
        public DbSet<TinTuc> TinTuc { get; set; }
        public DbSet<AuditLog> AuditLog { get; set; }

        public DbSet<MucDoCamXuc> MucDoCamXuc { get; set; }
        public DbSet<CamXucChiTiet> CamXucChiTiet { get; set; }
        public DbSet<DanhMucTacDong> DanhMucTacDong { get; set; }

        public DbSet<NhatKy> NhatKy { get; set; }
        public DbSet<NhatKy_CamXuc> NhatKy_CamXuc { get; set; }
        public DbSet<NhatKy_DanhMuc> NhatKy_DanhMuc { get; set; }
        public DbSet<NhatKyMedia> NhatKyMedia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NguoiDung>().ToTable("NguoiDung");
            modelBuilder.Entity<PhanQuyen>().ToTable("PhanQuyen");
            modelBuilder.Entity<Menu>().ToTable("Menu");
            modelBuilder.Entity<Slide>().ToTable("Slide");
            modelBuilder.Entity<TinTuc>().ToTable("TinTuc");
            modelBuilder.Entity<AuditLog>().ToTable("AuditLog");
            modelBuilder.Entity<MucDoCamXuc>().ToTable("CamXuc");
            modelBuilder.Entity<CamXucChiTiet>().ToTable("CamXucChiTiet");
            modelBuilder.Entity<DanhMucTacDong>().ToTable("DanhMuc");
            modelBuilder.Entity<NhatKy>().ToTable("NhatKy");
            modelBuilder.Entity<NhatKy_CamXuc>().ToTable("NhatKy_CamXucChiTiet");
            modelBuilder.Entity<NhatKy_DanhMuc>().ToTable("NhatKy_DanhMuc");
            modelBuilder.Entity<NhatKyMedia>().ToTable("NhatKyMedia");

            modelBuilder.Entity<NguoiDung>()
                .HasIndex(x => x.TaiKhoan)
                .IsUnique();

            modelBuilder.Entity<NguoiDung>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<NhatKy>()
                .HasMany(x => x.Medias)
                .WithOne(x => x.NhatKy)
                .HasForeignKey(x => x.NhatKyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NhatKy>()
                .HasOne(x => x.MucDoCamXuc)
                .WithMany(x => x.NhatKys)
                .HasForeignKey(x => x.MucDoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CamXucChiTiet>()
                .HasOne(x => x.MucDoCamXuc)
                .WithMany(x => x.CamXucChiTiets)
                .HasForeignKey(x => x.MucDoId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<NhatKy_CamXuc>()
                .HasKey(x => new { x.NhatKyId, x.CamXucId });

            modelBuilder.Entity<NhatKy_CamXuc>()
                .HasOne(x => x.NhatKy)
                .WithMany(x => x.NhatKy_CamXucs)
                .HasForeignKey(x => x.NhatKyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NhatKy_CamXuc>()
                .HasOne(x => x.CamXucChiTiet)
                .WithMany()
                .HasForeignKey(x => x.CamXucId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<NhatKy_DanhMuc>()
                .HasKey(x => new { x.NhatKyId, x.DanhMucId });

            modelBuilder.Entity<NhatKy_DanhMuc>()
                .HasOne(x => x.NhatKy)
                .WithMany(x => x.NhatKy_DanhMucs)
                .HasForeignKey(x => x.NhatKyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NhatKy_DanhMuc>()
                .HasOne(x => x.DanhMucTacDong)
                .WithMany(x => x.NhatKy_DanhMucs)
                .HasForeignKey(x => x.DanhMucId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
    