namespace Nhom3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HoaDon")]
    public partial class HoaDon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HoaDon()
        {
            ChiTietHoaDons = new HashSet<ChiTietHoaDon>();
        }

        [Key]
        public int MaHD { get; set; }

        public int MaTK { get; set; }

        public DateTime NgayDat { get; set; }

        [Column(TypeName = "ntext")]
        public string GhiChu { get; set; }

        public int TrangThai { get; set; }

        [Required]
        [StringLength(100)]
        public string HoTenNguoiNhan { get; set; }

        [Required]
        [StringLength(100)]
        public string DiaChiNhan { get; set; }

        [Required]
        [StringLength(11)]
        public string SoDienThoaiNhan { get; set; }

        public DateTime? NgaySua { get; set; }

        [StringLength(100)]
        public string NguoiSua { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }

        public virtual TaiKhoanNguoiDung TaiKhoanNguoiDung { get; set; }
    }
}
