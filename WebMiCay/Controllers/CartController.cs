using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebMiCay.Models;

namespace WebMiCay.Controllers
{
    public class CartController : Controller
    {
        QuanLyQuanMiCayEntities db = new QuanLyQuanMiCayEntities();
        // Xử lý cập nhật số lượng món vô cart
        [HttpPost]
        public ActionResult UpdateQuantity(string MaMon, int SoLuongMon)
        {
            var cartItems = (List<CartItem>)Session["Cart"];
            var item = cartItems.Find(i => i.MaMon == MaMon);
            if (item != null)
            {
                item.SoLuongMon = SoLuongMon;
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                // Người dùng chưa đăng nhập, chuyển hướng và hiển thị thông báo lỗi
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để sử dụng tính năng giỏ hàng";
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

                if (Session["Cart"] == null)
                {
                    Session["Cart"] = new List<CartItem>();
                }

                // Lấy thông tin giỏ hàng từ Session
                var cartItems = (List<CartItem>)Session["Cart"];

                // Truyền thông tin giỏ hàng đến view để hiển thị
                return View(cartItems);
            }

            return View();
        }

        public ActionResult Checkout()
        {
            var cartItems = (List<CartItem>)Session["Cart"];

            // Call the stored procedure to insert orders
            InsertOrderToDatabase(cartItems);

            // Clear the cart after placing the order
            Session["Cart"] = new List<CartItem>();

            // Redirect to a confirmation or thank you page
            return RedirectToAction("Donhang", "Myaccount");
        }

        private DataTable ConvertToDataTable(List<CartItem> orderDetails)
        {
            DataTable table = new DataTable();
            table.Columns.Add("MaMon", typeof(string));
            table.Columns.Add("SoLuongMon", typeof(int));

            foreach (var orderDetail in orderDetails)
            {
                table.Rows.Add(orderDetail.MaMon, orderDetail.SoLuongMon);
            }

            return table;
        }

        private void InsertOrderToDatabase(List<CartItem> cartItems)
        {
            if (Session["user"] != null)
            {
                var user = Session["user"] as KhachHang;
                if (user != null)
                {
                    var maKH = user.MaKH;
                    // Lấy thông tin cần thiết từ giỏ hàng hoặc từ các nguồn khác
                    var ngayDat = DateTime.Now; // Ngày đặt là ngày hiện tại
                    decimal tongGia = cartItems.Sum(item => item.GiaCa * item.SoLuongMon);

                    DataTable orderDetailsTable = ConvertToDataTable(cartItems);

                    // Tạo tham số để truyền vào stored procedure
                    var parameters = new[]
                    {
                        new SqlParameter("@MaKH", maKH),
                        new SqlParameter("@TongGia", SqlDbType.Decimal) { Value = tongGia },
                        new SqlParameter("@OrderDetails", orderDetailsTable) { TypeName = "dbo.OrderDetailsType" }
                    };

                    // Gọi stored procedure
                    db.Database.ExecuteSqlCommand("EXEC sp_InsertOrder @MaKH, @TongGia, @OrderDetails", parameters);
                }
            }
        }

        [HttpPost]
        public ActionResult RemoveFromCart(string itemId)
        {
            // Kiểm tra nếu Session["Cart"] tồn tại
            if (Session["Cart"] != null)
            {
                var cartItems = (List<CartItem>)Session["Cart"];

                // Tìm kiếm và xóa món khỏi giỏ hàng dựa trên itemId
                var itemToRemove = cartItems.FirstOrDefault(item => item.MaMon == itemId);
                if (itemToRemove != null)
                {
                    cartItems.Remove(itemToRemove);
                }

                // Cập nhật lại Session["Cart"]
                Session["Cart"] = cartItems;
            }

            // Chuyển hướng về trang giỏ hàng
            return RedirectToAction("Index", "Cart");
        }
    }
}