namespace Hidistro.SqlDal.Members
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;
    using System.Text.RegularExpressions;

    public class DistributorsDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddBalanceDrawRequest(BalanceDrawRequestInfo bdrinfo)
        {
            string query = "INSERT INTO Hishop_BalanceDrawRequest(UserId,RequestType,UserName,Amount,AccountName,CellPhone,MerchantCode,Remark,RequestTime,IsCheck) VALUES(@UserId,@RequestType,@UserName,@Amount,@AccountName,@CellPhone,@MerchantCode,@Remark,getdate(),0)";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, bdrinfo.UserId);
            this.database.AddInParameter(sqlStringCommand, "RequestType", DbType.Int32, bdrinfo.RequesType);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, bdrinfo.UserName);
            this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Decimal, bdrinfo.Amount);
            this.database.AddInParameter(sqlStringCommand, "AccountName", DbType.String, bdrinfo.AccountName);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, bdrinfo.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "MerchantCode", DbType.String, bdrinfo.MerchanCade);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, bdrinfo.Remark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public void AddDistributorProducts(int productId, int distributorId)
        {
            string query = "INSERT INTO Hishop_DistributorProducts VALUES(@ProductId,@UserId)";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, distributorId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool CreateDistributor(DistributorsInfo distributor)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_Distributors(UserId,StoreName,Logo,BackImage,RequestAccount,GradeId,ReferralUserId,ReferralPath, ReferralOrders,ReferralBlance, ReferralRequestBalance,ReferralStatus,StoreDescription,ReferralUserId2) VALUES(@UserId,@StoreName,@Logo,@BackImage,@RequestAccount,@GradeId,@ReferralUserId,0,@ReferralOrders,@ReferralBlance, @ReferralRequestBalance, @ReferralStatus,@StoreDescription,@ReferralUserId2)");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, distributor.UserId);
            this.database.AddInParameter(sqlStringCommand, "StoreName", DbType.String, distributor.StoreName);
            this.database.AddInParameter(sqlStringCommand, "Logo", DbType.String, distributor.Logo);
            this.database.AddInParameter(sqlStringCommand, "BackImage", DbType.String, distributor.BackImage);
            this.database.AddInParameter(sqlStringCommand, "RequestAccount", DbType.String, distributor.RequestAccount);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int64, (int) distributor.DistributorGradeId);
            this.database.AddInParameter(sqlStringCommand, "ReferralUserId", DbType.Int64, distributor.ParentUserId == null ? 0 : distributor.ParentUserId.Value);
            //this.database.AddInParameter(sqlStringCommand, "ReferralPath", DbType.String, distributor.ReferralPath);
            this.database.AddInParameter(sqlStringCommand, "ReferralOrders", DbType.Int64, distributor.ReferralOrders);
            this.database.AddInParameter(sqlStringCommand, "ReferralBlance", DbType.Decimal, distributor.ReferralBlance);
            this.database.AddInParameter(sqlStringCommand, "ReferralRequestBalance", DbType.Decimal, distributor.ReferralRequestBalance);
            this.database.AddInParameter(sqlStringCommand, "ReferralStatus", DbType.Int64, distributor.ReferralStatus);
            this.database.AddInParameter(sqlStringCommand, "StoreDescription", DbType.String, distributor.StoreDescription);
            this.database.AddInParameter(sqlStringCommand, "ReferralUserId2", DbType.Int64, distributor.ReferralUserId2);
            
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public void SetDistributorStatus(int userid, int status)
        {
            string sql = string.Format("update aspnet_Distributors set [Status] = {0} where UserId = {1}", status, userid);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public void AddDistributor(string userid,
                string username,
                string mobile,
                string weixin,
                string parent_id,
                string email,
                string qq,
                string address)
        {
            //会员是否存在
            MemberDao mDao = new MemberDao();
            MemberInfo member = mDao.GetMember(int.Parse(userid));
            if (member == null) {
                throw new Exception("会员信息不存在");
            }

            DistributorsInfo info = GetDistributorInfo(int.Parse(userid));
            if (info == null) {
                info = new DistributorsInfo();
                info.UserId = int.Parse(userid);
                info.StoreName = "速特销";
                info.Logo = "/Templates/vshop/default/images/logo.jpg";
                info.BackImage = "/Templates/vshop/default/images/logo_bg.jpg";
                
                if (!string.IsNullOrEmpty(parent_id))
                {
                    info.ReferralUserId = int.Parse(parent_id);
                }
                CreateDistributor(info);
            }

            if (!string.IsNullOrEmpty(weixin))
            {
                member.MicroSignal = weixin;
            }

            if (!string.IsNullOrEmpty(username))
            {
                member.RealName = username;
            }

            if (!string.IsNullOrEmpty(qq))
            {
                member.QQ = qq;
            }
            if (!string.IsNullOrEmpty(email))
            {
                member.Email = email;
            }

            if (!string.IsNullOrEmpty(address))
            {
                member.Address = address;
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                member.CellPhone = mobile;
            }

            if (!string.IsNullOrEmpty(parent_id))
            {
                member.ReferralUserId = int.Parse(parent_id);
            }

            mDao.Update(member);
        }

        public DataTable GetAllDistributorsName(string keywords)
        {
            DataTable table = new DataTable();
            string[] strArray = Regex.Split(DataHelper.CleanSearchString(keywords), @"\s+");
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" StoreName LIKE '%{0}%' OR UserName LIKE '%{0}%'", DataHelper.CleanSearchString(DataHelper.CleanSearchString(strArray[0])));
            for (int i = 1; (i < strArray.Length) && (i <= 5); i++)
            {
                builder.AppendFormat(" OR StoreName LIKE '%{0}%' OR UserName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[i]));
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 10 StoreName,UserName from vw_Hishop_DistributorsMembers WHERE " + builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DbQueryResult GetBalanceDrawRequest(BalanceDrawRequestQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(query.StoreName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" StoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
            }
            if (!string.IsNullOrEmpty(query.UserId))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" UserId = {0}", DataHelper.CleanSearchString(query.UserId));
            }
            if (!string.IsNullOrEmpty(query.RequestTime.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" convert(varchar(10),RequestTime,120)='{0}'", query.RequestTime);
            }
            if (!string.IsNullOrEmpty(query.IsCheck.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" IsCheck={0}", query.IsCheck);
            }
            if (!string.IsNullOrEmpty(query.CheckTime.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" convert(varchar(10),CheckTime,120)='{0}'", query.CheckTime);
            }
            if (!string.IsNullOrEmpty(query.RequestStartTime.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" datediff(dd,'{0}',RequestTime)>=0", query.RequestStartTime);
            }
            if (!string.IsNullOrEmpty(query.RequestEndTime.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("  datediff(dd,'{0}',RequestTime)<=0", query.RequestEndTime);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BalanceDrawRequesDistributors ", "SerialID", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public DbQueryResult GetCommissions(CommissionsQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.UserId > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("UserId = {0}", query.UserId);
            }
            if (!string.IsNullOrEmpty(query.StoreName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("StoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
            }
            if (!string.IsNullOrEmpty(query.OrderNum))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" OrderId = {0}", query.OrderNum);
            }
            if (!string.IsNullOrEmpty(query.StartTime.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" datediff(dd,'{0}',TradeTime)>=0", query.StartTime);
            }
            if (!string.IsNullOrEmpty(query.EndTime.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("  datediff(dd,'{0}',TradeTime)<=0", query.EndTime);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_CommissionDistributors", "CommId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public DataTable GetCurrentDistributorsCommosion(int userId)
        {
            string query = string.Format("SELECT sum(OrderTotal) AS OrderTotal,sum(CommTotal) AS CommTotal from dbo.Hishop_Commissions where UserId={0} AND OrderId in (select OrderId from dbo.Hishop_Orders where ReferralUserId={0})", userId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            if ((set == null) || (set.Tables.Count <= 0))
            {
                return null;
            }
            return set.Tables[0];
        }

        public DistributorsInfo GetDistributorInfo(int distributorId)
        {
            if (distributorId <= 0)
            {
                return null;
            }
            DistributorsInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM aspnet_Distributors where UserId={0}", distributorId));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateDistributorInfo(reader);
                }
            }
            return info;
        }

        public int GetDistributorNum()
        {
            int num = 0;
            string query = string.Format("SELECT COUNT(*) FROM aspnet_Distributors where ReferralUserId = '{0}' OR ReferralUserId2 = '{0}'", Globals.GetCurrentMemberUserId());
            //if (grade != DistributorGrade.All)
            //{
            //    query = query + " AND GradeId=" + ((int) grade);
            //}
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    num = (int) reader[0];
                    reader.Close();
                }
            }
            return num;
        }

        public DbQueryResult GetDistributors(DistributorsQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.GradeId > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("GradeId = {0}", query.GradeId);
            }
            if (query.UserId > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("UserId = {0}", query.UserId);
            }
            if (query.ReferralStatus > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("ReferralStatus = '{0}'", query.ReferralStatus);
            }
            if (!string.IsNullOrEmpty(query.StoreName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("StoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
            }
            if (!string.IsNullOrEmpty(query.CellPhone))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("CellPhone='{0}'", DataHelper.CleanSearchString(query.CellPhone));
            }
            if (!string.IsNullOrEmpty(query.MicroSignal))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("MicroSignal = '{0}'", DataHelper.CleanSearchString(query.MicroSignal));
            }
            //if (!string.IsNullOrEmpty(query.RealName))
            //{
            //    if (builder.Length > 0)
            //    {
            //        builder.Append(" AND ");
            //    }
            //    builder.AppendFormat("RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.RealName));
            //}
            if (!string.IsNullOrEmpty(query.RealName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.RealName));
            }
            if (query.ReferralUserId > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("ReferralUserId = {0}", query.ReferralUserId);
            }

            if (query.ReferralUserId2 > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("ReferralUserId2 = {0}", query.ReferralUserId2);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_DistributorsMembers", "UserId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public DataTable GetDistributorsCommission(DistributorsQuery query)
        {
            StringBuilder builder = new StringBuilder("1=1");
            //if (query.GradeId > 0)
            //{
            //    builder.AppendFormat("AND GradeId = {0}", query.GradeId);
            //}

            if (query.ReferralUserId2 > 0)
            {
                builder.AppendFormat("AND ReferralUserId2 = {0}", query.ReferralUserId2);
            }
            else {
                builder.AppendFormat("AND ReferralUserId = {0}", query.ReferralUserId);
            }

            string str = string.Concat(new object[] { "select TOP ", query.PageSize, " UserId,StoreName,GradeId,CreateTime,isnull((select SUM(OrderTotal) from Hishop_Commissions where UserId=", query.UserId, " AND ReferralUserId=aspnet_Distributors.UserId),0) as OrderTotal,isnull((select SUM(CommTotal) from Hishop_Commissions where UserId=", query.UserId, " AND ReferralUserId=aspnet_Distributors.UserId),0) as  CommTotal,(select UserName from aspnet_Members where userid = aspnet_Distributors.UserId) UserName  from aspnet_Distributors WHERE ", builder.ToString(), " order by CreateTime " });
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable GetDistributorsCommosion(int userId)
        {
            string query = string.Format("SELECT  GradeId,COUNT(*),SUM(OrdersTotal) AS OrdersTotal,SUM(ReferralOrders) AS ReferralOrders,SUM(ReferralBlance) AS ReferralBlance,SUM(ReferralRequestBalance) AS ReferralRequestBalance FROM aspnet_Distributors WHERE ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}' GROUP BY GradeId", userId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            if ((set == null) || (set.Tables.Count <= 0))
            {
                return null;
            }
            return set.Tables[0];
        }

        public DataTable GetDistributorsCommosion(int userId, DistributorGrade grade)
        {
            string query = string.Format("SELECT sum(OrderTotal) AS OrderTotal,sum(CommTotal) AS CommTotal from dbo.Hishop_Commissions where UserId={0} AND ReferralUserId in (select UserId from aspnet_Distributors  WHERE (ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}') and GradeId={1})", userId, (int) grade);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            if ((set == null) || (set.Tables.Count <= 0))
            {
                return null;
            }
            return set.Tables[0];
        }

        public int GetDownDistributorNum(string userid)
        {
            int num = 0;
            string query = string.Format("SELECT COUNT(*) FROM aspnet_Distributors where ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}'", userid);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    num = (int)reader[0];
                    reader.Close();
                }
            }
            return num;
        }

        /// <summary>
        /// 获取上级商家信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetReferralDistributor(int userid)
        {
            string query = string.Format("SELECT * FROM aspnet_Distributors where UserId={0}", userid);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            if ((set == null) || (set.Tables.Count <= 0))
            {
                return null;
            }
            return set.Tables[0];
        }

        public int GetDownDistributorNumReferralOrders(string userid)
        {
            int num = 0;
            string query = string.Format("SELECT isnull(sum(ReferralOrders),0) FROM aspnet_Distributors where ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}'", userid);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            if (set.Tables[0].Rows.Count > 0)
            {
                num = int.Parse(set.Tables[0].Rows[0][0].ToString());
            }
            return num;
        }

        public int IsExiteDistributorsByStoreName(string storname)
        {
            string query = "SELECT UserId FROM aspnet_Distributors WHERE StoreName=@StoreName";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "StoreName", DbType.String, DataHelper.CleanSearchString(storname));
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            return ((obj2 != null) ? ((int) obj2) : 0);
        }

        public bool IsExitsCommionsRequest(int userId)
        {
            bool flag = false;
            string query = "SELECT * FROM dbo.Hishop_BalanceDrawRequest WHERE IsCheck=0 AND UserId=@UserId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    flag = true;
                }
            }
            return flag;
        }

        public void RemoveDistributorProducts(List<int> productIds, int distributorId)
        {
            string str = string.Join<int>(",", productIds);
            string query = "DELETE FROM Hishop_DistributorProducts WHERE UserId=@UserId AND ProductId IN (" + str + ");";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, distributorId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool UpdateBalanceDistributors(int UserId, decimal ReferralRequestBalance)
        {
            string query = "UPDATE aspnet_Distributors set ReferralBlance=ReferralBlance-@ReferralBlance,ReferralRequestBalance=ReferralRequestBalance+@ReferralRequestBalance  WHERE UserId=@UserId ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ReferralBlance", DbType.Decimal, ReferralRequestBalance);
            this.database.AddInParameter(sqlStringCommand, "ReferralRequestBalance", DbType.Decimal, ReferralRequestBalance);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateBalanceDrawRequest(int Id, string Remark)
        {
            string query = "UPDATE Hishop_BalanceDrawRequest set Remark=@Remark,IsCheck=1,CheckTime=getdate() WHERE SerialID=@SerialID ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, Remark);
            this.database.AddInParameter(sqlStringCommand, "SerialID", DbType.Int32, Id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateCalculationCommission(ArrayList UserIdList, ArrayList ReferralBlanceList, string orderid, ArrayList OrdersTotalList, string userid)
        {
            string query = "";
            query = query + "begin try  " + "  begin tran TranUpdate";
            for (int i = 0; i < UserIdList.Count; i++)
            {
                object obj2 = query;
                obj2 = string.Concat(new object[] { obj2, " INSERT INTO [Hishop_Commissions]([UserId],[ReferralUserId],[OrderId],[OrderTotal],[CommTotal],[CommType])VALUES(", UserIdList[i], ",", userid, ",'", orderid, "',", OrdersTotalList[i], ",", ReferralBlanceList[i], ",1);" });
                query = string.Concat(new object[] { obj2, "  UPDATE aspnet_Distributors set ReferralBlance=ReferralBlance+", ReferralBlanceList[i], "  WHERE UserId=", UserIdList[i], "; " });
            }
            query = query + " COMMIT TRAN TranUpdate" + "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateDistributor(DistributorsInfo distributor)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Distributors SET StoreName=@StoreName,Logo=@Logo,BackImage=@BackImage,ReferralOrders=@ReferralOrders,ReferralBlance=@ReferralBlance, ReferralRequestBalance=@ReferralRequestBalance,StoreDescription=@StoreDescription,ReferralStatus=@ReferralStatus WHERE UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, distributor.UserId);
            this.database.AddInParameter(sqlStringCommand, "StoreName", DbType.String, distributor.StoreName);
            this.database.AddInParameter(sqlStringCommand, "Logo", DbType.String, distributor.Logo);
            this.database.AddInParameter(sqlStringCommand, "BackImage", DbType.String, distributor.BackImage);
            this.database.AddInParameter(sqlStringCommand, "ReferralOrders", DbType.Int64, distributor.ReferralOrders);
            this.database.AddInParameter(sqlStringCommand, "ReferralStatus", DbType.Int64, distributor.ReferralStatus);
            this.database.AddInParameter(sqlStringCommand, "ReferralBlance", DbType.Decimal, distributor.ReferralBlance);
            this.database.AddInParameter(sqlStringCommand, "ReferralRequestBalance", DbType.Decimal, distributor.ReferralRequestBalance);
            this.database.AddInParameter(sqlStringCommand, "StoreDescription", DbType.String, distributor.StoreDescription);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateDistributorById(string requestAccount, int distributorId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Distributors SET RequestAccount=@RequestAccount WHERE UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, distributorId);
            this.database.AddInParameter(sqlStringCommand, "RequestAccount", DbType.String, requestAccount);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateReferralUserId(int userid, int referralUserId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Distributors SET ReferralUserId=@ReferralUserId WHERE UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "ReferralUserId", DbType.Int32, referralUserId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateDistributorsOrderNum(string userid, string OrdersTotal)
        {
            string query = "";
            string str2 = query + "begin try  " + "  begin tran TranUpdate";
            query = (str2 + "  UPDATE aspnet_Distributors set  OrdersTotal=OrdersTotal+" + OrdersTotal + ",ReferralOrders=ReferralOrders+1  WHERE UserId=" + userid + "; ") + " COMMIT TRAN TranUpdate" + "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateNotSetCalculationCommission(ArrayList UserIdList, ArrayList OrdersTotal)
        {
            string query = "";
            query = query + "begin try  " + "  begin tran TranUpdate";
            for (int i = 0; i < UserIdList.Count; i++)
            {
                object obj2 = query;
                query = string.Concat(new object[] { obj2, "  UPDATE aspnet_Distributors set OrdersTotal=OrdersTotal+", OrdersTotal[i], " WHERE UserId=", UserIdList[i], "; " });
            }
            query = query + " COMMIT TRAN TranUpdate" + "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

