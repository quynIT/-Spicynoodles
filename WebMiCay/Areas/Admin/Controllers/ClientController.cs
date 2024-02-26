using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMiCay.Areas.Admin.Models;
using static System.Web.Razor.Parser.SyntaxConstants;

namespace WebMiCay.Areas.Admin.Controllers
{
    public class ClientController : Controller
    {
        QuanLyQuanMiCayEntities db = new QuanLyQuanMiCayEntities();

        public ActionResult Index(string searchString, string sortOrder, string genderFilter)
        {
            var data = (from a in db.TaiKhoanKhach
                        join b in db.KhachHang on a.MaKH equals b.MaKH
                        select new ClientViewModel
                        {
                            MaKH = b.MaKH,
                            TenKH = b.TenKH,
                            SDT = b.SDT,
                            NgaySinh = b.NgaySinh,
                            DiaChi = b.DiaChi,
                            GioiTinh = b.GioiTinh,
                            MaTaiKhoanKhach = a.MaTaiKhoanKhach,
                            TenTaiKhoanKhach = a.TenTaiKhoanKhach,
                            MatKhauKhach = a.MatKhauKhach,
                            NgayTao = a.NgayTao,
                        });

            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                data = data.Where(b => b.TenKH.ToLower().Contains(searchString));
            }

            if (!String.IsNullOrEmpty(genderFilter))
            {
                data = data.Where(b => b.GioiTinh == genderFilter);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    data = data.OrderByDescending(b => b.TenKH);
                    break;
                default:
                    data = data.OrderBy(b => b.TenKH);
                    break;
            }

            return View(data.ToList());
        }

        // Action xử lý lấy thông tin cho xem full thông tin khách hàng
        public ActionResult Profileclient(string maKhach, string tenTaiKhoanKhach, string tenKH, string soDienThoai, DateTime ngaySinh, string diaChi, string gioiTinh, string matKhau)
        {
            var cm = db.KhachHang.SingleOrDefault(c => c.MaKH == maKhach);

            var model = new ClientViewModel
            {
                TenTaiKhoanKhach = tenTaiKhoanKhach,
                TenKH = tenKH,
                SDT = soDienThoai,
                NgaySinh = ngaySinh,
                DiaChi = diaChi,
                GioiTinh = gioiTinh,
                MatKhauKhach = matKhau
            };

            return View(model);
        }

        // Action quay về trang trước
        public ActionResult GoBack()
        {
            return RedirectToAction("Index");
        }

        // Action hiện form cập nhật thông tin khách và hiện tên tài khoản khách
        public ActionResult Updateclient(string tenTaiKhoanKhach)
        {
            var model = new ClientViewModel
            {
                TenTaiKhoanKhach = tenTaiKhoanKhach
            };

            return View(model);
        }

        // Action xử lý cập nhật thông tin khách hàng
        [HttpPost]
        public ActionResult Updateclient(string maKhach, string maTaiKhoanKhach, string tenKH, string soDienThoai, DateTime? ngaySinh, string diaChi, string gioiTinh, string matKhau)
        {
            var cm = db.KhachHang.SingleOrDefault(c => c.MaKH == maKhach);

            if (!string.IsNullOrEmpty(tenKH))
            {
                cm.TenKH = tenKH;
            }

            if (!string.IsNullOrEmpty(soDienThoai))
            {
                cm.SDT = soDienThoai;
            }

            if (ngaySinh.HasValue)
            {
                cm.NgaySinh = ngaySinh.Value;
            }

            if (!string.IsNullOrEmpty(diaChi))
            {
                cm.DiaChi = diaChi;
            }

            cm.GioiTinh= gioiTinh;

            var cma = db.TaiKhoanKhach.SingleOrDefault(d => d.MaTaiKhoanKhach == maTaiKhoanKhach);

            if (!string.IsNullOrEmpty(matKhau))
            {
                cma.MatKhauKhach = matKhau;
            }

            db.SaveChanges();
            
            return RedirectToAction("Index");
        }

        // Action xóa khách hàng
        public ActionResult Deleteclient(string maKhach)
        {
            var cm = db.KhachHang.SingleOrDefault(c => c.MaKH == maKhach);

            db.KhachHang.Remove(cm);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}