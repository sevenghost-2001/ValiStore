using System;
using System.Collections.Generic;

namespace ValiStore.Models
{
    public partial class TLoaiDt
    {
        public TLoaiDt()
        {
            TDanhMucSps = new HashSet<TDanhMucSp>();
        }

        public string MaDt { get; set; } = null!;
        public string? TenLoai { get; set; }

        public virtual ICollection<TDanhMucSp> TDanhMucSps { get; set; }
    }
}
