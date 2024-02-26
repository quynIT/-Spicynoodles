using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMiCay.Areas.Admin.Models;

namespace WebMiCay.Areas.Admin.Controllers
{
    public class DishController : Controller
    {
        QuanLyQuanMiCayEntities db = new QuanLyQuanMiCayEntities();
        public ActionResult Index(string searchString, string categoryString, string sortOrder)
        {
            var data = (from a in db.Mon
                        join b in db.LoaiMon on a.MaTheLoai equals b.MaTheLoai
                        select new DishViewModel
                        {
                            MaMon = a.MaMon,
                            TenMon = a.TenMon,
                            GiaCa = a.GiaCa,
                            AnhMon = a.AnhMon,
                            MaTheLoai = b.MaTheLoai,
                            TenTheLoai = b.TenTheLoai
                        });

            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                data = data.Where(b => b.TenMon.ToLower().Contains(searchString));
            }

            // Lấy danh sách thể loại từ cơ sở dữ liệu
            var categories = db.LoaiMon.Select(c => new { c.TenTheLoai, c.MaTheLoai }).ToList();

            // Truyền danh sách thể loại cho dropdown list
            ViewBag.CategoryList = new SelectList(categories, "MaTheLoai", "TenTheLoai");

            // Xử lý tìm kiếm theo thể loại
            if (!String.IsNullOrEmpty(categoryString))
            {
                data = data.Where(b => b.MaTheLoai == categoryString);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    data = data.OrderByDescending(b => b.TenMon);
                    break;
                default:
                    data = data.OrderBy(b => b.TenMon);
                    break;
            }

            return View(data.ToList());
        }

        public ActionResult Adddish()
        {
            
            // Lấy danh sách thể loại từ CSDL
            var danhSachTheLoai = db.LoaiMon.ToList();

            // Tạo một instance của DishViewModel và đặt DSTheLoaiMon của nó
            var model = new DishViewModel
            {
                DSTheLoaiMon = danhSachTheLoai
            };

            // Truyền model đến view
            return View(model);
        }

        [HttpPost]
        public ActionResult Adddish(string TenMon, decimal GiaCa, HttpPostedFileBase AnhMon, string MaTheLoai)
        {
            // Kiểm tra xem tên món đã tồn tại chưa
            if (db.Mon.Any(m => m.TenMon == TenMon))
            {
                ViewBag.ErrorMessage = "Tên món ăn đã tồn tại. Vui lòng chọn tên khác.";
                var danhSachTheLoai = db.LoaiMon.ToList();
                var model = new DishViewModel
                {
                    DSTheLoaiMon = danhSachTheLoai
                };
                    return View(model);
            }

            // Lưu file ảnh vào thư mục "Images"
            string filePath = Path.Combine(Server.MapPath("~/Content/food_img/"), Path.GetFileName(AnhMon.FileName));
            AnhMon.SaveAs(filePath);

            db.Database.ExecuteSqlCommand("EXEC sp_InsertMon @p0, @p1, @p2, @p3",
            parameters: new[]
            {
                new SqlParameter("@p0", TenMon),
                new SqlParameter("@p1", GiaCa),
                new SqlParameter("@p2", AnhMon.FileName),
                new SqlParameter("@p3", MaTheLoai)
            });
           
            return RedirectToAction("Index");
        }

        public ActionResult Updatedish()
        {
            // Lấy danh sách thể loại từ CSDL
            var danhSachTheLoai = db.LoaiMon.ToList();

            // Tạo một instance của DishViewModel và đặt DSTheLoaiMon của nó
            var model = new DishViewModel
            {
                DSTheLoaiMon = danhSachTheLoai
            };

            // Truyền model đến view
            return View(model);
        }

        [HttpPost]
        public ActionResult Updatedish(string maMon, string tenMon, decimal? giaCa, HttpPostedFileBase anhMon, string maTheLoai, string tenTheLoai)
        {
            var d = db.Mon.SingleOrDefault(c => c.MaMon == maMon);

            if (d == null)
            {
                // Xử lý khi giá trị maMon không tồn tại
                return HttpNotFound(); // hoặc RedirectToAction("Error")
            }

            if (!string.IsNullOrEmpty(tenMon))
            {
                d.TenMon = tenMon;
            }

            // Kiểm tra xem tên món đã tồn tại chưa
            if (db.Mon.Any(m => m.TenMon == tenMon))
            {
                ViewBag.ErrorMessage = "Tên món ăn đã tồn tại. Vui lòng chọn tên khác.";
                var danhSachTheLoai = db.LoaiMon.ToList();
                var model = new DishViewModel
                {
                    DSTheLoaiMon = danhSachTheLoai
                };
                return View(model);
            }

            // Nếu giá cả được nhập, cập nhật giá cả
            if (giaCa.HasValue && giaCa.Value > 0)
            {
                d.GiaCa = giaCa.Value;
            }

            // Nếu hình ảnh được chọn, cập nhật hình ảnh
            if (anhMon != null && anhMon.ContentLength > 0)
            {
                string fileName = Path.GetFileName(anhMon.FileName);
                string filePath = Path.Combine(Server.MapPath("~/Content/food_img/"), fileName);
                anhMon.SaveAs(filePath);


                // Cập nhật đường dẫn hình ảnh trong cơ sở dữ liệu
                d.AnhMon = anhMon.FileName;
            }

            //Nếu mã thể loại được chọn, cập nhật mã thể loại
            if (!string.IsNullOrEmpty(maTheLoai))
            {
                d.MaTheLoai = maTheLoai;
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Deletedish(string maMon)
        {
            var d = db.Mon.SingleOrDefault(c => c.MaMon == maMon);

            db.Mon.Remove(d);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}