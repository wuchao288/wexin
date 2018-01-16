namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Sales;
    using Hidistro.Messages;
    using Hidistro.SqlDal.Commodities;
    using Hidistro.SqlDal.Members;
    using Hidistro.SqlDal.Orders;
    using Hidistro.SqlDal.Promotions;
    using Hidistro.SqlDal.Sales;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Web.Caching;
    using System.Linq;
    using Hidistro.Core.Entities;

    public static class MemberProcessor
    {
        public static int AddShippingAddress(ShippingAddressInfo shippingAddress)
        {
            ShippingAddressDao dao = new ShippingAddressDao();
            int shippingId = dao.AddShippingAddress(shippingAddress);
            if (dao.SetDefaultShippingAddress(shippingId, Globals.GetCurrentMemberUserId()))
            {
                return 1;
            }
            return 0;
        }

        public static bool ConfirmOrderFinish(OrderInfo order)
        {
            bool flag = false;
            if (order.CheckAction(OrderActions.BUYER_CONFIRM_GOODS))
            {
                order.OrderStatus = OrderStatus.Finished;
                order.FinishDate = new DateTime?(DateTime.Now);
                flag = new OrderDao().UpdateOrder(order, null);
            }
            return flag;
        }

        public static bool CreateMember(MemberInfo member)
        {
            MemberDao dao = new MemberDao();
            if (dao.IsExitOpenId(member.OpenId))
            {
                return dao.SetMemberSessionId(member.SessionId, member.SessionEndTime, member.OpenId);
            }
            return dao.CreateMember(member);
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static bool IsExitOpenId(string openid) {
            MemberDao dao = new MemberDao();
            return dao.IsExitOpenId(openid);
        }

        public static bool Delete(int userId)
        {
            bool flag = new MemberDao().Delete(userId);
            if (flag)
            {
                HiCache.Remove(string.Format("DataCache-Member-{0}", userId));
            }
            return flag;
        }

        public static bool DelShippingAddress(int shippingid, int userid)
        {
            return new ShippingAddressDao().DelShippingAddress(shippingid, userid);
        }

        public static MemberInfo GetCurrentMember()
        {
            return GetMember(Globals.GetCurrentMemberUserId());
        }

        public static int GetDefaultMemberGrade()
        {
            return new MemberGradeDao().GetDefaultMemberGrade();
        }

        public static ShippingAddressInfo GetDefaultShippingAddress()
        {
            IList<ShippingAddressInfo> shippingAddresses = new ShippingAddressDao().GetShippingAddresses(Globals.GetCurrentMemberUserId());
            foreach (ShippingAddressInfo info in shippingAddresses)
            {
                if (info.IsDefault)
                {
                    return info;
                }
            }
            return null;
        }

        public static MemberInfo GetMember(int userId)
        {
            MemberInfo member = HiCache.Get(string.Format("DataCache-Member-{0}", userId)) as MemberInfo;
            if (member == null)
            {
                member = new MemberDao().GetMember(userId);
                HiCache.Insert(string.Format("DataCache-Member-{0}", userId), member, 360, CacheItemPriority.Normal);
            }
            return member;
        }

        public static int GetMemberCount()
        {
            return new MemberDao().GetMemberCount();
        }

        public static MemberInfo GetMember(string sessionId)
        {
            return new MemberDao().GetMember(sessionId);
        }

        public static MemberInfo GetMembers(string openid)
        {
            return new MemberDao().GetMemberByOpenid(openid);
        }

        public static MemberGradeInfo GetMemberGrade(int gradeId)
        {
            return new MemberGradeDao().GetMemberGrade(gradeId);
        }

        public static ShippingAddressInfo GetShippingAddress(int shippingId)
        {
            return new ShippingAddressDao().GetShippingAddress(shippingId);
        }

        public static int GetShippingAddressCount()
        {
            return new ShippingAddressDao().GetShippingAddresses(Globals.GetCurrentMemberUserId()).Count;
        }

        public static IList<ShippingAddressInfo> GetShippingAddresses()
        {
            return new ShippingAddressDao().GetShippingAddresses(Globals.GetCurrentMemberUserId());
        }

        public static DataTable GetUserCoupons(int userId, int useType = 0)
        {
            return new CouponDao().GetUserCoupons(userId, useType);
        }

        public static int GetUserHistoryPoint(int userId)
        {
            return new PointDetailDao().GetHistoryPoint(userId);
        }

        public static MemberInfo GetusernameMember(string username)
        {
            return new MemberDao().GetusernameMember(username);
        }

        public static DataSet GetUserOrder(int userId, OrderQuery query)
        {
            return new OrderDao().GetUserOrder(userId, query);
        }

        public static int GetUserOrderCount(int userId, OrderQuery query)
        {
            return new OrderDao().GetUserOrderCount(userId, query);
        }

        public static void RemoveUserCache(int userId)
        {
            HiCache.Remove(string.Format("DataCache-Member-{0}", userId));
        }

        public static bool SetDefaultShippingAddress(int shippingId, int UserId)
        {
            return new ShippingAddressDao().SetDefaultShippingAddress(shippingId, UserId);
        }

        public static bool SetMemberSessionId(string sessionId, DateTime sessionEndTime, string openId)
        {
            return new MemberDao().SetMemberSessionId(sessionId, sessionEndTime, openId);
        }

        public static bool UpdateMember(MemberInfo member)
        {
            HiCache.Remove(string.Format("DataCache-Member-{0}", member.UserId));
            return new MemberDao().Update(member);
        }

        public static bool UpdateShippingAddress(ShippingAddressInfo shippingAddress)
        {
            return new ShippingAddressDao().UpdateShippingAddress(shippingAddress);
        }

        public static bool UserPayOrder(OrderInfo order)
        {
            OrderDao dao = new OrderDao();
            order.OrderStatus = OrderStatus.BuyerAlreadyPaid;
            order.PayDate = new DateTime?(DateTime.Now);
            bool flag = dao.UpdateOrder(order, null);
            if (flag)
            {
                dao.UpdatePayOrderStock(order.OrderId);
                foreach (LineItemInfo info in order.LineItems.Values)
                {
                    ProductDao dao2 = new ProductDao();
                    ProductInfo productDetails = dao2.GetProductDetails(info.ProductId);
                    productDetails.SaleCounts += info.Quantity;
                    productDetails.ShowSaleCounts += info.Quantity;
                    dao2.UpdateProduct(productDetails, null);
                }
                MemberInfo member = GetMember(order.UserId);
                if (member == null)
                {
                    return flag;
                }
                MemberDao dao3 = new MemberDao();
                PointDetailInfo point = new PointDetailInfo {
                    OrderId = order.OrderId,
                    UserId = member.UserId,
                    TradeDate = DateTime.Now,
                    TradeType = PointTradeType.Bounty,
                    Increased = new int?(order.Points),
                    Points = order.Points + member.Points
                };
                if ((point.Points > 0x7fffffff) || (point.Points < 0))
                {
                    point.Points = 0x7fffffff;
                }
                PointDetailDao dao4 = new PointDetailDao();
                dao4.AddPointDetail(point);
                member.Expenditure += order.GetTotal();
                member.OrderNumber++;
                dao3.Update(member);
                Messenger.OrderPayment(member, order.OrderId, order.GetTotal());
                int historyPoint = dao4.GetHistoryPoint(member.UserId);
                List<MemberGradeInfo> memberGrades = new MemberGradeDao().GetMemberGrades() as List<MemberGradeInfo>;
                foreach (MemberGradeInfo info5 in from item in memberGrades
                    orderby item.Points descending
                    select item)
                {
                    if (member.GradeId == info5.GradeId)
                    {
                        return flag;
                    }
                    if (info5.Points <= historyPoint)
                    {
                        member.GradeId = info5.GradeId;
                        dao3.Update(member);
                        return flag;
                    }
                }
            }
            return flag;
        }
    }
}

