using System;
using System.ComponentModel.DataAnnotations;

namespace JournalWeb.Models
{
    public class AuditLog
    {
        [Key]   // ✅ thêm dòng này
        public int LogId { get; set; }
        public string NoiDung { get; set; }
        public DateTime? ThoiGian { get; set; }
        public int? NguoiDungId { get; set; }
    }
}
