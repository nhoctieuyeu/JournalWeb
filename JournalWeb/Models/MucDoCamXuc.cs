using System.Collections.Generic;

namespace JournalWeb.Models
{
    public class MucDoCamXuc
    {
        public int MucDoId { get; set; }
        public string TenMucDo { get; set; }
        public string MauNenGradient { get; set; }
        public string HieuUngCSS { get; set; }

        public ICollection<CamXucChiTiet> CamXucChiTiets { get; set; }
        public ICollection<NhatKy> NhatKys { get; set; }
    }
}
