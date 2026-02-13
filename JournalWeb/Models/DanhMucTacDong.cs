using System.Collections.Generic;

namespace JournalWeb.Models
{
    public class DanhMucTacDong
    {
        public int DanhMucId { get; set; }
        public string TenDanhMuc { get; set; }
        public string IconSVG { get; set; }
        public string NhomDanhMuc { get; set; }

        public ICollection<NhatKy_DanhMuc> NhatKy_DanhMucs { get; set; }
    }
}
