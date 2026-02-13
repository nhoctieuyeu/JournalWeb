using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JournalWeb.Models
{
    [Table("CamXucChiTiet")]
    public class CamXucChiTiet
    {
        [Key]
        public int ChiTietId { get; set; }
        public int MucDoId { get; set; }
        public string TenChiTiet { get; set; }
        public int ThuTu { get; set; }

        [ForeignKey("MucDoId")]
        public virtual CamXuc CamXuc { get; set; }

        public virtual ICollection<NhatKy_CamXucChiTiet> NhatKy_CamXucChiTiets { get; set; }
    }
}