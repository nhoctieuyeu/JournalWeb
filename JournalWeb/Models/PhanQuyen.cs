using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JournalWeb.Models
{
    [Table("PhanQuyen")]
    public class PhanQuyen
    {
        [Key]
        public int QuyenId { get; set; }
        public string TenQuyen { get; set; }

        public virtual ICollection<NguoiDung> NguoiDungs { get; set; }
    }
}