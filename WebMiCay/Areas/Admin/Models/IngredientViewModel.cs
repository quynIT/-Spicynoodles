using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMiCay.Models;

namespace WebMiCay.Areas.Admin.Models
{
    public class IngredientViewModel
    {
        public string MaNguyenLieu { get; set; }
        public string TenNguyenLieu { get; set; }
        public decimal GiaNguyenLieu { get; set; }
        public string DonVi { get; set; }
        public int? SoLuong { get; set; }
        public DateTime? NgayNhap { get; set; }

    }
}