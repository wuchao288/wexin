namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.SqlDal.Commodities;
    using Hidistro.SqlDal.Orders;
    using Hidistro.SqlDal.Promotions;
    using Hidistro.SqlDal.Sales;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;

    public static class ShoppingProcessor
    {
        private static object createOrderLocker = new object();

        public static decimal CalcFreight(int regionId, decimal totalWeight, ShippingModeInfo shippingModeInfo)
        {
            decimal price = 0M;
            int topRegionId = RegionHelper.GetTopRegionId(regionId);
            decimal num3 = totalWeight;
            int num4 = 1;
            if ((num3 > shippingModeInfo.Weight) && (shippingModeInfo.AddWeight.HasValue && (shippingModeInfo.AddWeight.Value > 0M)))
            {
                decimal num6 = num3 - shippingModeInfo.Weight;
                if ((num6 % shippingModeInfo.AddWeight) == 0M)
                {
                    num4 = Convert.ToInt32(Math.Truncate((decimal) ((num3 - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value)));
                }
                else
                {
                    num4 = Convert.ToInt32(Math.Truncate((decimal) ((num3 - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value))) + 1;
                }
            }
            if ((shippingModeInfo.ModeGroup == null) || (shippingModeInfo.ModeGroup.Count == 0))
            {
                if ((num3 > shippingModeInfo.Weight) && shippingModeInfo.AddPrice.HasValue)
                {
                    return ((num4 * shippingModeInfo.AddPrice.Value) + shippingModeInfo.Price);
                }
                return shippingModeInfo.Price;
            }
            int? nullable = null;
            foreach (ShippingModeGroupInfo info in shippingModeInfo.ModeGroup)
            {
                foreach (ShippingRegionInfo info2 in info.ModeRegions)
                {
                    if (topRegionId == info2.RegionId)
                    {
                        nullable = new int?(info2.GroupId);
                        break;
                    }
                }
                if (nullable.HasValue)
                {
                    if (num3 > shippingModeInfo.Weight)
                    {
                        price = (num4 * info.AddPrice) + info.Price;
                    }
                    else
                    {
                        price = info.Price;
                    }
                    break;
                }
            }
            if (nullable.HasValue)
            {
                return price;
            }
            if ((num3 > shippingModeInfo.Weight) && shippingModeInfo.AddPrice.HasValue)
            {
                return ((num4 * shippingModeInfo.AddPrice.Value) + shippingModeInfo.Price);
            }
            return shippingModeInfo.Price;
        }

        public static decimal CalcPayCharge(decimal cartMoney, PaymentModeInfo paymentModeInfo)
        {
            if (!paymentModeInfo.IsPercent)
            {
                return paymentModeInfo.Charge;
            }
            return (cartMoney * (paymentModeInfo.Charge / 100M));
        }

        private static void checkCanGroupBuy(int quantity, int groupBuyId)
        {
            GroupBuyInfo groupBuy = GroupBuyBrowser.GetGroupBuy(groupBuyId);
            if (groupBuy.Status != GroupBuyStatus.UnderWay)
            {
                throw new OrderException("当前团购状态不允许购买");
            }
            if ((groupBuy.StartDate > DateTime.Now) || (groupBuy.EndDate < DateTime.Now))
            {
                throw new OrderException("当前不在团购时间范围内");
            }
            int num = groupBuy.MaxCount - groupBuy.SoldCount;
            if (quantity > num)
            {
                throw new OrderException("剩余可购买团购数量不够");
            }
        }

        public static OrderInfo ConvertShoppingCartToOrder(ShoppingCartInfo shoppingCart, bool isCountDown, bool isSignBuy)
        {
            if (shoppingCart.LineItems.Count == 0)
            {
                return null;
            }
            OrderInfo info = new OrderInfo {
                Points = shoppingCart.GetPoint(),
                ReducedPromotionId = shoppingCart.ReducedPromotionId,
                ReducedPromotionName = shoppingCart.ReducedPromotionName,
                ReducedPromotionAmount = shoppingCart.ReducedPromotionAmount,
                IsReduced = shoppingCart.IsReduced,
                SentTimesPointPromotionId = shoppingCart.SentTimesPointPromotionId,
                SentTimesPointPromotionName = shoppingCart.SentTimesPointPromotionName,
                IsSendTimesPoint = shoppingCart.IsSendTimesPoint,
                TimesPoint = shoppingCart.TimesPoint,
                FreightFreePromotionId = shoppingCart.FreightFreePromotionId,
                FreightFreePromotionName = shoppingCart.FreightFreePromotionName,
                IsFreightFree = shoppingCart.IsFreightFree
            };
            string str = string.Empty;
            if (shoppingCart.LineItems.Count > 0)
            {
                foreach (ShoppingCartItemInfo info2 in shoppingCart.LineItems)
                {
                    str = str + string.Format("'{0}',", info2.SkuId);
                }
            }
            if (shoppingCart.LineItems.Count > 0)
            {
                foreach (ShoppingCartItemInfo info2 in shoppingCart.LineItems)
                {
                    LineItemInfo info3 = new LineItemInfo {
                        SkuId = info2.SkuId,
                        ProductId = info2.ProductId,
                        SKU = info2.SKU,
                        Quantity = info2.Quantity,
                        ShipmentQuantity = info2.ShippQuantity,
                        ItemCostPrice = new SkuDao().GetSkuItem(info2.SkuId).CostPrice,
                        ItemListPrice = info2.MemberPrice,
                        ItemAdjustedPrice = info2.AdjustedPrice,
                        ItemDescription = info2.Name,
                        ThumbnailsUrl = info2.ThumbnailUrl40,
                        ItemWeight = info2.Weight,
                        SKUContent = info2.SkuContent,
                        PromotionId = info2.PromotionId,
                        PromotionName = info2.PromotionName
                    };
                    info.LineItems.Add(info3.SkuId, info3);
                }
            }
            info.Tax = 0.00M;
            info.InvoiceTitle = "";
            return info;
        }

        public static bool CreatOrder(OrderInfo orderInfo)
        {
            bool flag = false;
            Database database = DatabaseFactory.CreateDatabase();
            int quantity = orderInfo.LineItems.Sum<KeyValuePair<string, LineItemInfo>>((Func<KeyValuePair<string, LineItemInfo>, int>) (item => item.Value.Quantity));
            lock (createOrderLocker)
            {
                if (orderInfo.GroupBuyId > 0)
                {
                    checkCanGroupBuy(quantity, orderInfo.GroupBuyId);
                }
                using (DbConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    DbTransaction dbTran = connection.BeginTransaction();
                    try
                    {
                        try
                        {
                            if (!new OrderDao().CreatOrder(orderInfo, dbTran))
                            {
                                dbTran.Rollback();
                                return false;
                            }
                            if ((orderInfo.LineItems.Count > 0) && !new LineItemDao().AddOrderLineItems(orderInfo.OrderId, orderInfo.LineItems.Values, dbTran))
                            {
                                dbTran.Rollback();
                                return false;
                            }
                            if (!string.IsNullOrEmpty(orderInfo.CouponCode) && !new CouponDao().AddCouponUseRecord(orderInfo, dbTran))
                            {
                                dbTran.Rollback();
                                return false;
                            }
                            if (orderInfo.GroupBuyId > 0)
                            {
                                GroupBuyDao dao = new GroupBuyDao();
                                GroupBuyInfo groupBuy = dao.GetGroupBuy(orderInfo.GroupBuyId, dbTran);
                                groupBuy.SoldCount += quantity;
                                dao.UpdateGroupBuy(groupBuy, dbTran);
                                dao.RefreshGroupBuyFinishState(orderInfo.GroupBuyId, dbTran);
                            }
                            dbTran.Commit();
                            flag = true;
                        }
                        catch
                        {
                            dbTran.Rollback();
                            throw;
                        }
                        return flag;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return flag;
        }

        public static DataTable GetCoupon(decimal orderAmount)
        {
            return new CouponDao().GetCoupon(orderAmount);
        }

        public static CouponInfo GetCoupon(string couponCode)
        {
            return new CouponDao().GetCouponDetails(couponCode);
        }

        public static OrderInfo GetOrderInfo(string orderId)
        {
            return new OrderDao().GetOrderInfo(orderId);
        }

        public static PaymentModeInfo GetPaymentMode(int modeId)
        {
            return new PaymentModeDao().GetPaymentMode(modeId);
        }

        public static IList<PaymentModeInfo> GetPaymentModes()
        {
            return new PaymentModeDao().GetPaymentModes();
        }

        public static SKUItem GetProductAndSku(MemberInfo member, int productId, string options)
        {
            return new SkuDao().GetProductAndSku(member, productId, options);
        }

        public static ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
        {
            return new ShippingModeDao().GetShippingMode(modeId, includeDetail);
        }

        public static IList<ShippingModeInfo> GetShippingModes()
        {
            return new ShippingModeDao().GetShippingModes();
        }

        public static CouponInfo UseCoupon(decimal orderAmount, string claimCode)
        {
            if (!string.IsNullOrEmpty(claimCode))
            {
                CouponInfo coupon = GetCoupon(claimCode);
                if (coupon != null)
                {
                    decimal? amount;
                    if (coupon.Amount.HasValue)
                    {
                        amount = coupon.Amount;
                        if (!((amount.GetValueOrDefault() > 0M) && amount.HasValue) || (orderAmount < coupon.Amount.Value))
                        {
                        }
                    }
                    if (!(coupon.Amount.HasValue && (!(((amount = coupon.Amount).GetValueOrDefault() == 0M) && amount.HasValue) || (orderAmount <= coupon.DiscountValue))))
                    {
                        return coupon;
                    }
                }
            }
            return null;
        }
    }
}

