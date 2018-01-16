using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Messages;
using Hidistro.SaleSystem.Vshop;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Notify;
using System;
using System.Web.UI;
namespace Hidistro.UI.Web.Pay
{
    public class wx_Pay : System.Web.UI.Page
    {
        protected OrderInfo Order;
        protected string OrderId;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SiteSettings siteSettings = SettingsManager.GetMasterSettings(false);
            NotifyClient client = new NotifyClient(siteSettings.WeixinAppId, siteSettings.WeixinAppSecret, siteSettings.WeixinPartnerID, siteSettings.WeixinPartnerKey, siteSettings.WeixinPaySignKey);
            PayNotify notify = client.GetPayNotify(base.Request.InputStream);
            if (notify == null)
            {
                return;
            }
            this.OrderId = notify.PayInfo.OutTradeNo;
            this.Order = ShoppingProcessor.GetOrderInfo(this.OrderId);
            if (this.Order == null)
            {
                base.Response.Write("success");
                return;
            }
            this.Order.GatewayOrderId = notify.PayInfo.TransactionId;
            this.UserPayOrder();
        }
        private void UserPayOrder()
        {
            if (this.Order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
            {
                base.Response.Write("success");
                return;
            }

            if (this.Order.CheckAction(OrderActions.BUYER_PAY) && MemberProcessor.UserPayOrder(this.Order))
            {
                if (this.Order.UserId != 0 && this.Order.UserId != 1100)
                {
                    MemberInfo meminfo = MemberProcessor.GetMember(this.Order.UserId);
                    if (meminfo != null)
                    {
                        Messenger.OrderPayment(meminfo, this.OrderId, this.Order.GetTotal());
                    }
                }
                this.Order.OnPayment();
                base.Response.Write("success");
            }
        }
    }
}
