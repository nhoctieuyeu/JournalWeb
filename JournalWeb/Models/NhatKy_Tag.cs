using System.ComponentModel.DataAnnotations.Schema;

namespace JournalWeb.Models
{
    public class NhatKy_Tag
    {
        public int NhatKyId { get; set; }
        public int TagId { get; set; }

        public NhatKy NhatKy { get; set; }
        public Tag Tag { get; set; }
    }
}
