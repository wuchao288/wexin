using Hishop.Weixin.MP.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
namespace Hishop.Weixin.MP.Util
{
	public static class EntityHelper
	{
		public static void FillEntityWithXml<T>(T entity, XDocument doc) where T : AbstractRequest, new()
		{
			T arg_10_0;
			if ((arg_10_0 = entity) == null)
			{
				arg_10_0 = Activator.CreateInstance<T>();
			}
			entity = arg_10_0;
			XElement root = doc.Root;
			PropertyInfo[] properties = entity.GetType().GetProperties();
			PropertyInfo[] array = properties;
			for (int i = 0; i < array.Length; i++)
			{
				PropertyInfo propertyInfo = array[i];
				string name = propertyInfo.Name;
				if (root.Element(name) != null)
				{
					string name2 = propertyInfo.PropertyType.Name;
					if (name2 == null)
					{
						goto IL_1CC;
					}
					if (!(name2 == "DateTime"))
					{
						if (!(name2 == "Boolean"))
						{
							if (!(name2 == "Int64"))
							{
								if (!(name2 == "RequestEventType"))
								{
									if (!(name2 == "RequestMsgType"))
									{
										goto IL_1CC;
									}
									propertyInfo.SetValue(entity, MsgTypeHelper.GetMsgType(root.Element(name).Value), null);
								}
								else
								{
									propertyInfo.SetValue(entity, EventTypeHelper.GetEventType(root.Element(name).Value), null);
								}
							}
							else
							{
								propertyInfo.SetValue(entity, long.Parse(root.Element(name).Value), null);
							}
						}
						else
						{
							if (!(name == "FuncFlag"))
							{
								goto IL_1CC;
							}
							propertyInfo.SetValue(entity, root.Element(name).Value == "1", null);
						}
					}
					else
					{
						propertyInfo.SetValue(entity, new DateTime(long.Parse(root.Element(name).Value)), null);
					}
					goto IL_1EE;
					IL_1CC:
					propertyInfo.SetValue(entity, root.Element(name).Value, null);
				}
				IL_1EE:;
			}
		}
		public static XDocument ConvertEntityToXml<T>(T entity) where T : class, new()
		{
			T arg_17_0;
			if ((arg_17_0 = entity) == null)
			{
				arg_17_0 = Activator.CreateInstance<T>();
			}
			entity = arg_17_0;
			XDocument xDocument = new XDocument();
			xDocument.Add(new XElement("xml"));
			XElement root = xDocument.Root;
			List<string> @object = new string[]
			{
				"ToUserName",
				"FromUserName",
				"CreateTime",
				"MsgType",
				"Content",
				"ArticleCount",
				"Articles",
				"FuncFlag",
				"Title ",
				"Description ",
				"PicUrl",
				"Url"
			}.ToList<string>();
			Func<string, int> orderByPropName = new Func<string, int>(@object.IndexOf);
			List<PropertyInfo> list = (
				from p in entity.GetType().GetProperties()
				orderby orderByPropName(p.Name)
				select p).ToList<PropertyInfo>();
			foreach (PropertyInfo current in list)
			{
				string name = current.Name;
				if (name == "Articles")
				{
					XElement xElement = new XElement("Articles");
					List<Article> list2 = current.GetValue(entity, null) as List<Article>;
					foreach (Article current2 in list2)
					{
						IEnumerable<XElement> content = EntityHelper.ConvertEntityToXml<Article>(current2).Root.Elements();
						xElement.Add(new XElement("item", content));
					}
					root.Add(xElement);
				}
                else if (name == "Image") {
                    XElement xElement = new XElement("Image");

                    xElement.Add(new XElement("MediaId", new XCData(((Image)current.GetValue(entity, null)).MediaId )));
                    
                    root.Add(xElement);
                }
                else
                {
                    string name2 = current.PropertyType.Name;
                    if (name2 == null)
                    {
                        goto IL_353;
                    }
                    if (!(name2 == "String"))
                    {
                        if (!(name2 == "DateTime"))
                        {
                            if (!(name2 == "Boolean"))
                            {
                                if (!(name2 == "ResponseMsgType"))
                                {
                                    if (!(name2 == "Article"))
                                    {
                                        goto IL_353;
                                    }
                                    root.Add(new XElement(name, current.GetValue(entity, null).ToString().ToLower()));
                                }
                                else
                                {
                                    root.Add(new XElement(name, current.GetValue(entity, null).ToString().ToLower()));
                                }
                            }
                            else
                            {
                                if (!(name == "FuncFlag"))
                                {
                                    goto IL_353;
                                }
                                root.Add(new XElement(name, ((bool)current.GetValue(entity, null)) ? "1" : "0"));
                            }
                        }
                        else
                        {
                            root.Add(new XElement(name, ((DateTime)current.GetValue(entity, null)).Ticks));
                        }
                    }
                    else
                    {
                        root.Add(new XElement(name, new XCData((current.GetValue(entity, null) as string) ?? "")));
                    }
                    continue;
                IL_353:
                    root.Add(new XElement(name, current.GetValue(entity, null)));
                }
			}
			return xDocument;
		}
	}
}
