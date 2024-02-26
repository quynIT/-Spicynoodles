using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Parser.SyntaxTree;

namespace WebMiCay.Controllers
{
    public class FeedbackController : Controller
    {
        QuanLyQuanMiCayEntities db = new QuanLyQuanMiCayEntities();
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                // Người dùng chưa đăng nhập, chuyển hướng và hiển thị thông báo lỗi
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để sử dụng tính năng phản hồi";
                return RedirectToAction("Login", "Home");
            }

            if (Session["user"] != null)
            {
                var user = Session["user"] as KhachHang;
                if (user != null)
                {
                    ViewBag.TenKH = user.TenKH;
                    ViewBag.SDT = user.SDT;
                }
            }

            return View();
        }

        // Action xử lý lưu thông tin phản hồi của khách
        [HttpPost]
        public ActionResult Index(string s)
        {
            string maKhachHang = "";

            // Kiểm tra xem Session có tồn tại và có là đối tượng KhachHang không
            if (Session["user"] != null)
            {
                // Sử dụng user.MaKH ở đây
                var user = Session["user"] as KhachHang;
                maKhachHang = user.MaKH;
            }

            db.Database.ExecuteSqlCommand("EXEC sp_InsertPhanHoi @p0, @p1",
            parameters: new[]
            {
                new SqlParameter("@p0", s),
                new SqlParameter("@p1", maKhachHang)
            });

            return RedirectToAction("Index", "HomeClient");
        }
    }
}