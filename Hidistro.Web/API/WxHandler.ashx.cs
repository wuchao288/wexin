using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.Web.API;
using Hishop.Weixin.MP.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Hidistro.Web.API
{
    // URL：/API/WxHandler.ashx?url=

    /// <summary>
    /// WxHandler 的摘要说明
    /// </summary>
    public class WxHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);

            string url = context.Request.Form["url"];

            //token
            string token = TokenApi.GetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
            JObject obj2 = JsonConvert.DeserializeObject(token) as JObject;

            //ticket
            string ticket = TokenApi.GetTicket(obj2["access_token"].ToString());
            JObject obj3 = JsonConvert.DeserializeObject(ticket) as JObject;

            //noncestr
            string noncestr = DateTime.Now.ToFileTime().ToString();

            int timestamp = DateTime.Now.Second;

            string str = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}",
                obj3["ticket"].ToString(), noncestr, timestamp, url);

            //signature
            string signature = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1");

            Hashtable result = new Hashtable();
            result["noncestr"] = noncestr;
            result["signature"] = signature;
            result["timestamp"] = timestamp.ToString();

            //Utils.LogWriter.SaveLog("ticket：" + ticket);
            //Utils.LogWriter.SaveLog("noncestr：" + noncestr);
            //Utils.LogWriter.SaveLog("timestamp：" + timestamp);
            //Utils.LogWriter.SaveLog("url：" + url);
            //Utils.LogWriter.SaveLog("str：" + str);

//            string xml = @"<xml><ToUserName><![CDATA[gh_299ece9e014f]]></ToUserName>
//                        <FromUserName><![CDATA[oRQ-RswyYjEInIDb6D0q4CLr-TBo]]></FromUserName>
//                        <CreateTime>1437031621</CreateTime>
//                        <MsgType><![CDATA[event]]></MsgType>
//                        <Event><![CDATA[subscribe]]></Event>
//                        <EventKey><![CDATA[qrscene_117]]></EventKey>
//                        <Ticket><![CDATA[gQE78DoAAAAAAAAAASxodHRwOi8vd2VpeGluLnFxLmNvbS9xL1JrZ3h3SS1tYUJZUnlxZE5GV2FCAAIEI1CnVQMEgDoJAA==]]></Ticket>
//                        </xml>";
            //二维码
//            string xml = @"<xml><ToUserName><![CDATA[gh_299ece9e014f]]></ToUserName>
//                <FromUserName><![CDATA[oRQ-RswyYjEInIDb6D0q4CLr-TBo]]></FromUserName>
//                <CreateTime>1437037352</CreateTime>
//                <MsgType><![CDATA[event]]></MsgType>
//                <Event><![CDATA[CLICK]]></Event>
//                <EventKey><![CDATA[13]]></EventKey>
//                </xml>";

            //string test = TokenApi.CreateQRCode(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);

            //CustomMsgHandler handler = new CustomMsgHandler(xml);

            //handler.Execute();

            //string ss = handler.ResponseDocument;

            context.Response.Write(JsonConvert.SerializeObject(result));
        }

        private string GetResponseResult(string url)
        {
            using (HttpWebResponse response = (HttpWebResponse)WebRequest.Create(url).GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}