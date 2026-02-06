using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JournalWeb.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string TenTag { get; set; }

        public ICollection<NhatKy_Tag> NhatKy_Tags { get; set; }
    }
}
