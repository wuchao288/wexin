namespace Hidistro.Entities.Orders
{
    using System;
    using System.Runtime.CompilerServices;

    public class LineItemInfo
    {
        public decimal GetSubTotal()
        {
            return (this.ItemAdjustedPrice * this.Quantity);
        }

        public decimal GetSubCommission()
        {
            return ((this.ItemAdjustedPrice - this.ItemCostPrice) * this.Quantity);
        }

        public decimal ItemAdjustedPrice { get; set; }

        public decimal ItemCostPrice { get; set; }

        public string ItemDescription { get; set; }

        public decimal ItemListPrice { get; set; }

        public decimal ItemWeight { get; set; }

        public int ProductId { get; set; }

        public int PromotionId { get; set; }

        public string PromotionName { get; set; }

        public int Quantity { get; set; }

        public int ShipmentQuantity { get; set; }

        public string SKU { get; set; }

        public string SKUContent { get; set; }

        public string SkuId { get; set; }

        public string ThumbnailsUrl { get; set; }
    }
}

