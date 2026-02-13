namespace JournalWeb.Models
{
    public class NhatKy_DanhMuc
    {
        public int NhatKyId { get; set; }
        public int DanhMucId { get; set; }

        public NhatKy NhatKy { get; set; }
        public DanhMucTacDong DanhMucTacDong { get; set; }
    }
}
