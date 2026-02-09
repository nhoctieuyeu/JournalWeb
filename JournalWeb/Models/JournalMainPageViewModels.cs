using System;
using System.Collections.Generic;

namespace JournalWeb.Models
{
    public class JournalMonthGroupVm
    {
        public string GroupTitle { get; set; }
        public List<JournalCardVm> Items { get; set; } = new List<JournalCardVm>();
    }

    public class JournalCardVm
    {
        public int NhatKyId { get; set; }
        public string TieuDe { get; set; }
        public string NoiDungTomTat { get; set; }
        public string MoodLabel { get; set; }
        public int MoodLevel { get; set; } = 3;
        public string MoodColor { get; set; }
        public string MoodTopic { get; set; }
        public DateTime? NgayViet { get; set; }
        public string DisplayDateLine { get; set; }
        public List<NhatKyMedia> Medias { get; set; } = new List<NhatKyMedia>();
    }
}
