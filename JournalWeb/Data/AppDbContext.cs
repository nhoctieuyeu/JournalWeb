using Microsoft.EntityFrameworkCore;
using JournalWeb.Models;

namespace JournalWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Entities
        public DbSet<NguoiDung> NguoiDung { get; set; }
        public DbSet<NhatKy> NhatKy { get; set; }
        public DbSet<NhatKyMedia> NhatKyMedia { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<NhatKy_Tag> NhatKy_Tag { get; set; }
        public DbSet<AuditLog> AuditLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // NhatKyMedia PK & relation
            modelBuilder.Entity<NhatKyMedia>()
                .HasKey(x => x.MediaId);

            modelBuilder.Entity<NhatKy>()
                .HasMany(x => x.Medias)
                .WithOne(x => x.NhatKy)
                .HasForeignKey(x => x.NhatKyId);

            // NhatKy_Tag: composite key and relationships for many-to-many
            modelBuilder.Entity<NhatKy_Tag>()
                .HasKey(nt => new { nt.NhatKyId, nt.TagId });

            modelBuilder.Entity<NhatKy_Tag>()
                .HasOne(nt => nt.NhatKy)
                .WithMany(n => n.NhatKy_Tags)
                .HasForeignKey(nt => nt.NhatKyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NhatKy_Tag>()
                .HasOne(nt => nt.Tag)
                .WithMany(t => t.NhatKy_Tags)
                .HasForeignKey(nt => nt.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
