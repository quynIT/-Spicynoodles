using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMiCay.Models
{
    public class CartItem
    {
        public string MaMon { get; set; }
        public string TenMon { get; set; }
        public string AnhMon { get; set; }
        public decimal GiaCa { get; set; }
        public string MaTheLoai { get; set; }
        public int SoLuongMon { get; set; }
    }
}