namespace Hidistro.SqlDal
{
    using Hidistro.Entities;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class RefundDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public void AddRefund(RefundInfo refundInfo)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("insert into Hishop_OrderRefund(OrderId,ApplyForTime,RefundRemark,HandleStatus) values('{0}','{1}','{2}',{3})", new object[] { refundInfo.OrderId, refundInfo.ApplyForTime, refundInfo.RefundRemark, (int) refundInfo.HandleStatus });
            this.database.ExecuteNonQuery(CommandType.Text, builder.ToString());
        }

        public RefundInfo GetByOrderId(string orderId)
        {
            string query = "select * from Hishop_OrderRefund where OrderId='" + orderId + "'";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<RefundInfo>(reader);
            }
        }

        public void GetRefundType(string orderId, out int refundType, out string remark)
        {
            refundType = 0;
            remark = "";
            string query = "select RefundType,RefundRemark from Hishop_OrderRefund where OrderId='" + orderId + "'";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    refundType = (reader["RefundType"] != DBNull.Value) ? ((int) reader["RefundType"]) : 0;
                    remark = (string) reader["RefundRemark"];
                }
            }
        }

        public void GetRefundTypeFromReturn(string orderId, out int refundType, out string remark)
        {
            refundType = 0;
            remark = "";
            string query = "select RefundType,Comments from Hishop_OrderReturns where HandleStatus=0 and OrderId='" + orderId + "'";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            IDataReader reader = this.database.ExecuteReader(sqlStringCommand);
            if (reader.Read())
            {
                refundType = (int) reader["RefundType"];
                remark = (string) reader["Comments"];
            }
        }

        public void UpdateByOrderId(RefundInfo refundInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_OrderRefund set AdminRemark=@AdminRemark,ApplyForTime=@ApplyForTime,HandleStatus=@HandleStatus,HandleTime=@HandleTime,Operator=@Operator,RefundRemark=@RefundRemark where OrderId =@OrderId");
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, refundInfo.AdminRemark);
            this.database.AddInParameter(sqlStringCommand, "ApplyForTime", DbType.String, refundInfo.ApplyForTime);
            this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, refundInfo.HandleStatus);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, refundInfo.HandleTime);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, refundInfo.Operator);
            this.database.AddInParameter(sqlStringCommand, "RefundRemark", DbType.String, refundInfo.RefundRemark);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, refundInfo.OrderId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

