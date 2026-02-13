using System;

namespace JournalWeb.Models
{
    public class TinTuc
    {
        public int TinTucId { get; set; }
        public string TieuDe { get; set; }
        public string MoTaNgan { get; set; }
        public string NoiDung { get; set; }
        public string HinhAnh { get; set; }
        public DateTime? NgayDang { get; set; }
    }
}
