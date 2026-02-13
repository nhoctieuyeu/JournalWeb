using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JournalWeb.Models
{
    [Table("NguoiDung")]
    public class NguoiDung
    {
        [Key]
        public int NguoiDungId { get; set; }

        [Required]
        [MaxLength(50)]
        public string TaiKhoan { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("MatKhauHash")]
        public string MatKhauHash { get; set; }

        [MaxLength(255)]
        public string PinHash { get; set; }   // 👈 BỎ [NotMapped]

        [MaxLength(100)]
        public string HoTen { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string DienThoai { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? NgayTao { get; set; }

        public int QuyenId { get; set; } = 2;   // 👈 THÊM

        // Navigation properties
        [ForeignKey("QuyenId")]
        public virtual PhanQuyen PhanQuyen { get; set; }

        public virtual ICollection<NhatKy> NhatKys { get; set; }
    }
}