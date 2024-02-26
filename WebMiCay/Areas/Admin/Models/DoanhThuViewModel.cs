using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMiCay.Areas.Admin.Models
{
    public class DoanhThuViewModel
    {
        public string TenThang { get; set; }
        public int Thang { get; set; }
        public decimal DoanhThu { get; set; }
    }
}