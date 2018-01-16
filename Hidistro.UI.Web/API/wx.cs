namespace Hidistro.UI.Web.API
{
    using Hidistro.Core;
    using Hishop.Weixin.MP.Util;
    using System;
    using System.IO;
    using System.Web;
    using System.Xml;
    using System.Xml.Linq;

    public class wx : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            string weixinToken = SettingsManager.GetMasterSettings(false).WeixinToken;
            string signature = request["signature"];
            string nonce = request["nonce"];
            string timestamp = request["timestamp"];
            string s = request["echostr"];


            if (request.HttpMethod == "GET")
            {
                if (CheckSignature.Check(signature, timestamp, nonce, weixinToken))
                {
                    context.Response.Write(s);
                }
                else
                {
                    context.Response.Write("");
                }
                context.Response.End();
            }
            else
            {
                try
                {
                    StreamReader stream = new StreamReader(request.InputStream);
                    string xml = stream.ReadToEnd();
                    Utils.LogWriter.SaveLog("wx xml：" + xml);

                    CustomMsgHandler handler = new CustomMsgHandler(xml);

                    handler.Execute();

                    //Utils.LogWriter.SaveLog("wx ResponseDocument：" + handler.ResponseDocument);

                    context.Response.Write(handler.ResponseDocument);
                }
                catch (Exception ex)
                {
                    Utils.LogWriter.SaveLog("wx：" + ex.Message);
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

