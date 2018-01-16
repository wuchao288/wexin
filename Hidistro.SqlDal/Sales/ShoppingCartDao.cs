﻿namespace Hidistro.SqlDal.Sales
{
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;

    public class ShoppingCartDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public void AddLineItem(MemberInfo member, string skuId, int quantity)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_ShoppingCart_AddLineItem");
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, member.UserId);
            this.database.AddInParameter(storedProcCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(storedProcCommand, "Quantity", DbType.Int32, quantity);
            this.database.ExecuteNonQuery(storedProcCommand);
        }

        public void ClearShoppingCart(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ShoppingCarts WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        /// <summary>
        /// 2015-4-17 update 
        /// 修改：添加完收货地址后，返回报错
        /// </summary>
        /// <param name="member"></param>
        /// <param name="skuId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public ShoppingCartItemInfo GetCartItemInfo(MemberInfo member, string skuId, int quantity)
        {
            ShoppingCartItemInfo info = null;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_ShoppingCart_GetItemInfo");
            this.database.AddInParameter(storedProcCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, (member != null) ? member.UserId : 0);
            this.database.AddInParameter(storedProcCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(storedProcCommand, "GradeId", DbType.Int32, (member != null) ? member.GradeId : 0);
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                if (!reader.Read())
                {
                    return info;
                }
                info = new ShoppingCartItemInfo();

                info.SkuId = skuId;
                info.Quantity = info.ShippQuantity = quantity;
                info.ProductId = (int)reader["ProductId"];


                if (reader["SKU"] != DBNull.Value)
                {
                    info.SKU = (string)reader["SKU"];
                }
                info.Name = (string)reader["ProductName"];
                if (DBNull.Value != reader["Weight"])
                {
                    info.Weight = (int)reader["Weight"];
                }
                info.MemberPrice = info.AdjustedPrice = (decimal)reader["SalePrice"];
                if (DBNull.Value != reader["ThumbnailUrl40"])
                {
                    info.ThumbnailUrl40 = reader["ThumbnailUrl40"].ToString();
                }
                if (DBNull.Value != reader["ThumbnailUrl60"])
                {
                    info.ThumbnailUrl60 = reader["ThumbnailUrl60"].ToString();
                }
                if (DBNull.Value != reader["ThumbnailUrl100"])
                {
                    info.ThumbnailUrl100 = reader["ThumbnailUrl100"].ToString();
                }
                if (DBNull.Value != reader["IsfreeShipping"])
                {
                    info.IsfreeShipping = Convert.ToBoolean(reader["IsfreeShipping"]);
                }
                string str = string.Empty;
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        if (!((((reader["AttributeName"] == DBNull.Value) || string.IsNullOrEmpty((string)reader["AttributeName"])) || (reader["ValueStr"] == DBNull.Value)) || string.IsNullOrEmpty((string)reader["ValueStr"])))
                        {
                            object obj2 = str;
                            str = string.Concat(new object[] { obj2, reader["AttributeName"], "：", reader["ValueStr"], "; " });
                        }
                    }
                }
                info.SkuContent = str;
            }
            return info;
        }

        public ShoppingCartInfo GetShoppingCart(MemberInfo member)
        {
            ShoppingCartInfo info = new ShoppingCartInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ShoppingCarts WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, member.UserId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    ShoppingCartItemInfo item = this.GetCartItemInfo(member, (string)reader["SkuId"], (int)reader["Quantity"]);
                    if (item != null)
                    {
                        info.LineItems.Add(item);
                    }
                }
            }
            return info;
        }

        public void RemoveLineItem(int userId, string skuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ShoppingCarts WHERE UserId = @UserId AND SkuId = @SkuId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public void UpdateLineItemQuantity(MemberInfo member, string skuId, int quantity)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ShoppingCarts SET Quantity = @Quantity WHERE UserId = @UserId AND SkuId = @SkuId");
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, member.UserId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

