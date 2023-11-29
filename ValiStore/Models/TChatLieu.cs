using System;
using System.Collections.Generic;

namespace ValiStore.Models
{
    public partial class TChatLieu
    {
        public TChatLieu()
        {
            TDanhMucSps = new HashSet<TDanhMucSp>();
        }

        public string MaChatLieu { get; set; } = null!;
        public string? ChatLieu { get; set; }

        public virtual ICollection<TDanhMucSp> TDanhMucSps { get; set; }
    }
}
