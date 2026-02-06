using System;
using System.ComponentModel.DataAnnotations;

namespace JournalWeb.Models
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }
        public int? NguoiDungId { get; set; }
        public string HanhDong { get; set; }
        public string TenBang { get; set; }
        public int? IdBanGhi { get; set; }
        public DateTime ThoiGian { get; set; }
    }
}
