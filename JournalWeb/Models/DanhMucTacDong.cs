using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace JournalWeb.Models
{
    [Table("DanhMuc")]
    public class DanhMucTacDong
    {
        public int DanhMucId { get; set; }
        public string TenDanhMuc { get; set; }

        [Column("DuongDanIcon")]
        public string IconSVG { get; set; }

        public string NhomDanhMuc { get; set; }

        public ICollection<NhatKy_DanhMuc> NhatKy_DanhMucs { get; set; }
    }
}
