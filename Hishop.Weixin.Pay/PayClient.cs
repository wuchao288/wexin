using Hishop.Weixin.Pay.Domain;
using Hishop.Weixin.Pay.Util;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
namespace Hishop.Weixin.Pay
{
	public class PayClient
	{
		public static readonly string Deliver_Notify_Url = "https://api.weixin.qq.com/pay/delivernotify";
		public static readonly string prepay_id_Url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
		private PayAccount _payAccount;
		public PayClient(string appId, string appSecret, string partnerId, string partnerKey, string paySignKey)
		{
			this._payAccount = new PayAccount
			{
				AppId = appId,
				AppSecret = appSecret,
				PartnerId = partnerId,
				PartnerKey = partnerKey,
				PaySignKey = paySignKey
			};
		}
		public PayClient(PayAccount account) : this(account.AppId, account.AppSecret, account.PartnerId, account.PartnerKey, account.PaySignKey)
		{
		}
		internal string BuildPackage(PackageInfo package)
		{
			PayDictionary payDictionary = new PayDictionary();
			payDictionary.Add("appid", this._payAccount.AppId);
			payDictionary.Add("mch_id", this._payAccount.PartnerId);
			payDictionary.Add("device_info", "");
			payDictionary.Add("nonce_str", Utils.CreateNoncestr());
			payDictionary.Add("body", package.Body);
			payDictionary.Add("attach", "");
			payDictionary.Add("out_trade_no", package.OutTradeNo);
			payDictionary.Add("total_fee", (int)package.TotalFee);
			payDictionary.Add("spbill_create_ip", package.SpbillCreateIp);
			payDictionary.Add("time_start", package.TimeExpire);
			payDictionary.Add("time_expire", "");
			payDictionary.Add("goods_tag", package.GoodsTag);
			payDictionary.Add("notify_url", package.NotifyUrl);
			payDictionary.Add("trade_type", "JSAPI");
			payDictionary.Add("openid", package.OpenId);
			payDictionary.Add("product_id", "");
			string sign = SignHelper.SignPackage(payDictionary, this._payAccount.PartnerKey);
			string text = this.GetPrepay_id(payDictionary, sign);
			if (text.Length > 64)
			{
				text = "";
			}
			return string.Format("prepay_id=" + text, new object[0]);
		}
		public PayRequestInfo BuildPayRequest(PackageInfo package)
		{
			PayRequestInfo payRequestInfo = new PayRequestInfo();
			payRequestInfo.appId = this._payAccount.AppId;
			payRequestInfo.package = this.BuildPackage(package);
			payRequestInfo.timeStamp = Utils.GetCurrentTimeSeconds().ToString();
			payRequestInfo.nonceStr = Utils.CreateNoncestr();
			payRequestInfo.paySign = SignHelper.SignPay(new PayDictionary
			{

				{
					"appId",
					this._payAccount.AppId
				},

				{
					"timeStamp",
					payRequestInfo.timeStamp
				},

				{
					"package",
					payRequestInfo.package
				},

				{
					"nonceStr",
					payRequestInfo.nonceStr
				},

				{
					"signType",
					"MD5"
				}
			}, this._payAccount.PartnerKey);
			return payRequestInfo;
		}
		public bool DeliverNotify(DeliverInfo deliver)
		{
			string token = Utils.GetToken(this._payAccount.AppId, this._payAccount.AppSecret);
			return this.DeliverNotify(deliver, token);
		}
		public bool DeliverNotify(DeliverInfo deliver, string token)
		{
			PayDictionary payDictionary = new PayDictionary();
			payDictionary.Add("appid", this._payAccount.AppId);
			payDictionary.Add("openid", deliver.OpenId);
			payDictionary.Add("transid", deliver.TransId);
			payDictionary.Add("out_trade_no", deliver.OutTradeNo);
			payDictionary.Add("deliver_timestamp", Utils.GetTimeSeconds(deliver.TimeStamp));
			payDictionary.Add("deliver_status", deliver.Status ? 1 : 0);
			payDictionary.Add("deliver_msg", deliver.Message);
			deliver.AppId = this._payAccount.AppId;
			deliver.AppSignature = SignHelper.SignPay(payDictionary, "");
			payDictionary.Add("app_signature", deliver.AppSignature);
			payDictionary.Add("sign_method", deliver.SignMethod);
			string data = JsonConvert.SerializeObject(payDictionary);
			string url = string.Format("{0}?access_token={1}", PayClient.Deliver_Notify_Url, token);
			string text = new WebUtils().DoPost(url, data);
			return !string.IsNullOrEmpty(text) && text.Contains("ok");
		}
		internal string GetPrepay_id(PayDictionary dict, string sign)
		{
			dict.Add("sign", sign);
			string value = SignHelper.BuildQuery(dict, false);
			string text = SignHelper.BuildXml(dict, false);
			string text2 = PayClient.PostData(PayClient.prepay_id_Url, text);
			DataTable dataTable = new DataTable();
			dataTable.TableName = "log";
			dataTable.Columns.Add(new DataColumn("OperTime"));
			dataTable.Columns.Add(new DataColumn("Info"));
			dataTable.Columns.Add(new DataColumn("param"));
			dataTable.Columns.Add(new DataColumn("query"));
			DataRow dataRow = dataTable.NewRow();
			dataRow["OperTime"] = DateTime.Now.ToString();
			dataRow["Info"] = text2;
			dataRow["param"] = text;
			dataRow["query"] = value;
			dataTable.Rows.Add(dataRow);
			dataTable.WriteXml(HttpContext.Current.Request.MapPath("/PrepayID.xml"));
			return text2;
		}
		public static string PostData(string url, string postData)
		{
			string text = string.Empty;
			string result;
			try
			{
				Uri requestUri = new Uri(url);
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
				Encoding uTF = Encoding.UTF8;
				byte[] bytes = uTF.GetBytes(postData);
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "text/xml";
				httpWebRequest.ContentLength = (long)postData.Length;
				using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
				{
					streamWriter.Write(postData);
				}
				using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					using (Stream responseStream = httpWebResponse.GetResponseStream())
					{
						Encoding uTF2 = Encoding.UTF8;
						StreamReader streamReader = new StreamReader(responseStream, uTF2);
						text = streamReader.ReadToEnd();
						XmlDocument xmlDocument = new XmlDocument();
						try
						{
							xmlDocument.LoadXml(text);
						}
						catch (Exception ex)
						{
							text = string.Format("获取信息错误doc.load：{0}", ex.Message) + text;
						}
						try
						{
							if (xmlDocument == null)
							{
								result = text;
								return result;
							}
							XmlNode xmlNode = xmlDocument.SelectSingleNode("xml/return_code");
							if (xmlNode == null)
							{
								result = text;
								return result;
							}
							if (!(xmlNode.InnerText == "SUCCESS"))
							{
								result = xmlNode.InnerText;
								return result;
							}
							XmlNode xmlNode2 = xmlDocument.SelectSingleNode("xml/prepay_id");
							if (xmlNode2 != null)
							{
								result = xmlNode2.InnerText;
								return result;
							}
						}
						catch (Exception ex)
						{
							text = string.Format("获取信息错误node.load：{0}", ex.Message) + text;
						}
					}
				}
			}
			catch (Exception ex)
			{
				text = string.Format("获取信息错误post error：{0}", ex.Message) + text;
			}
			result = text;
			return result;
		}
	}
}
