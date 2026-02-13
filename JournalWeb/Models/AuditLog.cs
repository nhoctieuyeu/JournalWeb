using System;

namespace JournalWeb.Models
{
    public class AuditLog
    {
        public int LogId { get; set; }
        public string NoiDung { get; set; }
        public DateTime? ThoiGian { get; set; }
        public int? NguoiDungId { get; set; }
    }
}
