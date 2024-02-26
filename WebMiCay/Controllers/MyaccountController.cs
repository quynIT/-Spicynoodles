using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebMiCay.Controllers
{
    public class MyaccountController : Controller
    {
        QuanLyQuanMiCayEntities db = new QuanLyQuanMiCayEntities();
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                // Người dùng chưa đăng nhập, chuyển hướng và hiển thị thông báo lỗi
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để sử dụng tính năng my account";
                return RedirectToAction("Login", "Home");
            }

            if (Session["user"] != null)
            {
                var user = Session["user"] as KhachHang;
                if (user != null)
                {
                    ViewBag.TenKH = user.TenKH;
                }
            }

            return View();
        }

        public ActionResult Donhang()
        {
            string maKhachHang = "";
            // Kiểm tra xem Session có tồn tại và có là đối tượng KhachHang không
            if (Session["user"] != null)
            {
                // Sử dụng user.MaKH ở đây
                var user = Session["user"] as KhachHang;
                maKhachHang = user.MaKH;
            }

            var orders = GetOrdersByCustomerId(maKhachHang);

            // Truyền ds hóa đơn vào view
            return View(orders);
        }

        private List<HoaDon> GetOrdersByCustomerId(string maKhachHang)
        {
            using (var context = new QuanLyQuanMiCayEntities())
            {
                var orders = context.HoaDon.Include(h => h.CTHD.Select(c => c.Mon)).Where(h => h.MaKH == maKhachHang).ToList();

                return orders;
            }
        }

        public ActionResult Account()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Account(string TenKH, string SDT, DateTime? NgaySinh, string DiaChi, string GioiTinh, string MatKhauKhach)
        {
            // Lấy thông tin người dùng từ session
            var user = (KhachHang)Session["user"];

            // Kiểm tra xem thông tin người dùng có tồn tại không
            if (user != null)
            {
                // Tìm khách hàng trong database bằng mã khách hàng
                var khachHang = db.KhachHang.Find(user.MaKH);

                // Tìm tài khoản khách hàng trong database bằng mã khách hàng
                var taiKhoanKhach = db.TaiKhoanKhach.SingleOrDefault(x => x.MaKH == user.MaKH);

                if (khachHang != null && taiKhoanKhach != null)
                {
                    // Cập nhật thông tin khách hàng nếu người dùng nhập giá trị mới
                    if (!string.IsNullOrEmpty(TenKH)) khachHang.TenKH = TenKH;
                    if (!string.IsNullOrEmpty(SDT)) khachHang.SDT = SDT;
                    if (NgaySinh.HasValue) khachHang.NgaySinh = NgaySinh.Value;
                    if (!string.IsNullOrEmpty(DiaChi)) khachHang.DiaChi = DiaChi;
                    if (GioiTinh != "Lựa chọn giới tính") khachHang.GioiTinh = GioiTinh;

                    // Cập nhật thông tin tài khoản khách hàng nếu người dùng nhập giá trị mới
                    if (!string.IsNullOrEmpty(MatKhauKhach)) taiKhoanKhach.MatKhauKhach = MatKhauKhach;

                    // Lưu thay đổi vào database
                    db.SaveChanges();

                    // Cập nhật thông tin trong session
                    user = new KhachHang { MaKH = khachHang.MaKH, TenKH = khachHang.TenKH, SDT = khachHang.SDT, GioiTinh = khachHang.GioiTinh };
                    Session["user"] = user;

                    // Chuyển hướng đến trang tài khoản
                    return RedirectToAction("Index");
                }
            }

            return View();
        }

        public ActionResult Deletebill1(string maHD)
        {
            var h = db.HoaDon.SingleOrDefault(c => c.MaHD == maHD);

            // Kiểm tra nếu hóa đơn đã được duyệt
            if (h.TinhTrang == "Đã duyệt")
            {
                // Trả về thông báo lỗi
                return Json(new { success = false, message = "Không được phép hủy đơn" }, JsonRequestBehavior.AllowGet);
            }

            db.HoaDon.Remove(h);
            db.SaveChanges();

            return Json(new { success = true, message = "Hóa đơn đã được hủy thành công" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            Session.Remove("user");
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}