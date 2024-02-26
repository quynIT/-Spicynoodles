﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMiCay.Models
{
    public class ClientViewModel
    {
        public string MaKH { get; set; }
        public string TenKH { get; set; }
        public string SDT { get; set; }
        public DateTime NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public string GioiTinh { get; set; }
        public string MaTaiKhoanKhach { get; set; }
        public string TenTaiKhoanKhach { get; set; }
        public string MatKhauKhach { get; set; }
        public DateTime? NgayTao { get; set; }

        public KhachHang KhachHang { get; set; }
        public TaiKhoanKhach TaiKhoanKhach { get; set; }
    }
}