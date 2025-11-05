namespace Nhom3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Script.Serialization;

    [Table("KichCo")]
    public partial class KichCo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KichCo()
        {
            SanPhamChiTiets = new HashSet<SanPhamChiTiet>();
        }

        [Key]
        public int MaKichCo { get; set; }

        [Required]
        [StringLength(10)]
        public string TenKichCo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [ScriptIgnore]
        public virtual ICollection<SanPhamChiTiet> SanPhamChiTiets { get; set; }
    }
}
