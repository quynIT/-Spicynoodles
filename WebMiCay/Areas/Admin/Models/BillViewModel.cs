using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMiCay.Areas.Admin.Models
{
    public class BillViewModel
    {
        public string MaHD { get; set; }
        public DateTime? NgayDat { get; set; }
        public decimal? TongGia { get; set; }
        public string TinhTrang { get; set; }
        public string MaNV { get; set; }
        public int SoLuongMon { get; set; }
        public string MaMon { get; set; }
        public string TenMon { get; set; }
        public decimal GiaCa { get; set; }
        public string MaTheLoai { get; set; }
        public string MaKH { get; set; }
        public string TenKH { get;set; }
        public string SDT { get; set; }
        public string MaTaiKhoanKhach { get; set; }
        public string TenTaiKhoanKhach { get;set; }

        public List<ChiTietHoaDonViewModel> ChiTietHoaDon { get; set; }
    }

    public class ChiTietHoaDonViewModel
    {
        public string MaMon { get; set; }
        public string MaHD { get; set; }
        public int? SoLuongMon { get; set; }
        public string TenMon { get; set; }
        public decimal GiaMon { get; set; }
    }
}