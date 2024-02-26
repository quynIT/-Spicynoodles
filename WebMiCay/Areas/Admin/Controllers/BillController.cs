using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMiCay.Areas.Admin.Models;

namespace WebMiCay.Areas.Admin.Controllers
{
    public class BillController : Controller
    {
        QuanLyQuanMiCayEntities db = new QuanLyQuanMiCayEntities();
        public ActionResult Index()
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
                            MaNV = a.MaNV,
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
    }
}