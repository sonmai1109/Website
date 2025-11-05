using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nhom3.Models;
namespace Nhom3.Controllers
{
    public class CartController : Controller
    {
        Nhom3DB db = new Nhom3DB();
        //GET: Cart
        [HttpGet]
        public ActionResult Orders()
        {
            List<SanPhamChiTiet> list = new List<SanPhamChiTiet>();

            if (Session[Nhom3.Session.ConstainCart.CART] != null)
            {
                List<ChiTietHoaDon> ses = (List<ChiTietHoaDon>)Session[Nhom3.Session.ConstainCart.CART];

                foreach (ChiTietHoaDon item in ses)
                {
                    var spct = db.SanPhamChiTiets
                                 .Include("SanPham")
                                 .Include("KichCo")
                                 .FirstOrDefault(s => s.IDCTSP == item.IDCTSP);

                    if (spct != null)
                    {
                        // --- tìm khuyến mãi có hiệu lực áp dụng cho sản phẩm ---
                        var km = (from k in db.KhuyenMais
                                  join spkm in db.SanPhamKhuyenMais on k.MaKM equals spkm.MaKM
                                  where spkm.MaSP == spct.MaSP
                                        && k.NgayBatDau <= DateTime.Now
                                        && k.NgayKetThuc >= DateTime.Now
                                  select k).FirstOrDefault();

                        if (km != null)
                        {
                            decimal giaGiam = spct.SanPham.Gia * (1 - (decimal)km.PhanTram / 100);
                            item.GiaMua = Math.Round(giaGiam, 0);
                        }
                        else
                        {
                            item.GiaMua = spct.SanPham.Gia;
                        }

                        spct.ChiTietHoaDons.Add(item);
                        list.Add(spct);
                    }
                }
            }

            return View(list);
        }
        [HttpPost]
        public JsonResult AddToCart(ChiTietHoaDon chiTiet)
        {
            var spct = db.SanPhamChiTiets.Include("SanPham").FirstOrDefault(x => x.IDCTSP == chiTiet.IDCTSP);
            if (spct == null)
                return Json(new { status = false, message = "Không tìm thấy sản phẩm." }, JsonRequestBehavior.AllowGet);

            if (chiTiet.SoLuongMua > spct.SoLuong)
                return Json(new { status = false, message = "Số lượng vượt quá tồn kho." }, JsonRequestBehavior.AllowGet);

            // --- tìm khuyến mãi còn hiệu lực cho sản phẩm ---
            var km = (from k in db.KhuyenMais
                      join spkm in db.SanPhamKhuyenMais on k.MaKM equals spkm.MaKM
                      where spkm.MaSP == spct.MaSP
                            && k.NgayBatDau <= DateTime.Now
                            && k.NgayKetThuc >= DateTime.Now
                      select k).FirstOrDefault();

            decimal giaBan = spct.SanPham.Gia;
            if (km != null)
                giaBan = Math.Round(spct.SanPham.Gia * (1 - (decimal)km.PhanTram / 100), 0);

            chiTiet.GiaMua = giaBan;

            // --- thêm vào session ---
            List<ChiTietHoaDon> list = new List<ChiTietHoaDon>();
            if (Session[Nhom3.Session.ConstainCart.CART] != null)
            {
                list = (List<ChiTietHoaDon>)Session[Nhom3.Session.ConstainCart.CART];
                var existing = list.FirstOrDefault(x => x.IDCTSP == chiTiet.IDCTSP);
                if (existing != null)
                {
                    existing.SoLuongMua += chiTiet.SoLuongMua;
                }
                else
                {
                    list.Add(chiTiet);
                }
            }
            else
            {
                list.Add(chiTiet);
            }

            // xóa sản phẩm có số lượng 0
            list.RemoveAll(x => x.SoLuongMua <= 0);
            Session[Nhom3.Session.ConstainCart.CART] = list;

            return Json(new { status = true, cart = list }, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        // public ActionResult Orders()
        // {
        //     List<SanPhamChiTiet> list = new List<SanPhamChiTiet>();
        //     if (Session[Nhom3.Session.ConstainCart.CART] != null)
        //     {
        //         List<ChiTietHoaDon> ses = (List<ChiTietHoaDon>)Session[Nhom3.Session.ConstainCart.CART];
        //         foreach (ChiTietHoaDon item in ses)
        //         {
        //             list.Add(db.SanPhamChiTiets.Include("SanPham").Include("KichCo").Where(s => s.IDCTSP == item.IDCTSP).FirstOrDefault());
        //         }
        //         for (int i = 0; i < list.Count; i++)
        //         {
        //             list[i].ChiTietHoaDons.Add(ses[i]);
        //         }
        //     }
        //     return View(list);
        // }

        // [HttpPost]
        // public JsonResult AddToCart(ChiTietHoaDon chiTiet)
        // {
        //     if (chiTiet.SoLuongMua > db.SanPhamChiTiets.Where(x => x.IDCTSP == chiTiet.IDCTSP).FirstOrDefault().SoLuong)
        //     {
        //         return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        //     }
        //     bool isExists = false;
        //     List<ChiTietHoaDon> list = new List<ChiTietHoaDon>();
        //     if (Session[Nhom3.Session.ConstainCart.CART] != null)
        //     {
        //         list = (List<ChiTietHoaDon>)Session[Nhom3.Session.ConstainCart.CART];
        //         foreach (ChiTietHoaDon item in list)
        //         {
        //             if (item.IDCTSP == chiTiet.IDCTSP)
        //             {
        //                 item.SoLuongMua += chiTiet.SoLuongMua;
        //                 isExists = true;
        //             }
        //         }
        //         if (!isExists)
        //         {
        //             list.Add(chiTiet);
        //         }
        //     }
        //     else
        //     {
        //         list = new List<ChiTietHoaDon>();
        //         list.Add(chiTiet);
        //     }
        //     list.RemoveAll((x) => x.SoLuongMua <= 0);
        //     foreach (ChiTietHoaDon item in list)
        //     {
        //         item.GiaMua = db.SanPhamChiTiets.Include("SanPham").Where(s => s.IDCTSP == item.IDCTSP).FirstOrDefault().SanPham.Gia;
        //     }
        //     Session[Nhom3.Session.ConstainCart.CART] = list;
        //     return Json(new { status = true, cart = list }, JsonRequestBehavior.AllowGet);
        // }
        //[HttpGet]
        //public ActionResult Orders()
        //{
        //    var list = new List<SanPhamChiTiet>();

        //    if (Session[Nhom3.Session.ConstainCart.CART] is List<ChiTietHoaDon> ses)
        //    {
        //        foreach (var item in ses)
        //        {
        //            var spct = db.SanPhamChiTiets
        //                .Include("SanPham")
        //                .Include("KichCo")
        //                .FirstOrDefault(s => s.IDCTSP == item.IDCTSP);

        //            if (spct != null)
        //            {
        //                // ✅ Kiểm tra lại khuyến mãi đang còn hiệu lực
        //                var km = db.SanPhamKhuyenMais
        //                    .Include("KhuyenMai")
        //                    .FirstOrDefault(x => x.MaSP == spct.MaSP &&
        //                                         x.KhuyenMai.TrangThai == 1 &&
        //                                         x.KhuyenMai.NgayBatDau <= DateTime.Now &&
        //                                         x.KhuyenMai.NgayKetThuc >= DateTime.Now);

        //                decimal giaHienTai = spct.SanPham.Gia;
        //                if (km != null)
        //                {
        //                    giaHienTai = giaHienTai - (giaHienTai * km.KhuyenMai.PhanTram / 100);
        //                }

        //                // Gắn giá và thông tin mua vào model
        //                item.GiaMua = giaHienTai;
        //                spct.ChiTietHoaDons.Add(item);

        //                list.Add(spct);
        //            }
        //        }
        //    }

        //    return View(list);
        //}
        //[HttpPost]
        //public JsonResult AddToCart(ChiTietHoaDon chiTiet)
        //{
        //    // Kiểm tra tồn kho
        //    var chiTietSP = db.SanPhamChiTiets
        //        .Include("SanPham")
        //        .FirstOrDefault(s => s.IDCTSP == chiTiet.IDCTSP);

        //    if (chiTietSP == null)
        //    {
        //        return Json(new { status = false, message = "Sản phẩm không tồn tại!" }, JsonRequestBehavior.AllowGet);
        //    }

        //    if (chiTiet.SoLuongMua > chiTietSP.SoLuong)
        //    {
        //        return Json(new { status = false, message = "Số lượng hàng trong kho không đủ!" }, JsonRequestBehavior.AllowGet);
        //    }

        //    // ✅ Xác định giá khuyến mãi (nếu có)
        //    decimal giaGoc = chiTietSP.SanPham.Gia;
        //    decimal giaSauGiam = giaGoc;

        //    // Lấy khuyến mãi đang còn hiệu lực
        //    var km = db.SanPhamKhuyenMais
        //        .Include("KhuyenMai")
        //        .FirstOrDefault(x => x.MaSP == chiTietSP.MaSP &&
        //                             x.KhuyenMai.TrangThai == 1 &&
        //                             x.KhuyenMai.NgayBatDau <= DateTime.Now &&
        //                             x.KhuyenMai.NgayKetThuc >= DateTime.Now);

        //    if (km != null)
        //    {
        //        giaSauGiam = giaGoc - (giaGoc * km.KhuyenMai.PhanTram / 100);
        //    }

        //    // ✅ Thêm hoặc cập nhật session giỏ hàng
        //    var list = Session[Nhom3.Session.ConstainCart.CART] as List<ChiTietHoaDon> ?? new List<ChiTietHoaDon>();

        //    var existing = list.FirstOrDefault(x => x.IDCTSP == chiTiet.IDCTSP);
        //    if (existing != null)
        //    {
        //        existing.SoLuongMua += chiTiet.SoLuongMua;
        //    }
        //    else
        //    {
        //        chiTiet.GiaMua = giaSauGiam;
        //        list.Add(chiTiet);
        //    }

        //    // Xóa sản phẩm nếu số lượng <= 0
        //    list.RemoveAll(x => x.SoLuongMua <= 0);

        //    // ✅ Cập nhật lại session
        //    Session[Nhom3.Session.ConstainCart.CART] = list;

        //    // ✅ Trả lại tổng số lượng trong giỏ
        //    int totalQty = list.Sum(x => x.SoLuongMua);

        //    return Json(new { status = true, count = totalQty, cart = list }, JsonRequestBehavior.AllowGet);
        //}


        [HttpPost]
        public JsonResult DeleteFromCart(int idctsp)
        {
            List<ChiTietHoaDon> list = (List<ChiTietHoaDon>)Session[Nhom3.Session.ConstainCart.CART];
            list.RemoveAll((x) => x.IDCTSP == idctsp);
            Session[Nhom3.Session.ConstainCart.CART] = list;
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CheckOut()
        {
            TaiKhoanNguoiDung tk = (TaiKhoanNguoiDung)Session[Nhom3.Session.ConstaintUser.USER_SESSION];
            if (tk == null)
            {
                return RedirectToAction("Login", "Home");
            }
            List<SanPhamChiTiet> list = new List<SanPhamChiTiet>();
            List<ChiTietHoaDon> ses = (List<ChiTietHoaDon>)Session[Nhom3.Session.ConstainCart.CART];
            ViewBag.TaiKhoan = tk;
            foreach (ChiTietHoaDon item in ses)
            {
                list.Add(db.SanPhamChiTiets.Include("SanPham").Include("KichCo").Where(s => s.IDCTSP == item.IDCTSP).FirstOrDefault());
            }
            for (int i = 0; i < list.Count; i++)
            {
                list[i].ChiTietHoaDons.Add(ses[i]);
            }
            return View(list);
        }
    }
}