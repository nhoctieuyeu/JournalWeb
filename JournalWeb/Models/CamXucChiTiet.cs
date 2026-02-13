using System.ComponentModel.DataAnnotations.Schema;

namespace JournalWeb.Models
{
    public class CamXucChiTiet
    {
        [Column("ChiTietId")]
        public int CamXucId { get; set; }

        public int MucDoId { get; set; }

        [Column("TenChiTiet")]
        public string TenCamXuc { get; set; }

        public int ThuTu { get; set; }

        public MucDoCamXuc MucDoCamXuc { get; set; }
    }
}
