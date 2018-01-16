using Hishop.Weixin.MP.Domain;
using Hishop.Weixin.MP.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
namespace Hishop.Weixin.MP.Api
{
    public class TokenApi
    {
        public string AppId
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("AppId");
            }
        }
        public string AppSecret
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("AppSecret");
            }
        }
        public static string GetToken_Message(string appid, string secret)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret);
            string text = new WebUtils().DoGet(url, null);
            if (text.Contains("access_token"))
            {
                text = new JavaScriptSerializer().Deserialize<Token>(text).access_token;
            }
            return text;
        }

        static string AccessToken = null;
        //static DateTime AccessTokenTime;
        public static string GetToken(string appid, string secret)
        {
            //if (AccessTokenTime.Year > 2000 || (DateTime.Now - AccessTokenTime).TotalMinutes < 1)
            //{
            //    //直接返回 token
            //    return AccessToken;
            //}

            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret);

            AccessToken = new WebUtils().DoGet(url, null);
            //AccessTokenTime = DateTime.Now;

            return AccessToken;
        }

        public static string GetToken2(string appid, string secret)
        {
            string obj22 = TokenApi.GetToken(appid, secret);
            JObject obj33 = JsonConvert.DeserializeObject(obj22) as JObject;

            string token = obj33["access_token"].ToString();

            return token;
        }


        static string Ticket = null;
        static DateTime TicketTime;
        public static string GetTicket(string token)
        {
            if (TicketTime.Year > 2000 || (DateTime.Now - TicketTime).TotalMinutes < 100)
            {
                //直接返回 token
                return Ticket;
            }

            string url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", token);

            Ticket = new WebUtils().DoGet(url, null);
            TicketTime = DateTime.Now;

            return Ticket;
        }


        public static string AddNews(string appid, string secret, string path)
        {
            string token = GetToken(appid, secret);
            //Utils.LogWriter.SaveLog("上传素材 token：" + token);

            string media_id = "";

            JObject obj2 = JsonConvert.DeserializeObject(token) as JObject;

            string url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type=image", obj2["access_token"].ToString());


            Dictionary<string, string> pvars = new Dictionary<string, string>();

            WebClient c = new WebClient();

      
            c.Headers.Add("Content-Disposition", "attachment; name=media");

            //c.Headers.Add("Content-Disposition", string.Format("attachment; name=media;filelength=\"{1}\";filename=\"{0}\"", "QRCode_bg_29", fs.Length));
            //c.Headers.Add("Content-Type", "application/octet-stream");

            try
            {
                //Utils.LogWriter.SaveLog("上传素材 url：" + url);
                //Utils.LogWriter.SaveLog("上传素材 path：" + path);
                byte[] result = c.UploadFile(url, "POST", path);

                string strres = Encoding.Default.GetString(result);

                //Utils.LogWriter.SaveLog("上传素材：" + strres);

                JObject obj3 = JsonConvert.DeserializeObject(strres) as JObject;

                media_id = obj3["media_id"].ToString();
            }
            catch (Exception ex)
            {
                Utils.LogWriter.SaveLog("上传素材失败：" + ex.Message);
            }

            return media_id;
        }

        /// <summary>
        /// 生成带参数的二维码名片
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string CreateQRCode(string appid, string secret, int scene_id)
        {
            string obj2 = GetToken(appid, secret);

            JObject obj3 = JsonConvert.DeserializeObject(obj2) as JObject;

            string token = obj3["access_token"].ToString();

            string url = string.Format("https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}", token);
            string param = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + scene_id + "}}}";

            //{"ticket":"gQH47joAAAAAAAAAASxodHRwOi8vd2VpeGluLnFxLmNvbS9xL2taZ2Z3TVRtNzJXV1Brb3ZhYmJJAAIEZ23sUwMEmm3sUw==","expire_seconds":60,"url":"http:\/\/weixin.qq.com\/q\/kZgfwMTm72WWPkovabbI"}
            string text = new WebUtils().DoPost(url, param);
            JObject obj4 = JsonConvert.DeserializeObject(text) as JObject;
            string ticket = obj4["ticket"].ToString();

            string url2 = string.Format("https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}", ticket);


            return url2;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="secret"></param>
        /// <param name="open_id"></param>
        /// <returns></returns>
        public static JObject GetUserInfo(string appid, string secret, string open_id)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + GetToken2(appid, secret) + "&openid=" + open_id + "&lang=zh_CN";
            string responseResult = new WebUtils().DoGet(url, null);

            JObject obj2 = JsonConvert.DeserializeObject(responseResult) as JObject;

            return obj2;
        }
    }
}
