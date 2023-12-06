using System.Drawing;
using ValiStore.Models.ProductModels;

namespace ValiStore.Models
{
    public class CartItemModel
    {
        public TDanhMucSp Product { get; set; }
        public int Quantity { get; set; }
        public decimal? Total => Product.GiaLonNhat * Quantity;
        
    }
}
