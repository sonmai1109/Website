namespace Nhom3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Script.Serialization;

    [Table("ChiTietHoaDon")]
    public partial class ChiTietHoaDon
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaHD { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDCTSP { get; set; }

        public int SoLuongMua { get; set; }

        [Column(TypeName = "money")]
        public decimal GiaMua { get; set; }

        public virtual SanPhamChiTiet SanPhamChiTiet { get; set; }
        [ScriptIgnore]
        public virtual HoaDon HoaDon { get; set; }
    }
}
