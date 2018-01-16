using Hidistro.Entities;
using Hidistro.Entities.Store;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Xml;

namespace Hidistro.SqlDal.Store
{
    public class MenuDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public void ImportMenu(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xml);

            XmlNode xn = doc.SelectSingleNode("Menu");
            
            // 得到根节点的所有子节点
            XmlNodeList xnl = xn.ChildNodes;
            foreach (XmlNode item in xnl)   //Module
            {
                XmlNodeList Items = item.ChildNodes;
                foreach (XmlNode item2 in Items)
                {
                    string module = ((XmlElement)item2).GetAttribute("Title");

                    XmlNodeList links = item2.ChildNodes;

                    foreach (XmlNode item3 in links)
                    {
                        //保存连接
                        XmlElement e = (XmlElement)item3;

                        string title = e.GetAttribute("Title");
                        string link = e.GetAttribute("Link");
                        string qx = e.GetAttribute("Qx");

                        AddMenu(qx, module,title,link);
                    }

                }

            }
        }

        public void AddMenu(string qx, string category, string title, string link) {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("insert into aspnet_menu values('" + qx + "','" + category + "','" + title + "','" + link + "')");
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public IList<MenuQxInfo> GetList(int roleid, string module)
        {
            string sql = string.Format("select * from aspnet_menu where qx like '"+module+"%'");
            if (roleid != 1)
            {
                sql += " and qx in (select qx from aspnet_RolesQx where RoleId = " + roleid + ")";
            }

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<MenuQxInfo>(reader);
            }
        }
    }
}
