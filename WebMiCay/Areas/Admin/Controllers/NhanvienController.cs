using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMiCay.Areas.Admin.Models;
using WebMiCay.Models;

namespace WebMiCay.Areas.Admin.Controllers
{
    public class NhanvienController : Controller
    {
        QuanLyQuanMiCayEntities db = new QuanLyQuanMiCayEntities();
        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var taiKhoanNV = db.TaiKhoanNhanVien.SingleOrDefault(x => x.TenTaiKhoanNV == username && x.MatKhau == password && x.MaVaiTro == "staff");
            if (taiKhoanNV != null)
            {
                var nhanVien = db.NhanVien.Find(taiKhoanNV.MaNV);
                if (nhanVien != null)
                {
                    var user = new NhanVien { MaNV = nhanVien.MaNV, TenNV = nhanVien.TenNV, SDTNV = nhanVien.SDTNV };
                    if (Session != null)
                    {
                        Session["username"] = user;
                    }
                }
                return RedirectToAction("Billbook", "Nhanvien");
            }
            else
            {
                TempData["error"] = "Tài khoản đăng nhập không đúng";
                return View();
            }
        }

        //public ActionResult List()
        //{
        //    return View();
        //}

        public ActionResult Billbook()
        {
            var data = (from a in db.HoaDon
                        join d in db.KhachHang on a.MaKH equals d.MaKH
                        join e in db.TaiKhoanKhach on d.MaKH equals e.MaKH
                        select new BillViewModel
                        {
                            MaKH = d.MaKH,
                            TenKH = d.TenKH,
                            SDT = d.SDT,
                            MaTaiKhoanKhach = e.MaTaiKhoanKhach,
                            TenTaiKhoanKhach = e.TenTaiKhoanKhach,
                            MaHD = a.MaHD,
                            NgayDat = a.NgayDat,
                            TinhTrang = a.TinhTrang,
                            TongGia = a.TongGia,
                            ChiTietHoaDon = (from b in db.CTHD
                                             join c in db.Mon on b.MaMon equals c.MaMon
                                             where a.MaHD == b.MaHD
                                             select new ChiTietHoaDonViewModel
                                             {
                                                 MaMon = c.MaMon,
                                                 MaHD = a.MaHD,
                                                 SoLuongMon = b.SoLuongMon,
                                                 TenMon = c.TenMon,
                                                 GiaMon = c.GiaCa
                                             }).ToList()
                        }).ToList();
            return View(data);
        }

        [HttpPost]
        public ActionResult Billbook(string maHD)
        {
            var e = Session["username"] as NhanVien;

            if (e != null)
            {
                var hd = db.HoaDon.FirstOrDefault(c => c.MaHD == maHD);

                if (hd != null)
                {
                    hd.MaNV = e.MaNV;
                    hd.TinhTrang = "Đã duyệt";
                    db.SaveChanges();
                }
                return Json(new { success = true });
            }
            return Json(new { success = false, errorMessage = "Không có thông tin nhân viên. Hãy đăng nhập lại." });
        }

        public ActionResult ChitietHD(string maHD, DateTime ngayDatVe, string maKH, string tenKH, string dienThoai, string maMon, string tenMon, int soLuong, decimal giaCa, decimal tongGia)
        {
            var h = db.HoaDon.SingleOrDefault(c => c.MaHD == maHD);
            var chiTietHoaDon = (from ct in db.CTHD
                                 join mon in db.Mon on ct.MaMon equals mon.MaMon
                                 where ct.MaHD == maHD
                                 select new ChiTietHoaDonViewModel
                                 {
                                     MaMon = ct.MaMon,
                                     MaHD = ct.MaHD,
                                     SoLuongMon = ct.SoLuongMon,
                                     TenMon = mon.TenMon,
                                     GiaMon = mon.GiaCa
                                 }).ToList();

            var model = new BillViewModel
            {
                MaHD = maHD,
                NgayDat = ngayDatVe,
                MaKH = maKH,
                TenKH = tenKH,
                SDT = dienThoai,
                MaMon = maMon,
                TenMon = tenMon,
                SoLuongMon = soLuong,
                GiaCa = giaCa,
                TongGia = tongGia,
                ChiTietHoaDon = chiTietHoaDon
            };

            return View(model);
        }

        public ActionResult Deletebill(string maHD)
        {
            var h = db.HoaDon.SingleOrDefault(c => c.MaHD == maHD);

            db.HoaDon.Remove(h);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Booktable()
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
                            TinhTrang = c.TinhTrang
                        }).ToList();
            return View(data);
        }
        
        // Action xử lý form đặt bàn dựa vào mà lịch đặt bên form truyền vào
        [HttpPost]
        public ActionResult Booktable(string maLichDat)
        {
           var e = Session["username"] as NhanVien;

            if(e != null)
            {
                var booking = db.LichDat.FirstOrDefault(c => c.MaLichDat == maLichDat);

                if (booking != null)
                {
                    booking.MaNV = e.MaNV;
                    booking.TinhTrang = "Đã duyệt";
                    db.SaveChanges();
                }
                return Json(new { success = true });
            }
            return Json(new { success = false, errorMessage = "Không có thông tin nhân viên. Hãy đăng nhập lại." });
        }

        // Action để xóa lịch đặt
        public ActionResult Deletebooktable(string maLichDat)
        {
            var book = db.LichDat.SingleOrDefault(b => b.MaLichDat == maLichDat);

            db.LichDat.Remove(book);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // Action cho hiện phản hồi của khách
        public ActionResult Phanhoi()
        {
            var data = (from a in db.TaiKhoanKhach
                        join b in db.KhachHang on a.MaKH equals b.MaKH
                        join c in db.PhanHoi on b.MaKH equals c.MaKH
                        select new FeedbackViewModel
                        {
                            MaPhanHoi = c.MaPhanHoi,
                            MaKH = b.MaKH,
                            TenKH = b.TenKH,
                            NoiDungPhanHoi = c.NoiDungPhanHoi,
                            MaTaiKhoanKhach = a.MaTaiKhoanKhach,
                        }).ToList();

            return View(data);
        }


        public ActionResult Logout()
        {
            Session.Remove("username");
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}