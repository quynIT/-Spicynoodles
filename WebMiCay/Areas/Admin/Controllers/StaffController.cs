using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMiCay.Areas.Admin.Models;

namespace WebMiCay.Areas.Admin.Controllers
{
    public class StaffController : Controller
    {
        QuanLyQuanMiCayEntities db = new QuanLyQuanMiCayEntities();
        // Action xử lý thông tin lấy dữ liệu từ CSDL lên
        public ActionResult Index(string searchString, string sortOrder, string genderFilter)
        {
            var data = (from a in db.TaiKhoanNhanVien
                        join b in db.NhanVien on a.MaNV equals b.MaNV
                        join c in db.VaiTro on a.MaVaiTro equals c.MaVaiTro
                        select new StaffViewModel
                        {
                            MaNV = b.MaNV,
                            TenNV = b.TenNV,
                            SDTNV = b.SDTNV,
                            NgaySinh = b.NgaySinh,
                            DiaChi = b.DiaChi,
                            GioiTinh = b.GioiTinh,
                            MaTaiKhoanNV = a.MaTaiKhoanNV,
                            TenTaiKhoanNV = a.TenTaiKhoanNV,
                            MatKhau = a.MatKhau,
                            NgayTao = a.NgayTao,
                            MaVaiTro = c.MaVaiTro,
                            TenVaiTro = c.TenVaiTro
                        });

            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                data = data.Where(b => b.TenNV.ToLower().Contains(searchString));
            }

            if (!String.IsNullOrEmpty(genderFilter))
            {
                data = data.Where(b => b.GioiTinh == genderFilter);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    data = data.OrderByDescending(b => b.TenNV);
                    break;
                default:
                    data = data.OrderBy(b => b.TenNV);
                    break;
            }

            return View(data.ToList());
        }

        // Action xử lý lấy thông tin cho xem full thông tin nhân viên
        public ActionResult Profilestaff(string maNV, string tenNV, string soDienThoai, DateTime ngaySinh, string diaChi, string gioiTinh, string tenTaiKhoanNV, string matKhau)
        {
            var s = db.NhanVien.SingleOrDefault(c => c.MaNV == maNV);

            var model = new StaffViewModel
            {
                TenTaiKhoanNV = tenTaiKhoanNV,
                TenNV = tenNV,
                SDTNV = soDienThoai,
                NgaySinh = ngaySinh,
                DiaChi = diaChi,
                GioiTinh = gioiTinh,
                MatKhau = matKhau
            };

            return View(model);
        }

        // Action quay về trang trước
        public ActionResult GoBack()
        {
            return RedirectToAction("Index");
        }

        // Action hiện form cập nhật thông tin nhân viên và hiện tên tài khoản nhân viên
        public ActionResult Updatestaff(string tenTaiKhoanNV)
        {
            var model = new StaffViewModel
            {
                TenTaiKhoanNV = tenTaiKhoanNV
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Updatestaff(string maNV, string tenNV, string soDienThoai, DateTime? ngaySinh, string diaChi, string gioiTinh, string maTaiKhoanNV, string matKhau)
        {
            var s = db.NhanVien.SingleOrDefault(c => c.MaNV == maNV);

            if (!string.IsNullOrEmpty(tenNV))
            {
                s.TenNV = tenNV;
            }

            if (!string.IsNullOrEmpty(soDienThoai))
            {
                s.SDTNV = soDienThoai;
            }

            if (ngaySinh.HasValue)
            {
                s.NgaySinh = ngaySinh.Value;
            }

            if (!string.IsNullOrEmpty(diaChi))
            {
                s.DiaChi = diaChi;
            }

            s.GioiTinh = gioiTinh;

            var sa = db.TaiKhoanNhanVien.SingleOrDefault(d => d.MaTaiKhoanNV == maTaiKhoanNV);

            if (!string.IsNullOrEmpty(matKhau))
            {
                sa.MatKhau = matKhau;
            }

            db.SaveChanges();

            // Cập nhật session ngay lập tức
            if (System.Web.HttpContext.Current.Session["user"] != null)
            {
                var user = System.Web.HttpContext.Current.Session["user"] as NhanVien;
                if (user != null && user.MaNV == maNV)
                {
                    user.TenNV = s.TenNV;
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Addstaff()
        {
            return View();
        }

        // Action thêm mới nhân viên
        [HttpPost]
        public ActionResult Addstaff(string TenNV, string SDTNV, DateTime NgaySinh, string DiaChi, string GioiTinh, string TenTaiKhoanNV, string MatKhau)
        {
            try
            {
                db.Database.ExecuteSqlCommand("EXEC sp_InsertNhanVien @p0, @p1, @p2, @p3, @p4, @p5, @p6",
                parameters: new[]
                {
                    new SqlParameter("@p0", TenNV),
                    new SqlParameter("@p1", SDTNV),
                    new SqlParameter("@p2", NgaySinh),
                    new SqlParameter("@p3", DiaChi),
                    new SqlParameter("@p4", GioiTinh),
                    new SqlParameter("@p5", TenTaiKhoanNV),
                    new SqlParameter("@p6", MatKhau)
                });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50000) // Kiểm tra mã lỗi của RAISERROR
                {
                    ViewBag.ErrorMessage = ex.Message;
                    return View(); // Trả về view với ViewBag đã được cập nhật
                }
                throw; // Nếu không phải lỗi từ RAISERROR, ném lại ngoại lệ
            }

            return RedirectToAction("Index");
        }

        // Action xóa nhân viên
        public ActionResult Deletestaff(string maNV)
        {
            var s = db.NhanVien.SingleOrDefault(c => c.MaNV == maNV);

            db.NhanVien.Remove(s);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}