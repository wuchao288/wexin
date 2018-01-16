using Hidistro.Entities.Config;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hidistro.SqlDal.Config
{
    public class AppConfigDao
    {
        Database database = DatabaseFactory.CreateDatabase();

        public AppConfigModel GetConfig()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM tb_app_config where id = 1");
            AppConfigModel model = new AppConfigModel();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    model.Id = int.Parse(reader["id"].ToString());
                    model.LogoLink = reader["logo_link"].ToString();
                    model.IndexPicLink = reader["index_pic_link"].ToString();
                }

            }

            return model;
        }

        public void SaveConfigModel(AppConfigModel model)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update tb_app_config set logo_link = @logo_link,index_pic_link = @index_pic_link WHERE id = @id");
            this.database.AddInParameter(sqlStringCommand, "logo_link", DbType.String, model.LogoLink);
            this.database.AddInParameter(sqlStringCommand, "index_pic_link", DbType.String, model.IndexPicLink);
            this.database.AddInParameter(sqlStringCommand, "id", DbType.Int32, model.Id);

            this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}
