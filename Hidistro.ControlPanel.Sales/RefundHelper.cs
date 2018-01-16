namespace Hidistro.ControlPanel.Sales
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Entities;
    using Hidistro.Entities.Orders;
    using Hidistro.SqlDal;
    using System;
    using System.Runtime.InteropServices;
    using System.Transactions;

    public static class RefundHelper
    {
        public static void AddRefund(RefundInfo refundInfo)
        {
            new RefundDao().AddRefund(refundInfo);
        }

        public static bool EnsureRefund(string orderId, string Operator, string adminRemark, int refundType, bool accept)
        {
            RefundDao dao = new RefundDao();
            RefundInfo byOrderId = dao.GetByOrderId(orderId);
            byOrderId.Operator = Operator;
            byOrderId.AdminRemark = adminRemark;
            byOrderId.HandleTime = DateTime.Now;
            byOrderId.HandleStatus = accept ? RefundInfo.Handlestatus.Refunded : RefundInfo.Handlestatus.Refused;
            byOrderId.OrderId = orderId;
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
            using (TransactionScope scope = new TransactionScope())
            {
                OrderHelper.SetOrderState(orderId, accept ? OrderStatus.Refunded : OrderStatus.BuyerAlreadyPaid);
                dao.UpdateByOrderId(byOrderId);
                if (orderInfo.GroupBuyId > 0)
                {
                    GroupBuyHelper.RefreshGroupFinishBuyState(orderInfo.GroupBuyId);
                }
                scope.Complete();
            }
            return true;
        }

        public static void GetRefundType(string orderId, out int refundType, out string remark)
        {
            new RefundDao().GetRefundType(orderId, out refundType, out remark);
        }

        public static void GetRefundTypeFromReturn(string orderId, out int refundType, out string remark)
        {
            new RefundDao().GetRefundTypeFromReturn(orderId, out refundType, out remark);
        }
    }
}

