using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nhom3.Areas.Admin.Data;
using Nhom3.Models;
using Nhom3.Session;
using System.Data.Entity;

namespace Nhom3.Controllers
{
    public class HomeController : Controller
    {
        Nhom3DB db = new Nhom3DB();
        public ActionResult Index()

        {
            var sanPhamKhuyenMai = db.SanPhamKhuyenMais
         .Include(s => s.SanPham)
         .Include(s => s.KhuyenMai)
         .Where(s => s.KhuyenMai.NgayBatDau <= DateTime.Now && s.KhuyenMai.NgayKetThuc >= DateTime.Now)
         .ToList();

            ViewBag.SanPhamKhuyenMai = sanPhamKhuyenMai;
            ViewBag.SanPhamMoi = db.SanPhams.Select(p => p).OrderByDescending(p => p.NgayTao).Take(5);
            ViewBag.GiaTot = db.SanPhams.Select(p => p).OrderBy(p => p.Gia).Take(5);
            return View();
        }

        [ChildActionOnly]
        public ActionResult SearchBox()
        {
            IEnumerable<DanhMuc> danhmucs = db.DanhMucs.Select(p => p);
            return PartialView(danhmucs);
        }
        [ChildActionOnly]
        public ActionResult DropdownCategories()
        {
            IEnumerable<DanhMuc> danhmucs = db.DanhMucs.Select(p => p);
            return PartialView(danhmucs);
        }

        [ChildActionOnly]
        public ActionResult SelectOptionSize()
        {
            IEnumerable<KichCo> kichCos = db.KichCoes.Select(p => p);
            return PartialView(kichCos);
        }

        [ChildActionOnly]
        public ActionResult CartCount()
        {
            List<ChiTietHoaDon> list = new List<ChiTietHoaDon>();
            list = (List<ChiTietHoaDon>)Session[Nhom3.Session.ConstainCart.CART];
            return PartialView(list);
        }
        //[ChildActionOnly]
        //public ActionResult CartCount()
        //{
        //    var list = Session[Nhom3.Session.ConstainCart.CART] as List<ChiTietHoaDon>;
        //    int count = list?.Sum(x => x.SoLuongMua) ?? 0;
        //    return PartialView(count);
        //}



        [HttpGet]
        public ActionResult Login()
        {
            TaiKhoanNguoiDung session = (TaiKhoanNguoiDung)Session[Nhom3.Session.ConstaintUser.USER_SESSION];
            if (session != null)
            {
                return RedirectToAction("PageNotFound", "Error");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginAccount loginAccount)
        {
            if (ModelState.IsValid)
            {
                TaiKhoanNguoiDung tk = db.TaiKhoanNguoiDungs.Where
                (a => a.TenDangNhap.Equals(loginAccount.username) && a.MatKhau.Equals(loginAccount.password)).FirstOrDefault();
                if (tk != null)
                {
                    if (tk.TrangThai == false)
                    {
                        ModelState.AddModelError("ErrorLogin", "Tài khoản của bạn đã bị vô hiệu hóa !");
                    }
                    else
                    {
                        Session.Add(ConstaintUser.USER_SESSION, tk);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("ErrorLogin", "Tài khoản hoặc mật khẩu không đúng!");
                }
            }
            return View(loginAccount);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove(ConstaintUser.USER_SESSION);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            TaiKhoanNguoiDung session = (TaiKhoanNguoiDung)Session[Nhom3.Session.ConstaintUser.USER_SESSION];
            if (session != null)
            {
                return RedirectToAction("PageNotFound", "Error");
            }
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(TaiKhoanNguoiDung tk)
        {
            TaiKhoanNguoiDung check = db.TaiKhoanNguoiDungs.Where
                (a => a.TenDangNhap.Equals(tk.TenDangNhap)).FirstOrDefault();
            if (check != null)
            {
                ModelState.AddModelError("ErrorSignUp", "Tên đăng nhập đã tồn tại");
            }
            else
            {
                try
                {
                    tk.TrangThai = true;
                    db.TaiKhoanNguoiDungs.Add(tk);
                    db.SaveChanges();
                    TaiKhoanNguoiDung session = db.TaiKhoanNguoiDungs.Where(a => a.TenDangNhap.Equals(tk.TenDangNhap)).FirstOrDefault();
                    Session[Nhom3.Session.ConstaintUser.USER_SESSION] = session;
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("ErrorSignUp", "Đăng ký không thành công. Thử lại sau !");
                }
            }

            return View(tk);
        }


    }
}