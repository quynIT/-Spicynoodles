using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Mvc;
using WebMiCay.Areas.Admin.Models;

namespace WebMiCay.Areas.Admin.Controllers
{
    public class ProviderController : Controller
    {
        QuanLyQuanMiCayEntities db = new QuanLyQuanMiCayEntities();
        public ActionResult Index(string searchString, string sortOrder, string genderFilter)
        {
            var data = (from ncc in db.NhaCungCap
                        select new ProviderViewModel
                        {
                            MaNCC = ncc.MaNCC,
                            TenNCC = ncc.TenNCC,
                            SDTNCC = ncc.SDTNCC,
                            DiaChiNCC = ncc.DiaChiNCC,
                        });


            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                data = data.Where(b => b.TenNCC.ToLower().Contains(searchString));
            }

            if (!String.IsNullOrEmpty(genderFilter))
            {
                data = data.Where(b => b.MaNguyenLieu == genderFilter);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    data = data.OrderByDescending(b => b.TenNCC);
                    break;
                default:
                    data = data.OrderBy(b => b.TenNCC);
                    break;
            }
            return View(data.ToList());
        }

        public ActionResult Addprovider()
        {
            return View();
        }
        // Action thêm mới nhân viên
        [HttpPost]
        public ActionResult Addprovider(string TenNCC, string SDTNCC, string DiaChiNCC)
        {
            try
            {
                db.Database.ExecuteSqlCommand("EXEC sp_InsertNhaCC @p0, @p1, @p2",
                parameters: new[]
                {
                    new SqlParameter("@p0", TenNCC),
                    new SqlParameter("@p1", SDTNCC),
                    new SqlParameter("@p2", DiaChiNCC),
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
        public ActionResult Updateprovider(string tenNCC)
        {
            var model = new ProviderViewModel
            {
                TenNCC = tenNCC
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Updateprovider(string maNCC, string tenNCC, string sdtNCC, string diachiNCC)
        {
            var s = db.NhaCungCap.SingleOrDefault(c => c.MaNCC == maNCC);

            if (tenNCC != null)
            {
                s.TenNCC = tenNCC;
            }


            if (!string.IsNullOrEmpty(sdtNCC))
            {
                s.SDTNCC = sdtNCC;
            }

            if (!string.IsNullOrEmpty(diachiNCC))
            {
                s.DiaChiNCC = diachiNCC;
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }


        public ActionResult Deleteprovider(string maNCC)
        {
            var s = db.NhaCungCap.SingleOrDefault(c => c.MaNCC == maNCC);

            db.NhaCungCap.Remove(s);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}