using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMiCay.Areas.Admin.Models;
using WebMiCay.Models;
namespace WebMiCay.Areas.Admin.Controllers
{
    public class HomeADController : Controller
    {
        QuanLyQuanMiCayEntities db = new QuanLyQuanMiCayEntities();
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");
            }
            else {
                var doanhThuData = db.HoaDon
                    .Where(h => h.TinhTrang == "Đã duyệt" && h.NgayDat.HasValue)
                    .GroupBy(h => new { Thang = h.NgayDat.Value.Month, Nam = h.NgayDat.Value.Year })
                    .Select(group => new DoanhThuViewModel
                    {
                        Thang = group.Key.Thang,
                        DoanhThu = group.Sum(h => h.TongGia ?? 0)
                    })
                    .ToList();

                // Sau khi lấy dữ liệu, chuyển đổi tên tháng
                foreach (var item in doanhThuData)
                {
                    item.TenThang = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(item.Thang);
                }

                // Dùng để hiển thị doanh thu theo tháng
                ViewBag.DoanhThuData = doanhThuData;

                return View(doanhThuData);
            }
        }

        public ActionResult Login() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var taikhoanAD = db.TaiKhoanNhanVien.SingleOrDefault(x => x.TenTaiKhoanNV == username && x.MatKhau == password && x.MaVaiTro == "admin");
            if (taikhoanAD != null)
            {
                var nhanVienAD = db.NhanVien.Find(taikhoanAD.MaNV);
                if (nhanVienAD != null)
                {
                    var user = new NhanVien { MaNV = nhanVienAD.MaNV, TenNV = nhanVienAD.TenNV, SDTNV = nhanVienAD.SDTNV };
                    if (Session != null)
                    {
                        Session["user"] = user;
                    }
                }
                return RedirectToAction("Index", "HomeAD");
            }
            else
            {
                TempData["error"] = "Tài khoản đăng nhập không đúng";
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Remove("user");
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        // Xử lý đếm số lượng tài khoản khách
        [HttpGet]
        public ActionResult GetCustomerCount()
        {
            var count = db.TaiKhoanKhach.Count();
            return Json(count, JsonRequestBehavior.AllowGet);
        }

        // Xử lý đếm số lượng tài khoản nhân viên
        [HttpGet]
        public ActionResult GetEmployeeCount()
        {
            var count = db.TaiKhoanNhanVien.Count();
            return Json(count, JsonRequestBehavior.AllowGet);
        }

        // Xử lý đếm số lượng món
        [HttpGet]
        public ActionResult GetDishCount()
        {
            var count = db.Mon.Count();
            return Json(count, JsonRequestBehavior.AllowGet);
        }
    }
}