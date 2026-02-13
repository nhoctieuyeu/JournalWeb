using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace JournalWeb.Models
{
    public class NhatKy
    {
        public int NhatKyId { get; set; }
        public int NguoiDungId { get; set; }
        public int MucDoId { get; set; }

        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayViet { get; set; }
        public DateTime NgayTao { get; set; }
        public bool IsRiengTu { get; set; }

        [NotMapped]
        public string CamXuc { get; set; }

        [NotMapped]
        public DateTime? NgayCapNhat { get; set; }

        public ICollection<NhatKyMedia> Medias { get; set; }
        public ICollection<NhatKy_CamXuc> NhatKy_CamXucs { get; set; }
        public ICollection<NhatKy_DanhMuc> NhatKy_DanhMucs { get; set; }

        public NguoiDung NguoiDung { get; set; }
        public MucDoCamXuc MucDoCamXuc { get; set; }
    }
}
