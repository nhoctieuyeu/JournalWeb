using System.ComponentModel.DataAnnotations;

namespace JournalWeb.Models
{
    public class Slide
    {
        [Key]
        public int SlideId { get; set; }
        public string HinhAnh { get; set; }
        public string TieuDe { get; set; }
        public string LienKet { get; set; }
        public int? ThuTu { get; set; }
    }
}
