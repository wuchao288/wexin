namespace Hidistro.SqlDal.VShop
{
    using Hidistro.Entities;
    using Hidistro.Entities.VShop;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class ActivitySignUpDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public IList<ActivitySignUpInfo> GetActivitySignUpById(int activityId)
        {
            string query = "SELECT * FROM vshop_ActivitySignUp WHERE ActivityId = @ActivityId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<ActivitySignUpInfo>(reader);
            }
        }

        public bool SaveActivitySignUp(ActivitySignUpInfo info)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("IF NOT EXISTS (select 1 from vshop_ActivitySignUp WHERE ActivityId=@ActivityId and UserId=@UserId) ").Append("INSERT INTO vshop_ActivitySignUp(").Append("ActivityId,UserId,UserName,RealName,SignUpDate").Append(",Item1,Item2,Item3,Item4,Item5)").Append(" VALUES (").Append("@ActivityId,@UserId,@UserName,@RealName,@SignUpDate").Append(",@Item1,@Item2,@Item3,@Item4,@Item5)");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, info.ActivityId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, info.UserId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, info.UserName);
            this.database.AddInParameter(sqlStringCommand, "RealName", DbType.String, info.RealName);
            this.database.AddInParameter(sqlStringCommand, "SignUpDate", DbType.DateTime, info.SignUpDate);
            this.database.AddInParameter(sqlStringCommand, "Item1", DbType.String, info.Item1);
            this.database.AddInParameter(sqlStringCommand, "Item2", DbType.String, info.Item2);
            this.database.AddInParameter(sqlStringCommand, "Item3", DbType.String, info.Item3);
            this.database.AddInParameter(sqlStringCommand, "Item4", DbType.String, info.Item4);
            this.database.AddInParameter(sqlStringCommand, "Item5", DbType.String, info.Item5);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

