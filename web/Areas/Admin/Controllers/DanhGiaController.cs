using Nhom3.Models;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Nhom3.Areas.Admin.Controllers
{
    public class DanhGiaController : BaseController
    {
        private Nhom3DB db = new Nhom3DB();

        // GET: Admin/DanhGia
        [HttpGet]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            ViewBag.searchString = searchString;
            var ds = db.DanhGias.Include(d => d.TaiKhoanNguoiDung).Include(d => d.SanPham).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                ds = ds.Where(d => d.BinhLuan.Contains(searchString));
            }

            var paged = ds.OrderByDescending(d => d.NgayTao).ToPagedList(page, pageSize);
            return View(paged);
        }

        // toggle: ẩn/hiện (Ajax)
        [HttpPost]
        public JsonResult Update(int id)
        {
            try
            {
                var dg = db.DanhGias.Find(id);
                if (dg == null) return Json(new { status = false, message = "Không tìm thấy đánh giá" });

                dg.TrangThai = (dg.TrangThai == 1) ? 0 : 1;
                db.Entry(dg).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { status = true, message = dg.TrangThai == 1 ? "Đã hiển thị" : "Đã ẩn" });
            }
            catch (Exception)
            {
                return Json(new { status = false, message = "Có lỗi xảy ra" });
            }
        }

        // delete (Ajax)
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var dg = db.DanhGias.Find(id);
                if (dg == null) return Json(new { status = false });

                db.DanhGias.Remove(dg);
                db.SaveChanges();
                return Json(new { status = true });
            }
            catch (Exception)
            {
                return Json(new { status = false });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
