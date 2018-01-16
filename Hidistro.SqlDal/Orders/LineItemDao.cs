﻿namespace Hidistro.SqlDal.Orders
{
    using Hidistro.Entities.Orders;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class LineItemDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddOrderLineItems(string orderId, ICollection lineItems, DbTransaction dbTran)
        {
            if ((lineItems == null) || (lineItems.Count == 0))
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            int num = 0;
            StringBuilder builder = new StringBuilder();
            foreach (LineItemInfo info in lineItems)
            {
                string str = num.ToString();
                builder.Append("INSERT INTO Hishop_OrderItems(OrderId, SkuId, ProductId, SKU, Quantity, ShipmentQuantity, CostPrice").Append(",ItemListPrice, ItemAdjustedPrice, ItemDescription, ThumbnailsUrl, Weight, SKUContent, PromotionId, PromotionName) VALUES( @OrderId").Append(",@SkuId").Append(str).Append(",@ProductId").Append(str).Append(",@SKU").Append(str).Append(",@Quantity").Append(str).Append(",@ShipmentQuantity").Append(str).Append(",@CostPrice").Append(str).Append(",@ItemListPrice").Append(str).Append(",@ItemAdjustedPrice").Append(str).Append(",@ItemDescription").Append(str).Append(",@ThumbnailsUrl").Append(str).Append(",@Weight").Append(str).Append(",@SKUContent").Append(str).Append(",@PromotionId").Append(str).Append(",@PromotionName").Append(str).Append(");");
                this.database.AddInParameter(sqlStringCommand, "SkuId" + str, DbType.String, info.SkuId);
                this.database.AddInParameter(sqlStringCommand, "ProductId" + str, DbType.Int32, info.ProductId);
                this.database.AddInParameter(sqlStringCommand, "SKU" + str, DbType.String, info.SKU);
                this.database.AddInParameter(sqlStringCommand, "Quantity" + str, DbType.Int32, info.Quantity);
                this.database.AddInParameter(sqlStringCommand, "ShipmentQuantity" + str, DbType.Int32, info.ShipmentQuantity);
                this.database.AddInParameter(sqlStringCommand, "CostPrice" + str, DbType.Currency, info.ItemCostPrice);
                this.database.AddInParameter(sqlStringCommand, "ItemListPrice" + str, DbType.Currency, info.ItemListPrice);
                this.database.AddInParameter(sqlStringCommand, "ItemAdjustedPrice" + str, DbType.Currency, info.ItemAdjustedPrice);
                this.database.AddInParameter(sqlStringCommand, "ItemDescription" + str, DbType.String, info.ItemDescription);
                this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + str, DbType.String, info.ThumbnailsUrl);
                this.database.AddInParameter(sqlStringCommand, "Weight" + str, DbType.Int32, info.ItemWeight);
                this.database.AddInParameter(sqlStringCommand, "SKUContent" + str, DbType.String, info.SKUContent);
                this.database.AddInParameter(sqlStringCommand, "PromotionId" + str, DbType.Int32, info.PromotionId);
                this.database.AddInParameter(sqlStringCommand, "PromotionName" + str, DbType.String, info.PromotionName);
                num++;
                if (num == 50)
                {
                    int num2;
                    sqlStringCommand.CommandText = builder.ToString();
                    if (dbTran != null)
                    {
                        num2 = this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
                    }
                    else
                    {
                        num2 = this.database.ExecuteNonQuery(sqlStringCommand);
                    }
                    if (num2 <= 0)
                    {
                        return false;
                    }
                    builder.Remove(0, builder.Length);
                    sqlStringCommand.Parameters.Clear();
                    this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
                    num = 0;
                }
            }
            if (builder.ToString().Length > 0)
            {
                sqlStringCommand.CommandText = builder.ToString();
                if (dbTran != null)
                {
                    return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
                }
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            return true;
        }

        public bool DeleteLineItem(string skuId, string orderId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_OrderItems WHERE OrderId=@OrderId AND SkuId=@SkuId ");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public bool UpdateLineItem(string orderId, LineItemInfo lineItem, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_OrderItems SET ShipmentQuantity=@ShipmentQuantity,ItemAdjustedPrice=@ItemAdjustedPrice,Quantity=@Quantity, PromotionId = NULL, PromotionName = NULL WHERE OrderId=@OrderId AND SkuId=@SkuId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, lineItem.SkuId);
            this.database.AddInParameter(sqlStringCommand, "ShipmentQuantity", DbType.Int32, lineItem.ShipmentQuantity);
            this.database.AddInParameter(sqlStringCommand, "ItemAdjustedPrice", DbType.Currency, lineItem.ItemAdjustedPrice);
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, lineItem.Quantity);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }
    }
}

