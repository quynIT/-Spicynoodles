using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMiCay.Models;

namespace WebMiCay.Controllers
{
    public class BookController : Controller
    {
        QuanLyQuanMiCayEntities db = new QuanLyQuanMiCayEntities();
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                // Người dùng chưa đăng nhập, chuyển hướng và hiển thị thông báo lỗi
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để sử dụng tính năng đặt bàn";
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
        
        // Action xử lý cập nhật lịch đặt lấy theo MaKH đã lưu trong Session
        [HttpPost]
        public ActionResult Index(DateTime NgayDat, TimeSpan GioDat, byte SoBan)
        {
            string maKhachHang = "";

            // Kiểm tra xem Session có tồn tại và có là đối tượng KhachHang không
            if (Session["user"] != null)
            {
                // Sử dụng user.MaKH ở đây
                var user = Session["user"] as KhachHang;
                maKhachHang = user.MaKH;
            }

            db.Database.ExecuteSqlCommand("EXEC sp_InsertLichDat @p0, @p1, @p2, @p3",
            parameters: new[]
            {
                new SqlParameter("@p0", NgayDat),
                new SqlParameter("@p1", GioDat),
                new SqlParameter("@p2", SoBan),
                new SqlParameter("@p3", maKhachHang)
            });

            return RedirectToAction("Index", "HomeClient");
        }
    }
}