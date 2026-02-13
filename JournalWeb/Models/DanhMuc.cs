using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JournalWeb.Models
{
    [Table("DanhMuc")]
    public class DanhMuc
    {
        [Key]
        public int DanhMucId { get; set; }
        public string TenDanhMuc { get; set; }
        public string DuongDanIcon { get; set; }
        public string NhomDanhMuc { get; set; }

        public virtual ICollection<NhatKy_DanhMuc> NhatKy_DanhMucs { get; set; }
    }
}