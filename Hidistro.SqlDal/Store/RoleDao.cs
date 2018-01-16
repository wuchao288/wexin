namespace Hidistro.SqlDal.Store
{
    using Hidistro.Entities;
    using Hidistro.Entities.Store;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class RoleDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public void AddPrivilegeInRoles(int roleId, string strPermissions)
        {
            string[] strArray = strPermissions.Split(new char[] { ',' });
            StringBuilder builder = new StringBuilder(" ");
            if ((strArray != null) && (strArray.Length > 0))
            {
                foreach (string str in strArray)
                {
                    builder.AppendFormat("INSERT INTO Hishop_PrivilegeInRoles (RoleId, Privilege) VALUES (@RoleId, {0}); ", str);
                }
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.String, roleId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool AddRole(RoleInfo role)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_Roles (RoleName, Description) VALUES (@RoleName, @Description)");
            this.database.AddInParameter(sqlStringCommand, "RoleName", DbType.String, role.RoleName);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, role.Description);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public void ClearRolePrivilege(int roleId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PrivilegeInRoles WHERE RoleId = @RoleId");
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, roleId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool DeleteRole(int roleId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("if( select count(*) from aspnet_Managers where RoleId = @RoleId ) = 0 DELETE FROM aspnet_Roles WHERE RoleId = @RoleId");
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, roleId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public IList<int> GetPrivilegeByRoles(int roleId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PrivilegeInRoles  WHERE RoleId = @RoleId");
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, roleId);
            IList<int> list = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add((int) reader["Privilege"]);
                }
            }
            return list;
        }

        public RoleInfo GetRole(int roleId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Roles WHERE RoleId = @RoleId");
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, roleId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<RoleInfo>(reader);
            }
        }

        public IList<RoleInfo> GetRoles()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Roles");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<RoleInfo>(reader);
            }
        }

        public IList<RoleQx> GetRoleQx(int roleid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_RolesQx where RoleId = " + roleid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<RoleQx>(reader);
            }
        }

        public void SaveRoleQx(int roleid,string qx)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete aspnet_RolesQx where RoleId = " + roleid);
            this.database.ExecuteNonQuery(sqlStringCommand);


            if (string.IsNullOrEmpty(qx)) { return; }

            string[] strs = qx.Split(',');
            for (int i = 0; i < strs.Length; i++)
            {
                AddQx(roleid, strs[i]);
            }
        }

        public string GetModuleQx(int roleid) {
            string str = "";

            if (roleid == 1) {
                str += "<a onclick=\"ShowMenuLeft('微配置','v1',null)\">配置</a>";
                str += "<a onclick=\"ShowMenuLeft('微会员','v2',null)\">会员</a>";
                str += "<a onclick=\"ShowMenuLeft('微营销','v3',null)\">营销</a>";
                str += "<a onclick=\"ShowMenuLeft('微商品','v4',null)\">商品</a>";
                str += "<a onclick=\"ShowMenuLeft('速特销','v5',null)\">分销</a>";
                str += "<a onclick=\"ShowMenuLeft('微订单','v6',null)\">订单</a>";
                str += "<a onclick=\"ShowMenuLeft('微统计','v7',null)\">统计</a>";
                str += "<a onclick=\"ShowMenuLeft('系统工具','v8',null)\">系统</a>";
 
                return str;
            }

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("exec proc_get_module_qx " + roleid);
            this.database.ExecuteNonQuery(sqlStringCommand);

            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read()) {
                    string v1 = (reader[0] == null) ? "" : reader[0].ToString();
                    string v2 = (reader[1] == null) ? "" : reader[1].ToString();
                    string v3 = (reader[2] == null) ? "" : reader[2].ToString();
                    string v4 = (reader[3] == null) ? "" : reader[3].ToString();
                    string v5 = (reader[4] == null) ? "" : reader[4].ToString();
                    string v6 = (reader[5] == null) ? "" : reader[5].ToString();
                    string v7 = (reader[6] == null) ? "" : reader[6].ToString();
                    string v8 = (reader[7] == null) ? "" : reader[7].ToString();

                    if (v1 != "" && int.Parse(v1) > 0)
                    {
                        str += "<a onclick=\"ShowMenuLeft('微配置','v1',null)\">配置</a>";
                    }

                    if (v2 != "" && int.Parse(v2) > 0)
                    {
                        str += "<a onclick=\"ShowMenuLeft('微会员','v2',null)\">会员</a>";
                    }

                    if (v3 != "" && int.Parse(v3) > 0)
                    {
                        str += "<a onclick=\"ShowMenuLeft('微营销','v3',null)\">营销</a>";
                    }

                    if (v4 != "" && int.Parse(v4) > 0)
                    {
                        str += "<a onclick=\"ShowMenuLeft('微商品','v4',null)\">商品</a>";
                    }

                    if (v5 != "" && int.Parse(v5) > 0)
                    {
                        str += "<a onclick=\"ShowMenuLeft('速特销','v5',null)\">分销</a>";
                    }

                    if (v6 != "" && int.Parse(v6) > 0)
                    {
                        str += "<a onclick=\"ShowMenuLeft('微订单','v6',null)\">订单</a>";
                    }

                    if (v7 != "" && int.Parse(v7) > 0)
                    {
                        str += "<a onclick=\"ShowMenuLeft('微统计','v7',null)\">统计</a>";
                    }

                    if (v8 != "" && int.Parse(v8) > 0)
                    {
                        str += "<a onclick=\"ShowMenuLeft('系统工具','v8',null)\">系统</a>";
                    }
                }
            }


            return str;
        }

        void AddQx(int roleid, string qx) {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("insert into aspnet_RolesQx values("+roleid+",'"+qx+"')");
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool RoleExists(string roleName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM aspnet_Roles WHERE RoleName = @RoleName");
            this.database.AddInParameter(sqlStringCommand, "RoleName", DbType.String, roleName);
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public bool UpdateRole(RoleInfo role)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Roles SET RoleName = @RoleName, Description = @Description WHERE RoleId = @RoleId");
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, role.RoleId);
            this.database.AddInParameter(sqlStringCommand, "RoleName", DbType.String, role.RoleName);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, role.Description);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

