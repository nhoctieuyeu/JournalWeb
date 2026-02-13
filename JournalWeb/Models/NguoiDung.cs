using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace JournalWeb.Models
{
    public class NguoiDung
    {
        public int NguoiDungId { get; set; }
        public string TaiKhoan { get; set; }

        [Column("MatKhau")]
        public string MatKhauHash { get; set; }

        public string HoTen { get; set; }
        public string Email { get; set; }
        public string DienThoai { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? NgayTao { get; set; }

        [NotMapped]
        public string PinHash { get; set; }

        public ICollection<NhatKy> NhatKys { get; set; }
    }
}
