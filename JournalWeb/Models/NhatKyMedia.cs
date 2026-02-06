using System;

namespace JournalWeb.Models
{
    public class NhatKyMedia
    {
        public int MediaId { get; set; }   // 👈 BẮT BUỘC (PK)
        public int NhatKyId { get; set; }

        public string DuongDanFile { get; set; }
        public string LoaiMedia { get; set; }
        public DateTime NgayTao { get; set; }

        public NhatKy NhatKy { get; set; }
    }
}
