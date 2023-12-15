namespace ValiStore.Models
{
    public class KhachHang_ChiTietHDB
    {
        public THoaDonBan? hoaDonBan { get; set; }
        public TKhachHang? khachHang { get; set; }

        public List<TChiTietHdb>? ChiTietHdbs { get; set; }
    }
}
