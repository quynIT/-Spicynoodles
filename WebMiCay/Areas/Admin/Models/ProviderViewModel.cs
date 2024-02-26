using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMiCay.Models;


namespace WebMiCay.Areas.Admin.Models
{
    public class ProviderViewModel
    {
        public string MaNCC { get; set; }
        public string TenNCC { get; set; }
        public string SDTNCC { get; set; }
        public string DiaChiNCC { get; set; }
        public string MaNguyenLieu { get; set; }
        public DateTime NgayNhap { get; set; }
    }
}
