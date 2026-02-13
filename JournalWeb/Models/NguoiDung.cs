using System;
using System.Collections.Generic;

namespace JournalWeb.Models
{
    public class NguoiDung
    {
        public int NguoiDungId { get; set; }
        public string TaiKhoan { get; set; }
        public string MatKhauHash { get; set; }
        public string PinHash { get; set; }

        public string HoTen { get; set; }
        public string Email { get; set; }
        public string DienThoai { get; set; }
        public bool IsActive { get; set; }
        public DateTime NgayTao { get; set; }
        public int QuyenId { get; set; }

        public ICollection<NhatKy> NhatKys { get; set; }
    }
}
