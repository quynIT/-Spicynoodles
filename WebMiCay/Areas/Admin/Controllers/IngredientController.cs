using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMiCay.Areas.Admin.Models;

namespace WebMiCay.Areas.Admin.Controllers
{
    public class IngredientController : Controller
    {
        QuanLyQuanMiCayEntities db = new QuanLyQuanMiCayEntities();
        public ActionResult Index(string searchString, string sortOrder, string genderFilter)
        {
            var data = (from nl in db.NguyenLieu
                        select new IngredientViewModel
                        {
                            MaNguyenLieu = nl.MaNguyenLieu,
                            TenNguyenLieu = nl.TenNguyenLieu,
                            GiaNguyenLieu = nl.GiaNguyenLieu,
                            DonVi = nl.DonVi,
                        });


            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                data = data.Where(b => b.TenNguyenLieu.ToLower().Contains(searchString));
            }

            if (!String.IsNullOrEmpty(genderFilter))
            {
                data = data.Where(b => b.DonVi == genderFilter);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    data = data.OrderByDescending(b => b.TenNguyenLieu);
                    break;
                default:
                    data = data.OrderBy(b => b.TenNguyenLieu);
                    break;
            }
            return View(data.ToList());
        }

        public ActionResult Addingredient()
        {
            return View();
        }

        // Action thêm mới nguyên liệu
        [HttpPost]
        public ActionResult Addingredient(string TenNguyenLieu, decimal GiaNguyenLieu, string DonVi)
        {
            try
            {
                db.Database.ExecuteSqlCommand("EXEC sp_InsertNguyenLieu @p0, @p1, @p2",
                parameters: new[]
                {
                    new SqlParameter("@p0", TenNguyenLieu),
                    new SqlParameter("@p1", GiaNguyenLieu),
                    new SqlParameter("@p2", DonVi),
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
        public ActionResult Updateingredient(string tenNguyenLieu)
        {
            var model = new IngredientViewModel
            {
                TenNguyenLieu = tenNguyenLieu
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Updateingredient(string maNL, string tenNL, decimal? giaNL, string donVi)
        {
            var s = db.NguyenLieu.SingleOrDefault(c => c.MaNguyenLieu == maNL);

            if (!string.IsNullOrEmpty(tenNL))
            {
                s.TenNguyenLieu = tenNL;
            }

            if (giaNL != null)
            {
                s.GiaNguyenLieu = giaNL.Value;
            }

            s.DonVi = donVi;


            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Deleteingredient(string maNL)
        {
            var s = db.NguyenLieu.SingleOrDefault(c => c.MaNguyenLieu == maNL);

            db.NguyenLieu.Remove(s);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}   