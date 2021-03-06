﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
namespace Hishop.Weixin.MP.Util
{
	public sealed class WebUtils
	{
		public string DoPost(string url, IDictionary<string, string> parameters)
		{
			HttpWebRequest webRequest = this.GetWebRequest(url, "POST");
			webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
			byte[] bytes = Encoding.UTF8.GetBytes(WebUtils.BuildQuery(parameters));
			Stream requestStream = webRequest.GetRequestStream();
			requestStream.Write(bytes, 0, bytes.Length);
			requestStream.Close();
			HttpWebResponse rsp = (HttpWebResponse)webRequest.GetResponse();
			return this.GetResponseAsString(rsp, Encoding.UTF8);
		}
		public string DoPost(string url, string value)
		{
			HttpWebRequest webRequest = this.GetWebRequest(url, "POST");
			webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
			byte[] bytes = Encoding.UTF8.GetBytes(value);
			Stream requestStream = webRequest.GetRequestStream();
			requestStream.Write(bytes, 0, bytes.Length);
			requestStream.Close();
			HttpWebResponse rsp = (HttpWebResponse)webRequest.GetResponse();
			return this.GetResponseAsString(rsp, Encoding.UTF8);
		}
		public string DoGet(string url, IDictionary<string, string> parameters)
		{
			if (parameters != null && parameters.Count > 0)
			{
				if (url.Contains("?"))
				{
					url = url + "&" + WebUtils.BuildQuery(parameters);
				}
				else
				{
					url = url + "?" + WebUtils.BuildQuery(parameters);
				}
			}
			HttpWebRequest webRequest = this.GetWebRequest(url, "GET");
			webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
			HttpWebResponse rsp = (HttpWebResponse)webRequest.GetResponse();
			return this.GetResponseAsString(rsp, Encoding.UTF8);
		}
		public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}
		public HttpWebRequest GetWebRequest(string url, string method)
		{
			HttpWebRequest httpWebRequest;
			if (url.Contains("https"))
			{
				ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.CheckValidationResult);
				httpWebRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
			}
			else
			{
				httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			}
			httpWebRequest.ServicePoint.Expect100Continue = false;
			httpWebRequest.Method = method;
			httpWebRequest.KeepAlive = true;
			httpWebRequest.UserAgent = "Hishop";
			return httpWebRequest;
		}
		public string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
		{
			Stream stream = null;
			StreamReader streamReader = null;
			string result;
			try
			{
				stream = rsp.GetResponseStream();
				streamReader = new StreamReader(stream, encoding);
				result = streamReader.ReadToEnd();
			}
			finally
			{
				if (streamReader != null)
				{
					streamReader.Close();
				}
				if (stream != null)
				{
					stream.Close();
				}
				if (rsp != null)
				{
					rsp.Close();
				}
			}
			return result;
		}
		public string BuildGetUrl(string url, IDictionary<string, string> parameters)
		{
			if (parameters != null && parameters.Count > 0)
			{
				if (url.Contains("?"))
				{
					url = url + "&" + WebUtils.BuildQuery(parameters);
				}
				else
				{
					url = url + "?" + WebUtils.BuildQuery(parameters);
				}
			}
			return url;
		}
		public static string BuildQuery(IDictionary<string, string> parameters)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			IEnumerator<KeyValuePair<string, string>> enumerator = parameters.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, string> current = enumerator.Current;
				string key = current.Key;
				current = enumerator.Current;
				string value = current.Value;
				if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
				{
					if (flag)
					{
						stringBuilder.Append("&");
					}
					stringBuilder.Append(key);
					stringBuilder.Append("=");
					stringBuilder.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
					flag = true;
				}
			}
			return stringBuilder.ToString();
		}

        public static string UploadFileAndValues(string url, string[] files, Dictionary<string, string> values)
        {
            var request = WebRequest.Create(url);
            request.Method = "POST";
            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            boundary = "--" + boundary;

            using (var requestStream = new StreamWriter(request.GetRequestStream(), Encoding.UTF8))
            {
                foreach (string name in values.Keys)
                {
                    requestStream.WriteLine(boundary);
                    requestStream.WriteLine("Content-Disposition: form-data; name=\"{0}\"\r\n", name);
                    requestStream.WriteLine(HttpUtility.UrlEncode(values[name]));
                    requestStream.WriteLine();
                }
                foreach (var f in files)
                {
                    var file = new FileInfo(f);
                    requestStream.WriteLine(boundary);
                    FileStream fs = new FileStream(f, FileMode.Open);
                    StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"media\";filelength=\"{1}\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", file.Name, fs.Length));
                    fs.Close();
                    //requestStream.WriteLine("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", file.Name.Replace(".jpg",""), file.Name);
                    //requestStream.WriteLine("Content-Type: application/octet-stream\n");
                    requestStream.WriteLine(sbHeader);
                    requestStream.Write(File.ReadAllBytes(file.FullName));
                }
                requestStream.Write(boundary + "--");
            }

            using (var response = request.GetResponse())
            {
                return new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
        }
	}
}
