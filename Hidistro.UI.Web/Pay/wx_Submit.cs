namespace Hidistro.UI.Web.Pay
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hishop.Weixin.Pay;
    using Hishop.Weixin.Pay.Domain;
    using System;
    using System.Web.UI;

    public class wx_Submit : Page
    {
        public string pay_json = "";

        public wx_Submit()
        {
            this.pay_json ="";
        }

 


        public string ConvertPayJson(PayRequestInfo req)
        {


            //string packageValue = "";
            //packageValue += " \"appId\": \"" + req.appId + "\", ";
            //packageValue += " \"timeStamp\": \"" + req.timeStamp + "\", ";
            //packageValue += " \"nonceStr\": \"" + req.nonceStr + "\", ";
            //packageValue += " \"package\": \"" + req.package + "\", ";
            //packageValue += " \"signType\": \"MD5\", ";
            //packageValue += " \"paySign\": \"" + req.paySign + "\"";
            //return packageValue;

            string str = "{";
            return (((((((str + "\"appId\":\"" + req.appId + "\",") + "\"timeStamp\":\"" + req.timeStamp + "\",") + "\"nonceStr\":\"" + req.nonceStr + "\",") + "\"package\":\"" + req.package + "\",") + "\"signType\":\"" + req.signType + "\",") + "\"paySign\":\"" + req.paySign + "\"") + "}");
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            string str = base.Request.QueryString.Get("orderId");
            if (!string.IsNullOrEmpty(str))
            {
                OrderInfo orderInfo = OrderHelper.GetOrderInfo(str);
                if (orderInfo != null)
                {
                    PackageInfo package = new PackageInfo
                    {
                        Body = orderInfo.OrderId,
                        NotifyUrl = string.Format("http://{0}/pay/wx_Pay.aspx", base.Request.Url.Host),
                        OutTradeNo = orderInfo.OrderId,
                        TotalFee = (int)(orderInfo.GetTotal() * 100M)
                    };
                    if (package.TotalFee < 1M)
                    {
                        package.TotalFee = 1M;
                    }
                    string openId = "";
                    MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                    if (currentMember != null)
                    {
                        openId = currentMember.OpenId;
                    }
                    package.OpenId = openId;
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    PayRequestInfo req = new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey).BuildPayRequest(package);
                    this.pay_json = this.ConvertPayJson(req);
                }
            }
        }



    }
}

