using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JournalWeb.Models
{
    [Table("CamXuc")]
    public class CamXuc
    {
        [Key]
        public int CamXucId { get; set; }
        public string TenCamXuc { get; set; }
        public string MaMauGradient { get; set; }
        public string DuongDanIcon { get; set; }
        public string NhomCamXuc { get; set; }

        public virtual ICollection<CamXucChiTiet> CamXucChiTiets { get; set; }
        public virtual ICollection<NhatKy> NhatKys { get; set; }
    }
}