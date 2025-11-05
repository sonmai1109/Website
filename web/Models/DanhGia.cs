namespace Nhom3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Script.Serialization;

    [Table("DanhGia")]
    public partial class DanhGia
    {
        [Key]
        public int MaDanhGia { get; set; }

        public int MaTK { get; set; }

        public int MaSP { get; set; }

        public int XepHang { get; set; }

        [StringLength(500)]
        public string BinhLuan { get; set; }

        public DateTime NgayTao { get; set; }

        public int TrangThai { get; set; }
        [ScriptIgnore]
        public virtual SanPham SanPham { get; set; }
        [ScriptIgnore]
        public virtual TaiKhoanNguoiDung TaiKhoanNguoiDung { get; set; }
    }
}
