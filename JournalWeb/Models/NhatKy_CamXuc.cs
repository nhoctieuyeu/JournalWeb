using System.ComponentModel.DataAnnotations.Schema;

namespace JournalWeb.Models
{
    [Table("NhatKy_CamXucChiTiet")]
    public class NhatKy_CamXuc
    {
        public int NhatKyId { get; set; }

        [Column("ChiTietId")]
        public int CamXucId { get; set; }

        public NhatKy NhatKy { get; set; }
        public CamXucChiTiet CamXucChiTiet { get; set; }
    }
}
