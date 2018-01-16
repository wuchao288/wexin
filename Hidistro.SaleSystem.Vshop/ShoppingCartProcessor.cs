namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.SqlDal.Commodities;
    using Hidistro.SqlDal.Sales;
    using System;

    public static class ShoppingCartProcessor
    {
        public static void AddLineItem(string skuId, int quantity)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (quantity <= 0)
            {
                quantity = 1;
            }
            new ShoppingCartDao().AddLineItem(currentMember, skuId, quantity);
        }

        public static void ClearShoppingCart()
        {
            new ShoppingCartDao().ClearShoppingCart(Globals.GetCurrentMemberUserId());
        }

        public static ShoppingCartInfo GetGroupBuyShoppingCart(int groupbuyId, string productSkuId, int buyAmount)
        {
            ShoppingCartItemInfo info5;
            ShoppingCartInfo info = new ShoppingCartInfo();
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            ShoppingCartItemInfo info3 = new ShoppingCartDao().GetCartItemInfo(currentMember, productSkuId, buyAmount);
            if (info3 == null)
            {
                return null;
            }
            GroupBuyInfo groupBuy = GroupBuyBrowser.GetGroupBuy(groupbuyId);
            if (((groupBuy == null) || (groupBuy.StartDate > DateTime.Now)) || (groupBuy.Status != GroupBuyStatus.UnderWay))
            {
                return null;
            }
            int count = groupBuy.Count;
            decimal price = groupBuy.Price;

            //info5 = new ShoppingCartItemInfo {
            //    SkuId = info3.SkuId,
            //    ProductId = info3.ProductId,
            //    SKU = info3.SKU,
            //    Name = info3.Name,
            //    MemberPrice = info5.AdjustedPrice = price,
            //    SkuContent = info3.SkuContent,
            //    Quantity = info5.ShippQuantity = buyAmount,
            //    Weight = info3.Weight,
            //    ThumbnailUrl40 = info3.ThumbnailUrl40,
            //    ThumbnailUrl60 = info3.ThumbnailUrl60,
            //    ThumbnailUrl100 = info3.ThumbnailUrl100
            //};
            info5 = new ShoppingCartItemInfo();
            
                info5.SkuId = info3.SkuId;
                info5.ProductId = info3.ProductId;
                info5.SKU = info3.SKU;
                info5.Name = info3.Name;
                info5.MemberPrice = info5.AdjustedPrice = price;
               info5. SkuContent = info3.SkuContent;
               info5.Quantity = info5.ShippQuantity = buyAmount;
               info5. Weight = info3.Weight;
               info5. ThumbnailUrl40 = info3.ThumbnailUrl40;
                info5.ThumbnailUrl60 = info3.ThumbnailUrl60;
                info5.ThumbnailUrl100 = info3.ThumbnailUrl100;
             


            info.LineItems.Add(info5);
            return info;
        }

        public static ShoppingCartInfo GetShoppingCart()
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                return null;
            }
            ShoppingCartInfo shoppingCart = new ShoppingCartDao().GetShoppingCart(currentMember);
            if (shoppingCart.LineItems.Count == 0)
            {
                return null;
            }
            return shoppingCart;
        }

        /// <summary>
        /// 获得购物车信息
        /// </summary>
        /// <param name="productSkuId"></param>
        /// <param name="buyAmount"></param>
        /// <returns></returns>
        public static ShoppingCartInfo GetShoppingCart(string productSkuId, int buyAmount)
        {
            ShoppingCartInfo info = new ShoppingCartInfo();
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            ShoppingCartItemInfo item = new ShoppingCartDao().GetCartItemInfo(currentMember, productSkuId, buyAmount);
            if (item == null)
            {
                return null;
            }
            info.LineItems.Add(item);
            return info;
        }

        public static int GetSkuStock(string skuId)
        {
            return new SkuDao().GetSkuItem(skuId).Stock;
        }

        public static void RemoveLineItem(string skuId)
        {
            new ShoppingCartDao().RemoveLineItem(Globals.GetCurrentMemberUserId(), skuId);
        }

        public static void UpdateLineItemQuantity(string skuId, int quantity)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (quantity <= 0)
            {
                RemoveLineItem(skuId);
            }
            new ShoppingCartDao().UpdateLineItemQuantity(currentMember, skuId, quantity);
        }
    }
}

