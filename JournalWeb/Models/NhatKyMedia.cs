using System;
using System.ComponentModel.DataAnnotations;

namespace JournalWeb.Models
{
    public class NhatKyMedia
    {
        [Key]
        public int MediaId { get; set; }
        public int NhatKyId { get; set; }

        public string DuongDanFile { get; set; }
        public string LoaiMedia { get; set; }
        public string ThoiLuong { get; set; }
        public DateTime? NgayTao { get; set; }

        public NhatKy NhatKy { get; set; }
    }
}
