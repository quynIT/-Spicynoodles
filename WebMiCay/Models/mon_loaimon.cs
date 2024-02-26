using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMiCay.Models
{
    public class mon_loaimon
    {
        public string MaMon { get; set; }
        public string TenMon { get; set; }
        public decimal GiaCa { get; set; }
        public string AnhMon { get; set; }
        public string MaTheLoai { get; set; }
        public string TenTheLoai { get; set; }

        public List<LoaiMon> LoaiMon { get; set; }
        public List<Mon> Mon { get; set; }
    }
}