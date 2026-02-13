using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JournalWeb.Models
{
    [Table("NhatKy")]
    public class NhatKy
    {
        [Key]
        public int NhatKyId { get; set; }
        public int NguoiDungId { get; set; }
        public int MucDoId { get; set; }          // bắt buộc
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayViet { get; set; }    // lưu ngày người dùng chọn
        public DateTime NgayTao { get; set; }     // hệ thống
        public bool IsRiengTu { get; set; } = true;

        // Navigation
        [ForeignKey("NguoiDungId")]
        public virtual NguoiDung NguoiDung { get; set; }

        [ForeignKey("MucDoId")]
        public virtual CamXuc CamXuc { get; set; }

        public virtual ICollection<NhatKyMedia> Medias { get; set; }
        public virtual ICollection<NhatKy_DanhMuc> NhatKy_DanhMucs { get; set; }
        public virtual ICollection<NhatKy_CamXucChiTiet> NhatKy_CamXucChiTiets { get; set; }
    }
}