namespace Nhom3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SanPhamKhuyenMai")]
    public partial class SanPhamKhuyenMai
    {
        public int ID { get; set; }

        public int MaSP { get; set; }

        public int MaKM { get; set; }

        public virtual KhuyenMai KhuyenMai { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
