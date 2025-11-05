namespace Nhom3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SanPham")]
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            DanhGias = new HashSet<DanhGia>();
            SanPhamChiTiets = new HashSet<SanPhamChiTiet>();
            SanPhamKhuyenMais = new HashSet<SanPhamKhuyenMai>();
        }

        [Key]
        public int MaSP { get; set; }

        public int MaDM { get; set; }

        [Required]
        [StringLength(150)]
        public string TenSP { get; set; }

        [Column(TypeName = "money")]
        public decimal Gia { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string MoTa { get; set; }

        [Required]
        [StringLength(50)]
        public string ChatLieu { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string HuongDan { get; set; }

        public DateTime NgayTao { get; set; }

        [Required]
        [StringLength(100)]
        public string NguoiTao { get; set; }

        public DateTime NgaySua { get; set; }

        [Required]
        [StringLength(100)]
        public string NguoiSua { get; set; }

        [Required]
        [StringLength(10)]
        public string MaMau { get; set; }

        [Required]
        [StringLength(150)]
        public string HinhAnh { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DanhGia> DanhGias { get; set; }

        public virtual DanhMuc DanhMuc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SanPhamChiTiet> SanPhamChiTiets { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SanPhamKhuyenMai> SanPhamKhuyenMais { get; set; }
    }
}
