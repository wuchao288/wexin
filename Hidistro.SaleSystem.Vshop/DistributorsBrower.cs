namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.SqlDal.Members;
    using Hidistro.SqlDal.Orders;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Caching;

    public class DistributorsBrower
    {
        public static bool AddBalanceDrawRequest(BalanceDrawRequestInfo balancerequestinfo)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            DistributorsInfo currentDistributors = GetCurrentDistributors();

            if (((((currentMember != null) && !string.IsNullOrEmpty(currentMember.RealName)) && (currentDistributors != null) && (currentDistributors.UserId > 0))) && !string.IsNullOrEmpty(currentMember.CellPhone))
            {
                if (currentDistributors.RequestAccount != balancerequestinfo.MerchanCade)
                {
                    new DistributorsDao().UpdateDistributorById(balancerequestinfo.MerchanCade, currentMember.UserId);
                }
                balancerequestinfo.AccountName = currentMember.RealName;
                balancerequestinfo.UserId = currentMember.UserId;
                balancerequestinfo.UserName = currentMember.UserName;
                balancerequestinfo.MerchanCade = currentDistributors.RequestAccount;
                balancerequestinfo.RequesType = 1;
                balancerequestinfo.CellPhone = currentMember.CellPhone;
                return new DistributorsDao().AddBalanceDrawRequest(balancerequestinfo);
            }
            return false;
        }

        public static void AddDistributorProductId(List<int> productList)
        {
            int userId = GetCurrentDistributors().UserId;
            if ((userId > 0) && (productList.Count > 0))
            {
                new DistributorsDao().RemoveDistributorProducts(productList, userId);
                foreach (int num2 in productList)
                {
                    new DistributorsDao().AddDistributorProducts(num2, userId);
                }
            }
        }

        public static void AddDistributorProductId(List<int> productList, int userId)
        {
            if ((userId > 0) && (productList.Count > 0))
            {
                new DistributorsDao().RemoveDistributorProducts(productList, userId);
                foreach (int num2 in productList)
                {
                    new DistributorsDao().AddDistributorProducts(num2, userId);
                }
            }
        }

        public static bool AddDistributors(DistributorsInfo distributors)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();

            int ReferralUserId2 = 0;
            MemberInfo userInfo = MemberProcessor.GetMember(currentMember.UserId);
            //Utils.LogWriter.SaveLog("开始申请分销11：");
            DistributorsInfo parentDistributorsInfo = new DistributorsDao().GetDistributorInfo(userInfo.ReferralUserId);
            //Utils.LogWriter.SaveLog("开始申请分销22：");

            if (parentDistributorsInfo != null)
            {
                //Utils.LogWriter.SaveLog("开始申请分销33：");
                //上上级
                if (parentDistributorsInfo != null && parentDistributorsInfo.ReferralUserId != 0)
                {
                    ReferralUserId2 = parentDistributorsInfo.ReferralUserId;
                    //DistributorsInfo parentDistributorsInfo2 = new DistributorsDao().GetDistributorInfo(parentDistributorsInfo.ReferralUserId);
                    //if (parentDistributorsInfo != null && parentDistributorsInfo2.ReferralUserId > 0)
                    //{
                    //    ReferralUserId2 = parentDistributorsInfo.ReferralUserId;
                    //}
                }

                distributors.ParentUserId = userInfo.ReferralUserId;
            }

            //Utils.LogWriter.SaveLog("开始申请分销44：");

            //Utils.LogWriter.SaveLog("开始申请分销5：");
            distributors.DistributorGradeId = DistributorGrade.OneDistributor;
            distributors.ReferralUserId2 = ReferralUserId2;
            distributors.UserId = currentMember.UserId;
            DistributorsInfo currentDistributors = GetCurrentDistributors();
            if (currentDistributors != null)
            {
                if (currentDistributors.DistributorGradeId == DistributorGrade.OneDistributor)
                {
                    distributors.DistributorGradeId = DistributorGrade.TowDistributor;
                }
                else if (currentDistributors.DistributorGradeId == DistributorGrade.TowDistributor)
                {
                    distributors.DistributorGradeId = DistributorGrade.ThreeDistributor;
                }
                else
                {
                    distributors.DistributorGradeId = DistributorGrade.ThreeDistributor;
                }
            }
            return new DistributorsDao().CreateDistributor(distributors);
        }

        public static void DeleteDistributorProductIds(List<int> productList)
        {
            int userId = GetCurrentDistributors().UserId;
            if ((userId > 0) && (productList.Count > 0))
            {
                new DistributorsDao().RemoveDistributorProducts(productList, userId);
            }
        }

        public static DbQueryResult GetBalanceDrawRequest(BalanceDrawRequestQuery query)
        {
            return new DistributorsDao().GetBalanceDrawRequest(query);
        }

        public static DbQueryResult GetCommissions(CommissionsQuery query)
        {
            return new DistributorsDao().GetCommissions(query);
        }

        public static DistributorsInfo GetCurrentDistributors()
        {
            return GetCurrentDistributors(Globals.GetCurrentDistributorId());
        }

        public static DistributorsInfo GetCurrentDistributors(int userId)
        {
            DistributorsInfo distributorInfo = HiCache.Get(string.Format("DataCache-Distributor-{0}", userId)) as DistributorsInfo;
            if ((distributorInfo == null) || (distributorInfo.UserId == 0))
            {
                distributorInfo = new DistributorsDao().GetDistributorInfo(userId);
                HiCache.Insert(string.Format("DataCache-Distributor-{0}", userId), distributorInfo, 360, CacheItemPriority.Normal);
            }
            return distributorInfo;
        }

        public static DataTable GetCurrentDistributorsCommosion()
        {
            return new DistributorsDao().GetDistributorsCommosion(Globals.GetCurrentDistributorId());
        }

        public static DataTable GetCurrentDistributorsCommosion(int userId)
        {
            return new DistributorsDao().GetCurrentDistributorsCommosion(userId);
        }

        public static int GetDistributorNum()
        {
            return new DistributorsDao().GetDistributorNum();
        }

        public static DataSet GetDistributorOrder(OrderQuery query)
        {
            return new OrderDao().GetDistributorOrder(query);
        }

        public static int GetDistributorOrderCount(OrderQuery query)
        {
            return new OrderDao().GetDistributorOrderCount(query);
        }

        public static DbQueryResult GetDistributors(DistributorsQuery query)
        {
            return new DistributorsDao().GetDistributors(query);
        }

        public static DataTable GetDistributorsCommission(DistributorsQuery query)
        {
            return new DistributorsDao().GetDistributorsCommission(query);
        }

        public static DataTable GetReferralDistributor(int user_id)
        {
            return new DistributorsDao().GetReferralDistributor(user_id);
        }


        public static DataTable GetDistributorsCommosion(int userId, DistributorGrade distributorgrade)
        {
            return new DistributorsDao().GetDistributorsCommosion(userId, distributorgrade);
        }

        public static int GetDownDistributorNum(string userid)
        {
            return new DistributorsDao().GetDownDistributorNum(userid);
        }

        public static DistributorsInfo GetUserIdDistributors(int userid)
        {
            return new DistributorsDao().GetDistributorInfo(userid);
        }

        public static bool IsExitsCommionsRequest()
        {
            return new DistributorsDao().IsExitsCommionsRequest(Globals.GetCurrentDistributorId());
        }

        public static void RemoveDistributorCache(int userId)
        {
            HiCache.Remove(string.Format("DataCache-Distributor-{0}", userId));
        }

        public static bool UpdateCalculationCommission(OrderInfo order)
        {
            DistributorsInfo userIdDistributors = GetUserIdDistributors(order.ReferralUserId);
            bool flag = false;
            if (userIdDistributors != null)
            {
                Dictionary<string, LineItemInfo> lineItems = order.LineItems;
                LineItemInfo info2 = new LineItemInfo();
                DataView defaultView = CategoryBrowser.GetAllCategories().DefaultView;
                string str2 = null;
                string str3 = null;
                string str4 = null;
                decimal subTotal = 0M;
                foreach (KeyValuePair<string, LineItemInfo> pair in lineItems)
                {
                    string key = pair.Key;
                    info2 = pair.Value;
                    DataTable productCategories = ProductBrowser.GetProductCategories(info2.ProductId);
                    if ((productCategories.Rows.Count > 0) && (productCategories.Rows[0][0].ToString() != "0"))
                    {
                        defaultView.RowFilter = " CategoryId=" + productCategories.Rows[0][0];
                        str2 = defaultView[0]["FirstCommission"].ToString();
                        str3 = defaultView[0]["SecondCommission"].ToString();
                        str4 = defaultView[0]["ThirdCommission"].ToString();

                        //计算分销 一口价 减 成本价 乘以 佣金比例
                        if ((!string.IsNullOrEmpty(str2) && !string.IsNullOrEmpty(str3)) && !string.IsNullOrEmpty(str4))
                        {
                            ArrayList referralBlanceList = new ArrayList();
                            ArrayList userIdList = new ArrayList();
                            ArrayList ordersTotalList = new ArrayList();
                            subTotal = info2.GetSubTotal();

                            //三级分店
                            referralBlanceList.Add((decimal.Parse(str2.Split(new char[] { '|' })[0]) / 100M) * info2.GetSubCommission());
                            userIdList.Add(order.ReferralUserId);
                            ordersTotalList.Add(subTotal);

                            if (userIdDistributors.ParentUserId > 0)
                            {
                                referralBlanceList.Add((decimal.Parse(str3.Split(new char[] { '|' })[0]) / 100M) * info2.GetSubCommission());
                                userIdList.Add(userIdDistributors.ParentUserId);
                                ordersTotalList.Add(subTotal);

                                if (userIdDistributors.ReferralUserId2 > 0) {
                                    referralBlanceList.Add((decimal.Parse(str4.Split(new char[] { '|' })[0]) / 100M) * info2.GetSubCommission());
                                    userIdList.Add(userIdDistributors.ReferralUserId2);
                                    ordersTotalList.Add(subTotal);
                                }
                            }

                            flag = new DistributorsDao().UpdateCalculationCommission(userIdList, referralBlanceList, order.OrderId, ordersTotalList, order.UserId.ToString());
                        }
                    }
                }
                flag = new DistributorsDao().UpdateDistributorsOrderNum(order.ReferralUserId.ToString(), order.GetTotal().ToString());
                RemoveDistributorCache(userIdDistributors.UserId);
            }
            return flag;
        }

        public static bool UpdateDistributor(DistributorsInfo query)
        {
            return new DistributorsDao().UpdateDistributor(query);
        }
    }
}

