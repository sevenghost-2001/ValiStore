using System;
using System.Collections.Generic;

namespace ValiStore.Models
{
    public partial class TUser
    {
        public TUser()
        {
            TKhachHangs = new HashSet<TKhachHang>();
            TNhanViens = new HashSet<TNhanVien>();
        }

        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public byte? LoaiUser { get; set; }

        public virtual ICollection<TKhachHang> TKhachHangs { get; set; }
        public virtual ICollection<TNhanVien> TNhanViens { get; set; }
    }
}
