using System.ComponentModel.DataAnnotations;

namespace JournalWeb.Models
{
    public class Menu
    {
        [Key]
        public int MenuId { get; set; }
        public string TenMenu { get; set; }
        public string LienKet { get; set; }
        public int? ThuTu { get; set; }
        public int? MenuChaId { get; set; }
    }
}
