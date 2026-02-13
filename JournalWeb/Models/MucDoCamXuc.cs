using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace JournalWeb.Models
{
    [Table("CamXuc")]
    public class MucDoCamXuc
    {
        [Column("CamXucId")]
        public int MucDoId { get; set; }

        [Column("TenCamXuc")]
        public string TenMucDo { get; set; }

        [Column("MaMauGradient")]
        public string MauNenGradient { get; set; }

        [Column("DuongDanIcon")]
        public string DuongDanIcon { get; set; }

        [Column("NhomCamXuc")]
        public string NhomCamXuc { get; set; }

        public ICollection<CamXucChiTiet> CamXucChiTiets { get; set; }
        public ICollection<NhatKy> NhatKys { get; set; }
    }
}
