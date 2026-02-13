namespace JournalWeb.Models
{
    public class CamXucChiTiet
    {
        public int CamXucId { get; set; }
        public int? MucDoId { get; set; }
        public string TenCamXuc { get; set; }

        public MucDoCamXuc MucDoCamXuc { get; set; }
    }
}
