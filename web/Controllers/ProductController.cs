using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nhom3.Models;
using PagedList;
namespace Nhom3.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        Nhom3DB db = new Nhom3DB();
        public ActionResult Shop(string searchString, int? madm, int page = 1, int pageSize = 9)
        {
            ViewBag.searchString = searchString;
            ViewBag.madm = madm;

            // Lấy toàn bộ sản phẩm, include các bảng liên quan
            var sanphams = db.SanPhams
                .Include("DanhMuc")
                .Include("SanPhamKhuyenMais.KhuyenMai")
                .Select(sp => sp);

            // Tìm kiếm theo tên
            if (!string.IsNullOrEmpty(searchString))
            {
                sanphams = sanphams.Where(sp => sp.TenSP.Contains(searchString));
            }

            // Lọc theo danh mục
            if (madm != null && madm != 0)
            {
                sanphams = sanphams.Where(sp => sp.MaDM == madm);
                ViewBag.DanhMuc = db.DanhMucs.FirstOrDefault(d => d.MaDM == madm);
            }

            // Lấy danh sách đã sắp xếp để phân trang
            var result = sanphams.OrderBy(sp => sp.MaSP).ToPagedList(page, pageSize);

            // Truyền thêm dictionary chứa giá sau khuyến mãi (nếu có)
            ViewBag.GiaSauKhuyenMai = TinhGiaSauKhuyenMai(result.ToList());

            return View(result);
        }

        // ✅ Hàm tính giá sau khuyến mãi
        private Dictionary<int, decimal?> TinhGiaSauKhuyenMai(List<SanPham> sanPhams)
        {
            DateTime now = DateTime.Now;
            var dic = new Dictionary<int, decimal?>();

            foreach (var sp in sanPhams)
            {
                var kmsp = sp.SanPhamKhuyenMais?
                    .FirstOrDefault(k => k.KhuyenMai.TrangThai == 1
                                      && k.KhuyenMai.NgayBatDau <= now
                                      && k.KhuyenMai.NgayKetThuc >= now);
                if (kmsp != null)
                {
                    dic[sp.MaSP] = sp.Gia * (1 - (decimal)kmsp.KhuyenMai.PhanTram / 100);
                }
                else
                {
                    dic[sp.MaSP] = null;
                }
            }

            return dic;
        }

        //da sua
        public ActionResult ProductDetail(int id)
        {
            var sp = db.SanPhams
                        .Include("DanhMuc")
                        .Include("SanPhamKhuyenMais.KhuyenMai")
                        .FirstOrDefault(s => s.MaSP == id);

            if (sp == null)
                return HttpNotFound();

            // Tính giá hiển thị
            decimal giaHienThi = sp.Gia;
            KhuyenMai kmDangApDung = null;

            // Nếu sản phẩm có khuyến mãi, kiểm tra còn hạn
            if (sp.SanPhamKhuyenMais != null && sp.SanPhamKhuyenMais.Any())
            {
                var km = sp.SanPhamKhuyenMais
                            .Select(x => x.KhuyenMai)
                            .FirstOrDefault(x => DateTime.Now >= x.NgayBatDau && DateTime.Now <= x.NgayKetThuc);

                if (km != null)
                {
                    kmDangApDung = km;
                    giaHienThi = sp.Gia * (1 - (decimal)km.PhanTram / 100);
                }
            }

            // Truyền sang View
            ViewBag.GiaHienThi = giaHienThi;
            ViewBag.KhuyenMai = kmDangApDung;

            // Load danh sách chi tiết sản phẩm
            List<SanPhamChiTiet> list = db.SanPhamChiTiets
                                          .Include("KichCo")
                                          .Where(s => s.MaSP == id)
                                          .ToList();

            ViewBag.SPCT = list;
            ViewBag.Exitst = list.FirstOrDefault();

            return View(sp);
        }

        //[HttpPost]
        //public JsonResult Index(int id)
        //{
        //    SanPham sp = db.SanPhams.Include("DanhMuc").Include("SanPhamChiTiets").Where(s => s.MaSP.Equals(id)).FirstOrDefault();
        //    return Json(sp, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public JsonResult Index(int id)
        {
            var sp = db.SanPhams
                .Include("DanhMuc")
                .Include("SanPhamChiTiets")
                .Include("SanPhamKhuyenMais.KhuyenMai")
                .FirstOrDefault(s => s.MaSP == id);

            if (sp == null)
                return Json(null, JsonRequestBehavior.AllowGet);

            // Kiểm tra khuyến mãi hợp lệ
            var kmHienTai = sp.SanPhamKhuyenMais
                .Where(x => x.KhuyenMai.TrangThai == 1
                    && x.KhuyenMai.NgayBatDau <= DateTime.Now
                    && x.KhuyenMai.NgayKetThuc >= DateTime.Now)
                .Select(x => x.KhuyenMai)
                .FirstOrDefault();

            decimal giaSauKM = sp.Gia;
            if (kmHienTai != null)
            {
                giaSauKM = sp.Gia - (sp.Gia * kmHienTai.PhanTram / 100);
            }


            return Json(new
            {
                sp.MaSP,
                sp.TenSP,
                sp.Gia,
                GiaSauKhuyenMai = giaSauKM,
                sp.HinhAnh,
                DanhMuc = new { sp.DanhMuc.TenDanhMuc },
                SanPhamChiTiets = sp.SanPhamChiTiets.Select(ct => new
                {
                    ct.IDCTSP,
                    ct.MaKichCo,
                    ct.SoLuong
                }),
                sp.MaMau
            }, JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        //public JsonResult Index(int id)
        //{
        //    // Lấy sản phẩm + danh mục + chi tiết size
        //    var sp = db.SanPhams
        //        .Include("DanhMuc")
        //        .Include("SanPhamChiTiets")
        //        .Include("SanPhamKhuyenMais.KhuyenMai") // thêm liên kết qua trung gian
        //        .FirstOrDefault(s => s.MaSP == id);

        //    if (sp == null)
        //        return Json(null, JsonRequestBehavior.AllowGet);

        //    // Tìm khuyến mãi còn hiệu lực
        //    var km = sp.SanPhamKhuyenMais
        //        .Select(x => x.KhuyenMai)
        //        .FirstOrDefault(k =>
        //            k.TrangThai == 1 && // chỉ lấy khuyến mãi đang bật
        //            k.NgayBatDau <= DateTime.Now &&
        //            k.NgayKetThuc >= DateTime.Now
        //        );

        //    // Tính giá hiển thị (đã giảm)
        //    decimal giaHienThi = sp.Gia;
        //    int phanTram = 0;
        //    if (km != null)
        //    {
        //        phanTram = km.PhanTram;
        //        giaHienThi = sp.Gia - (sp.Gia * phanTram / 100);
        //    }

        //    // Trả dữ liệu JSON cho JS
        //    return Json(new
        //    {
        //        sp.MaSP,
        //        sp.TenSP,
        //        sp.Gia,
        //        GiaHienThi = giaHienThi,
        //        PhanTramGiam = phanTram,
        //        sp.HinhAnh,
        //        DanhMuc = new { sp.DanhMuc.TenDanhMuc },
        //        SanPhamChiTiets = sp.SanPhamChiTiets.Select(ct => new
        //        {
        //            ct.IDCTSP,
        //            ct.MaKichCo,
        //            ct.SoLuong
        //        })
        //    }, JsonRequestBehavior.AllowGet);
        //}



        [HttpPost]
        public JsonResult Detail(int id)
        {
            SanPhamChiTiet spct = db.SanPhamChiTiets.Where(sp => sp.IDCTSP == id).FirstOrDefault();
            return Json(spct, JsonRequestBehavior.AllowGet);
        }
    }
}