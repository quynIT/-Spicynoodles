using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMiCay.Areas.Admin.Models
{
    public class StaffViewModel
    {
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        public string SDTNV { get; set; }
        public DateTime NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public string GioiTinh { get; set; }
        public string MaVaiTro { get; set; }
        public string TenVaiTro { get; set; }
        public string MaTaiKhoanNV { get; set; }
        public string TenTaiKhoanNV { get; set; }
        public string MatKhau { get; set; }
        public DateTime? NgayTao { get; set; }
    }
}