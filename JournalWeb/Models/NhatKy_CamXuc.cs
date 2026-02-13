namespace JournalWeb.Models
{
    public class NhatKy_CamXuc
    {
        public int NhatKyId { get; set; }
        public int CamXucId { get; set; }

        public NhatKy NhatKy { get; set; }
        public CamXucChiTiet CamXucChiTiet { get; set; }
    }
}
