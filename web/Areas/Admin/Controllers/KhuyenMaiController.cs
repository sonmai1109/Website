using Nhom3.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using PagedList;

namespace Nhom3.Areas.Admin.Controllers
{
    public class KhuyenMaiController : BaseController
    {
        private Nhom3DB db = new Nhom3DB();

        // Index: liệt kê (tìm kiếm, phân trang)
        public ActionResult Index(string q, int page = 1, int pageSize = 10)
        {
            ViewBag.q = q;
            var list = db.KhuyenMais.AsQueryable();
            if (!string.IsNullOrEmpty(q)) list = list.Where(k => k.TenKM.Contains(q));
            return View(list.OrderByDescending(k => k.NgayTao).ToPagedList(page, pageSize));
        }

        // Create GET
        public ActionResult Create()
        {
            return View(new KhuyenMai { TrangThai = 1, NgayTao = DateTime.Now });
        }

        // Create POST
        [HttpPost]
        //[ValidateAntiForgeryToken] // có thể bật lại nếu muốn bảo mật
        public JsonResult Create(KhuyenMai model)
        {
            if (ModelState.IsValid)
            {
                model.NgayTao = DateTime.Now;
                db.KhuyenMais.Add(model);
                db.SaveChanges();

                return Json(new { success = true });
            }

            // trả lỗi
            var msg = string.Join("; ", ModelState.Values
                                .SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage));
            return Json(new { success = false, message = msg });
        }



        [HttpPost]
        public JsonResult Edit(KhuyenMai model)
        {
            if (ModelState.IsValid)
            {
                var km = db.KhuyenMais.Find(model.MaKM);
                if (km == null)
                    return Json(new { success = false, message = "Không tìm thấy khuyến mãi" });

                km.TenKM = model.TenKM;
                km.PhanTram = model.PhanTram;
                km.MoTa = model.MoTa;
                km.TrangThai = model.TrangThai;

                // Ngày hợp lệ
                if (model.NgayBatDau.HasValue && model.NgayBatDau.Value >= new DateTime(1753, 1, 1))
                    km.NgayBatDau = model.NgayBatDau;
                else
                    km.NgayBatDau = null;

                if (model.NgayKetThuc.HasValue && model.NgayKetThuc.Value >= new DateTime(1753, 1, 1))
                    km.NgayKetThuc = model.NgayKetThuc;
                else
                    km.NgayKetThuc = null;

                db.Entry(km).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { success = true });
            }

            var msg = string.Join("; ", ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage));
            return Json(new { success = false, message = msg });
        }


        // Toggle status (Ẩn/Hiện) - Ajax
        [HttpPost]
        public JsonResult ToggleStatus(int id)
        {
            var km = db.KhuyenMais.Find(id);
            if (km == null) return Json(new { ok = false });
            km.TrangThai = (km.TrangThai == 1) ? 0 : 1;
            db.Entry(km).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { ok = true, newStatus = km.TrangThai });
        }

        // Delete - Ajax
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var km = db.KhuyenMais.Find(id);
            if (km == null) return Json(new { ok = false });
            // xóa liên kết trước
            var links = db.SanPhamKhuyenMais.Where(s => s.MaKM == id);
            db.SanPhamKhuyenMais.RemoveRange(links);
            db.KhuyenMais.Remove(km);
            db.SaveChanges();
            return Json(new { ok = true });
        }

        // ManageProducts view (UI để gán/thu hồi sp)
        public ActionResult ManageProducts(int id) // id = MaKM
        {
            var km = db.KhuyenMais.Find(id);
            if (km == null) return HttpNotFound();
            var assigned = db.SanPhamKhuyenMais
                              .Where(s => s.MaKM == id)
                              .Include(s => s.SanPham)
                              .ToList();
            ViewBag.KhuyenMai = km;
            return View(assigned); // model: list of SanPhamKhuyenMai
        }

        // Ajax: Assign product to promotion
        [HttpPost]
        public JsonResult AssignProduct(int maKM, int maSP)
        {
            if (!db.SanPhamKhuyenMais.Any(x => x.MaKM == maKM && x.MaSP == maSP))
            {
                db.SanPhamKhuyenMais.Add(new SanPhamKhuyenMai { MaKM = maKM, MaSP = maSP });
                db.SaveChanges();
            }
            return Json(new { ok = true });
        }

        // Ajax: Remove assigned product (by link ID)
        [HttpPost]
        public JsonResult RemoveProduct(int id)
        {
            var link = db.SanPhamKhuyenMais.Find(id);
            if (link != null)
            {
                db.SanPhamKhuyenMais.Remove(link);
                db.SaveChanges();
                return Json(new { ok = true });
            }
            return Json(new { ok = false });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
        [HttpPost]
        public JsonResult Get(int id)
        {
            var km = db.KhuyenMais.Find(id);
            if (km == null) return Json(null);

            return Json(new
            {
                km.MaKM,
                km.TenKM,
                km.PhanTram,
                km.MoTa,
                NgayBatDau = km.NgayBatDau?.ToString("yyyy-MM-dd"),
                NgayKetThuc = km.NgayKetThuc?.ToString("yyyy-MM-dd")
            }, JsonRequestBehavior.AllowGet);

        }


    }
}
