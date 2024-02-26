using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMiCay.Models
{
    public class BookingViewModel
    {
        public string MaKH { get; set; }
        public string TenKH { get; set; }
        public string SDT { get; set; }
        public string MaTaiKhoanKhach { get; set; }
        public string TenTaiKhoanKhach { get; set; }
        public string MaLichDat { get; set; }
        public DateTime NgayDat { get; set; }
        public TimeSpan GioDat { get; set; }
        public byte SoBan { get; set; }
        public string TinhTrang { get; set; }
        public string MaNV { get; set; }
    }
}