using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMiCay.Areas.Admin.Models;

namespace WebMiCay.Areas.Admin.Models
{
    public class DishViewModel
    {
        public string MaMon { get; set; }
        public string TenMon { get; set; }
        public decimal GiaCa { get; set; }
        public string AnhMon { get; set; }
        public string MaTheLoai { get; set;}
        public string TenTheLoai { get; set;}

        // Ds này để hiển thị ra các tên thể loại món cho combo box
        public List<LoaiMon> DSTheLoaiMon { get; set; }
    }
}