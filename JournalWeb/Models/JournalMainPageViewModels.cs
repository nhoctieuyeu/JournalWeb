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
        public int MoodLevel { get; set; }
        public string MoodColor { get; set; }
        public List<string> MoodChiTiets { get; set; } = new List<string>();   // danh sách chip
        public List<string> DanhMucs { get; set; } = new List<string>();       // danh sách danh mục
        public DateTime NgayViet { get; set; }
        public string DisplayDateLine { get; set; }
        public List<NhatKyMedia> Medias { get; set; } = new List<NhatKyMedia>();
    }
}