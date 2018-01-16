namespace Hidistro.Jobs
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Jobs;
    using Hidistro.SaleSystem.Vshop;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Xml;

    public class OrderJob : IJob
    {
        public void Execute(XmlNode node)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(" UPDATE Hishop_Orders SET OrderStatus=4,CloseReason='过期没付款，自动关闭' WHERE OrderStatus=1 AND OrderDate <= @OrderDate; UPDATE Hishop_Orders SET FinishDate = getdate(), OrderStatus = 5 WHERE OrderStatus=3 AND ShippingDate <= @ShippingDate");
            database.AddInParameter(sqlStringCommand, "OrderDate", DbType.DateTime, DateTime.Now.AddDays((double) -masterSettings.CloseOrderDays));
            database.AddInParameter(sqlStringCommand, "ShippingDate", DbType.DateTime, DateTime.Now.AddDays((double) -masterSettings.FinishOrderDays));
            database.ExecuteNonQuery(sqlStringCommand);
            string query = string.Format("SELECT OrderId FROM  Hishop_Orders WHERE  OrderStatus=3 AND ShippingDate <= '" + DateTime.Now.AddDays((double) -masterSettings.FinishOrderDays) + "'", new object[0]);
            DbCommand command = database.GetSqlStringCommand(query);
            DataTable table = database.ExecuteDataSet(command).Tables[0];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DistributorsBrower.UpdateCalculationCommission(ShoppingProcessor.GetOrderInfo(table.Rows[0][0].ToString()));
            }
        }
    }
}

