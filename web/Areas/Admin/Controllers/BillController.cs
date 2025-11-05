using Nhom3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity;

namespace Nhom3.Areas.Admin.Controllers
{
    public class BillController : BaseController
    {
        // GET: Admin/Bill
        Nhom3DB db = new Nhom3DB();
        [HttpGet]
        public ActionResult Index(DateTime? searchString, int? status, int page = 1, int pageSize = 10)
        {
            List<HoaDon> hoaDons = db.HoaDons.Include("TaiKhoanNguoiDung").Select(p => p).ToList();
            if (status != null)
            {
                hoaDons = hoaDons.Where(x => x.TrangThai == status).ToList();
                ViewBag.Status = status;
            }
            if (searchString != null)
            {
                ViewBag.searchString = searchString.Value.ToString("yyyy-MM-dd");
                string search = searchString.Value.ToString("dd/MM/yyyy");
                hoaDons = hoaDons.Where(hd => hd.NgayDat.ToString().Contains(search)).ToList();
            }
            return View(hoaDons.OrderBy(hd => hd.NgayDat).ToPagedList(page, pageSize));
        }

        [HttpPost]
        public JsonResult Index(int id)
        {
            //try
            //{
            //    // Lấy hóa đơn + thông tin người đặt
            //    var hoaDon = db.HoaDons
            //                   .Include("TaiKhoanNguoiDung")
            //                   .FirstOrDefault(h => h.MaHD == id);

            //    if (hoaDon == null)
            //    {
            //        return Json(new { error = "Không tìm thấy hóa đơn" }, JsonRequestBehavior.AllowGet);
            //    }

            //    // Lấy chi tiết hóa đơn + sản phẩm chi tiết + kích cỡ
            //    var chiTietHD = db.ChiTietHoaDons
            //                      .Include("SanPhamChiTiet")
            //                      .Include("SanPhamChiTiet.KichCo")
            //                      .Include("SanPhamChiTiet.SanPham")
            //                      .Where(c => c.MaHD == id)
            //                      .ToList();

            //    // Lấy danh sách sản phẩm (map từ SanPhamChiTiet)
            //    var sanPhams = chiTietHD
            //                    .Where(c => c.SanPhamChiTiet != null)
            //                    .Select(c => c.SanPhamChiTiet.SanPham)
            //                    .Distinct()
            //                    .ToList();

            //    return Json(new
            //    {
            //        hoadon = hoaDon,
            //        cthd = chiTietHD,
            //        sp = sanPhams
            //    }, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ex)
            //{
            //    return Json(new { error = "Lỗi khi lấy dữ liệu: " + ex.Message }, JsonRequestBehavior.AllowGet);
            //}
            //try
            //{
            //    // Lấy thông tin hóa đơn
            //    HoaDon hd = db.HoaDons.Include("TaiKhoanNguoiDung").FirstOrDefault(x => x.MaHD == id);

            //    // Lấy thông tin các chi tiết hóa đơn
            //    IEnumerable<ChiTietHoaDon> chiTietHoaDons = db.ChiTietHoaDons.Include("SanPhamChiTiet")
            //        .Include("SanPhamChiTiet.KichCo")
            //        .Where(x => x.MaHD == id).ToList(); // Chuyển kết quả thành danh sách

            //    // Lấy thông tin sản phẩm
            //    List<SanPham> list = new List<SanPham>();
            //    foreach (ChiTietHoaDon item in chiTietHoaDons)
            //    {
            //        SanPham sp = db.SanPhams.FirstOrDefault(x => x.MaSP == item.SanPhamChiTiet.MaSP);
            //        if (sp != null)
            //            list.Add(sp);
            //    }

            //    return Json(new { hoadon = hd, cthd = chiTietHoaDons, sp = list }, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ex)
            //{
            //    // Xử lý ngoại lệ và ghi log
            //    Console.WriteLine($"Error: {ex.Message}");
            //    return Json(new { error = "An error occurred while processing your request." }, JsonRequestBehavior.AllowGet);
            //}
            try
            {
                var hoaDon = db.HoaDons
                    .Include("TaiKhoanNguoiDung")
                    .FirstOrDefault(h => h.MaHD == id);

                if (hoaDon == null)
                {
                    return Json(new { error = "Không tìm thấy hóa đơn" }, JsonRequestBehavior.AllowGet);
                }

                var chiTietHD = db.ChiTietHoaDons
                    .Include("SanPhamChiTiet")
                    .Include("SanPhamChiTiet.KichCo")
                    .Include("SanPhamChiTiet.SanPham")
                    .Where(c => c.MaHD == id)
                    .ToList();

                var result = new
                {
                    hoadon = new
                    {
                        hoaDon.MaHD,
                        hoaDon.HoTenNguoiNhan,
                        hoaDon.SoDienThoaiNhan,
                        hoaDon.DiaChiNhan,
                        hoaDon.TrangThai,
                        hoaDon.NgayDat,
                        hoaDon.NgaySua,
                        hoaDon.NguoiSua,
                        hoaDon.GhiChu,
                        TaiKhoanNguoiDung = new { hoaDon.TaiKhoanNguoiDung.HoTen }
                    },
                    cthd = chiTietHD.Select(c => new
                    {
                        c.GiaMua,
                        c.SoLuongMua,
                        SanPhamChiTiet = new
                        {
                            KichCo = new { c.SanPhamChiTiet.KichCo.TenKichCo }
                        }
                    }),
                    sp = chiTietHD.Select(c => new
                    {
                        c.SanPhamChiTiet.SanPham.TenSP,
                        c.SanPhamChiTiet.SanPham.HinhAnh
                    })
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Lỗi: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }



        }

        [HttpPost]
        public JsonResult ChangeStatus(int mahd, int stt)
        {
            try
            {
                TaiKhoanQuanTri tk = (TaiKhoanQuanTri)Session[Nhom3.Session.ConstaintUser.ADMIN_SESSION];
                HoaDon hd = db.HoaDons.FirstOrDefault(x => x.MaHD == mahd);

                if (hd == null)
                {
                    return Json(new { status = false, message = "Không tìm thấy hóa đơn" }, JsonRequestBehavior.AllowGet);
                }
                // Khi hủy đơn hàng (stt == 0) và trước đó là đã thanh toán (3) hoặc đang giao (2)
                if (stt == 0 && (hd.TrangThai == 3 || hd.TrangThai == 2))
                {
                    var chiTietHDs = db.ChiTietHoaDons
                        .Include("SanPhamChiTiet")
                        .Where(x => x.MaHD == mahd)
                        .ToList();

                    foreach (var cthd in chiTietHDs)
                    {
                        var spct = db.SanPhamChiTiets.FirstOrDefault(x => x.IDCTSP == cthd.IDCTSP);
                        if (spct != null)
                        {
                            // Cộng lại số lượng vào kho
                            spct.SoLuong += cthd.SoLuongMua;
                        }
                    }
                }
                // Nếu đổi sang "Đã thanh toán" (3) và trước đó chưa phải thanh toán thì mới trừ kho
                if (stt == 3 && hd.TrangThai != 3)
                {
                    var chiTietHDs = db.ChiTietHoaDons
                        .Include("SanPhamChiTiet")
                        .Where(x => x.MaHD == mahd)
                        .ToList();

                    foreach (var cthd in chiTietHDs)
                    {
                        var spct = db.SanPhamChiTiets.FirstOrDefault(x => x.IDCTSP == cthd.IDCTSP);
                        if (spct != null)
                        {
                            // Nếu tồn kho >= số lượng mua
                            if (spct.SoLuong >= cthd.SoLuongMua)
                            {
                                spct.SoLuong -= cthd.SoLuongMua;
                            }
                            else
                            {
                                // Nếu tồn kho < số lượng mua thì trừ hết về 0
                                spct.SoLuong = 0;
                                // Hoặc bạn có thể return lỗi tại đây:
                                // return Json(new { status = false, message = "Số lượng tồn không đủ cho sản phẩm: " + spct.SanPham.TenSP }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                }

                // Cập nhật trạng thái hóa đơn
                hd.TrangThai = stt;
                hd.NguoiSua = tk.HoTen;
                hd.NgaySua = DateTime.Now;

                db.SaveChanges();

                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Lỗi: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}