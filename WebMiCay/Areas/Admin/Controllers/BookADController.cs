using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMiCay.Models;

namespace WebMiCay.Areas.Admin.Controllers
{
    public class BookADController : Controller
    {
        QuanLyQuanMiCayEntities db = new QuanLyQuanMiCayEntities();
        public ActionResult Index()
        {
            var data = (from a in db.TaiKhoanKhach
                        join b in db.KhachHang on a.MaKH equals b.MaKH
                        join c in db.LichDat on a.MaKH equals c.MaKH
                        select new BookingViewModel
                        {
                            MaKH = b.MaKH,
                            TenKH = b.TenKH,
                            SDT = b.SDT,
                            MaTaiKhoanKhach = a.MaTaiKhoanKhach,
                            TenTaiKhoanKhach = a.TenTaiKhoanKhach,
                            MaLichDat = c.MaLichDat,
                            NgayDat = c.NgayDat,
                            GioDat = c.GioDat,
                            SoBan = c.SoBan,
                            TinhTrang = c.TinhTrang,
                            MaNV = c.MaNV
                        }).ToList();
            return View(data);
        }
    }
}