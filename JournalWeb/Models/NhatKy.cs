using System;
using System.Collections.Generic;

namespace JournalWeb.Models
{
    public class NhatKy
    {
        public int NhatKyId { get; set; }
        public int NguoiDungId { get; set; }

        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public DateTime? NgayViet { get; set; }
        public DateTime NgayTao { get; set; }

        // ===== NAVIGATION =====
        public ICollection<NhatKyMedia> Medias { get; set; }

        // Navigation cho quan hệ nhiều-nhiều với Tag thông qua NhatKy_Tag
        public ICollection<NhatKy_Tag> NhatKy_Tags { get; set; }
    }
}
