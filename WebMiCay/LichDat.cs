//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebMiCay
{
    using System;
    using System.Collections.Generic;
    
    public partial class LichDat
    {
        public string MaLichDat { get; set; }
        public System.DateTime NgayDat { get; set; }
        public System.TimeSpan GioDat { get; set; }
        public byte SoBan { get; set; }
        public string TinhTrang { get; set; }
        public string MaKH { get; set; }
        public string MaNV { get; set; }
    
        public virtual KhachHang KhachHang { get; set; }
        public virtual NhanVien NhanVien { get; set; }
    }
}
