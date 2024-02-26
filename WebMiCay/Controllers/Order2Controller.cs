using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMiCay.Areas.Admin.Models;
using WebMiCay.Models;

namespace WebMiCay.Controllers
{
    public class Order2Controller : Controller
    {
        QuanLyQuanMiCayEntities db = new QuanLyQuanMiCayEntities();
        public ActionResult Index(string searchString)
        {
            var categories = db.LoaiMon.ToList();
            var dishes = db.Mon.ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                dishes = dishes.Where(s => s.TenMon.Contains(searchString)).ToList();
            }

            var model = new List<mon_loaimon>();

            foreach (var category in categories)
            {
                var dishesInCategory = dishes.Where(d => d.MaTheLoai == category.MaTheLoai).ToList();

                if (dishesInCategory.Any())
                {
                    foreach (var dish in dishesInCategory)
                    {
                        model.Add(new mon_loaimon
                        {
                            MaMon = dish.MaMon,
                            TenMon = dish.TenMon,
                            GiaCa = dish.GiaCa,
                            AnhMon = dish.AnhMon,
                            MaTheLoai = category.MaTheLoai,
                            TenTheLoai = category.TenTheLoai
                        });
                    }
                }
                else
                {
                    model.Add(new mon_loaimon
                    {
                        MaTheLoai = category.MaTheLoai,
                        TenTheLoai = category.TenTheLoai
                    });
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string itemId, string itemName, string itemImage, decimal itemPrice, string itemCate)
        {
            // Kiểm tra nếu Session["Cart"] không tồn tại, tạo mới
            if (Session["Cart"] == null)
            {
                Session["Cart"] = new List<CartItem>();
            }

            var cartItems = (List<CartItem>)Session["Cart"];

            // Kiểm tra xem món đã tồn tại trong giỏ hàng chưa
            var existingItem = cartItems.FirstOrDefault(item => item.MaMon == itemId);

            if (existingItem != null)
            {
                // Món đã tồn tại trong giỏ hàng, có thể thông báo lỗi ở đây hoặc chuyển hướng về trang cần thiết
                // Ví dụ thông báo lỗi
                return RedirectToAction("Index");
            }
            else
            {
                // Thêm sản phẩm vào giỏ hàng trong Session
                var cartItem = new CartItem
                {
                    MaMon = itemId,
                    TenMon = itemName,
                    AnhMon = itemImage,
                    GiaCa = itemPrice,
                    MaTheLoai = itemCate
                };

                cartItems.Add(cartItem);

                // Cập nhật Session["Cart"]
                Session["Cart"] = cartItems;
            }

            // Chuyển hướng về trang chủ hoặc trang sản phẩm
            return RedirectToAction("Index");
        }
    }
}