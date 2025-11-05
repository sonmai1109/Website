using Nhom3.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace Nhom3.Controllers
{
    public class DanhGiaController : Controller
    {
        private Nhom3DB db = new Nhom3DB();

        // Partial: danh sách đánh giá cho 1 sản phẩm (chỉ hiện các đánh giá đã duyệt: TrangThai == 1)
        public PartialViewResult DanhSachTheoSP(int maSP)
        {
            var ds = db.DanhGias
                       .Where(d => d.MaSP == maSP && d.TrangThai == 1)
                       .OrderByDescending(d => d.NgayTao)
                       .Include(d => d.TaiKhoanNguoiDung)
                       .ToList();
            return PartialView("_DanhSachDanhGia", ds);
        }

        // POST: thêm đánh giá (AJAX)
        [HttpPost]
        public JsonResult ThemDanhGia(int maSP, int xepHang, string binhLuan)
        {
            try
            {
                var tk = (TaiKhoanNguoiDung)Session[Nhom3.Session.ConstaintUser.USER_SESSION];
                if (tk == null)
                {
                    return Json(new { success = false, message = "Bạn phải đăng nhập mới có thể đánh giá" });
                }

                using (var db = new Nhom3DB())
                {
                    DanhGia dg = new DanhGia
                    {
                        MaSP = maSP,
                        MaTK = tk.MaTK,   // lấy từ session
                        XepHang = xepHang,
                        BinhLuan = binhLuan,
                        NgayTao = DateTime.Now
                        // Nếu bạn có thêm cột TrangThai thì set mặc định = 1
                    };

                    db.DanhGias.Add(dg);
                    db.SaveChanges();
                }

                return Json(new { success = true, message = "Gửi đánh giá thành công! Đánh giá của bạn sẽ được duyệt và đăng trong thời gian sớm nhất." });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
