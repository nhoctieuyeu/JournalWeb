using System.ComponentModel.DataAnnotations.Schema;

namespace JournalWeb.Models
{
    [Table("NhatKy_CamXucChiTiet")]
    public class NhatKy_CamXucChiTiet
    {
        public int NhatKyId { get; set; }
        public int ChiTietId { get; set; }

        [ForeignKey("NhatKyId")]
        public virtual NhatKy NhatKy { get; set; }

        [ForeignKey("ChiTietId")]
        public virtual CamXucChiTiet CamXucChiTiet { get; set; }
    }
}