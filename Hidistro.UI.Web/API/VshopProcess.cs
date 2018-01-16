namespace Hidistro.UI.Web.API
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.VShop;
    using Hidistro.Messages;
    using Hidistro.SaleSystem.Vshop;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.SessionState;

    public class VshopProcess : IHttpHandler, IRequiresSessionState
    {
        private void AddCommissions(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            string msg = "";
            if (this.CheckAddCommissions(context, ref msg))
            {
                string str2 = context.Request["account"].Trim();
                decimal num = decimal.Parse(context.Request["commissionmoney"].Trim());
                BalanceDrawRequestInfo balancerequestinfo = new BalanceDrawRequestInfo
                {
                    MerchanCade = str2,
                    Amount = num
                };
                if (DistributorsBrower.AddBalanceDrawRequest(balancerequestinfo))
                {
                    msg = "{\"success\":true,\"msg\":\"申请成功！\"}";
                }
                else
                {
                    msg = "{\"success\":false,\"msg\":\"真实姓名或手机号未填写！\"}";
                }
            }
            context.Response.Write(msg);
            context.Response.End();
        }

        public void AddDistributor(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            StringBuilder sb = new StringBuilder();
            
            //获取商城配置
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string nname = context.Request.Form["name"];
            string phone = context.Request.Form["phone"];
            string weixin = context.Request.Form["weixin"];
            //Utils.LogWriter.SaveLog("开始申请分销：");

            

            if (this.CheckRequestDistributors(context, sb))
            {
                DistributorsInfo distributors = new DistributorsInfo
                {
                    RequestAccount = "",
                    StoreName = masterSettings.SiteName,
                    StoreDescription = masterSettings.ShopIntroduction,
                    BackImage = "/Templates/vshop/default/images/logo_bg.jpg",
                    CellPhone = context.Request["phone"].Trim()
                };

                //Utils.LogWriter.SaveLog("开始申请分销2：");

                distributors.Logo = "/Templates/vshop/default/images/logo.jpg";

                int user_id = Globals.GetCurrentMemberUserId();

                MemberInfo member = MemberProcessor.GetMember(user_id);
                member.CellPhone = phone;
                member.MicroSignal = weixin;
                member.RealName = nname;
                StringBuilder builder = new StringBuilder();
                MemberProcessor.UpdateMember(member);

                if (DistributorsBrower.AddDistributors(distributors))
                {
                    if (HttpContext.Current.Request.Cookies["Vshop-Member"] != null)
                    {
                        string name = "Vshop-ReferralId";
                        HttpContext.Current.Response.Cookies[name].Expires = DateTime.Now.AddDays(-1.0);
                        HttpCookie cookie = new HttpCookie(name)
                        {
                            Value = Globals.GetCurrentMemberUserId().ToString(),
                            Expires = DateTime.Now.AddYears(10)
                        };
                        HttpContext.Current.Response.Cookies.Add(cookie);
                    }


                    context.Response.Write("OK");
                    context.Response.End();
                }
                else
                {
                    context.Response.Write("添加失败");
                    context.Response.End();
                }
            }
            else
            {
                context.Response.Write(sb.ToString() ?? "");
                context.Response.End();
            }
        }

        private void AddDistributorProducts(HttpContext context)
        {
            if (!string.IsNullOrEmpty(context.Request["Params"]))
            {
                string json = context.Request["Params"];
                JObject source = JObject.Parse(json);
                if (source.Count > 0)
                {
                    DistributorsBrower.AddDistributorProductId((from s in source.Values() select Convert.ToInt32(s)).ToList<int>());
                }
            }
            context.Response.Write("{\"success\":\"true\",\"msg\":\"保存成功\"}");
            context.Response.End();
        }

        private void AddFavorite(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"请先登录才可以收藏商品\"}");
            }
            else if (ProductBrowser.AddProductToFavorite(Convert.ToInt32(context.Request["ProductId"]), currentMember.UserId))
            {
                context.Response.Write("{\"success\":true}");
            }
            else
            {
                context.Response.Write("{\"success\":false, \"msg\":\"提交失败\"}");
            }
        }

        private void AddProductConsultations(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            ProductConsultationInfo productConsultation = new ProductConsultationInfo
            {
                ConsultationDate = DateTime.Now,
                ConsultationText = context.Request["ConsultationText"],
                ProductId = Convert.ToInt32(context.Request["ProductId"]),
                UserEmail = currentMember.Email,
                UserId = currentMember.UserId,
                UserName = currentMember.UserName
            };
            if (ProductBrowser.InsertProductConsultation(productConsultation))
            {
                context.Response.Write("{\"success\":true}");
            }
            else
            {
                context.Response.Write("{\"success\":false, \"msg\":\"提交失败\"}");
            }
        }

        private void AddProductReview(HttpContext context)
        {
            int num2;
            int num3;
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            int productId = Convert.ToInt32(context.Request["ProductId"]);
            ProductBrowser.LoadProductReview(productId, currentMember.UserId, out num2, out num3);
            if (num2 == 0)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"您没有购买此商品(或此商品的订单尚未完成)，因此不能进行评论\"}");
            }
            else if (num3 >= num2)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"您已经对此商品进行过评论(或此商品的订单尚未完成)，因此不能再次进行评论\"}");
            }
            else
            {
                ProductReviewInfo review = new ProductReviewInfo
                {
                    ReviewDate = DateTime.Now,
                    ReviewText = context.Request["ReviewText"],
                    ProductId = productId,
                    UserEmail = currentMember.Email,
                    UserId = currentMember.UserId,
                    UserName = currentMember.UserName
                };
                if (ProductBrowser.InsertProductReview(review))
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false, \"msg\":\"提交失败\"}");
                }
            }
        }

        private void AddShippingAddress(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                ShippingAddressInfo shippingAddress = new ShippingAddressInfo
                {
                    Address = context.Request.Form["address"],
                    CellPhone = context.Request.Form["cellphone"],
                    ShipTo = context.Request.Form["shipTo"],
                    Zipcode = "12345",
                    IsDefault = true,
                    UserId = currentMember.UserId,
                    RegionId = Convert.ToInt32(context.Request.Form["regionSelectorValue"])
                };
                if (MemberProcessor.AddShippingAddress(shippingAddress) > 0)
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false}");
                }
            }
        }

        private void AddSignUp(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int activityid = Convert.ToInt32(context.Request["id"]);
            string str = Convert.ToString(context.Request["code"]);
            LotteryTicketInfo lotteryTicket = VshopBrowser.GetLotteryTicket(activityid);
            if (!string.IsNullOrEmpty(lotteryTicket.InvitationCode) && (lotteryTicket.InvitationCode != str))
            {
                context.Response.Write("{\"success\":false, \"msg\":\"邀请码不正确\"}");
            }
            else if (lotteryTicket.EndTime < DateTime.Now)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"活动已结束\"}");
            }
            else if (lotteryTicket.OpenTime < DateTime.Now)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"报名已结束\"}");
            }
            else if (VshopBrowser.GetUserPrizeRecord(activityid) == null)
            {
                PrizeRecordInfo model = new PrizeRecordInfo
                {
                    ActivityID = activityid
                };
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                model.UserID = currentMember.UserId;
                model.UserName = currentMember.UserName;
                model.IsPrize = true;
                model.Prizelevel = "已报名";
                model.PrizeTime = new DateTime?(DateTime.Now);
                VshopBrowser.AddPrizeRecord(model);
                context.Response.Write("{\"success\":true, \"msg\":\"报名成功\"}");
            }
            else
            {
                context.Response.Write("{\"success\":false, \"msg\":\"你已经报名了，请不要重复报名！\"}");
            }
        }

        private void AddTicket(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int activityid = Convert.ToInt32(context.Request["activityid"]);
            LotteryTicketInfo lotteryTicket = VshopBrowser.GetLotteryTicket(activityid);
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if ((currentMember != null) && !lotteryTicket.GradeIds.Contains(currentMember.GradeId.ToString()))
            {
                context.Response.Write("{\"success\":false, \"msg\":\"您的会员等级不在此活动范内\"}");
            }
            else if (lotteryTicket.EndTime < DateTime.Now)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"活动已结束\"}");
            }
            else if (DateTime.Now < lotteryTicket.OpenTime)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"抽奖还未开始\"}");
            }
            else if (VshopBrowser.GetCountBySignUp(activityid) < lotteryTicket.MinValue)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"还未达到人数下限\"}");
            }
            else
            {
                PrizeRecordInfo userPrizeRecord = VshopBrowser.GetUserPrizeRecord(activityid);
                try
                {
                    if (!lotteryTicket.IsOpened)
                    {
                        VshopBrowser.OpenTicket(activityid);
                        userPrizeRecord = VshopBrowser.GetUserPrizeRecord(activityid);
                    }
                    else if (!string.IsNullOrWhiteSpace(userPrizeRecord.RealName) && !string.IsNullOrWhiteSpace(userPrizeRecord.CellPhone))
                    {
                        context.Response.Write("{\"success\":false, \"msg\":\"您已经抽过奖了\"}");
                        return;
                    }
                    if ((userPrizeRecord == null) || string.IsNullOrEmpty(userPrizeRecord.PrizeName))
                    {
                        context.Response.Write("{\"success\":false, \"msg\":\"很可惜,你未中奖\"}");
                        return;
                    }
                    if (!userPrizeRecord.PrizeTime.HasValue)
                    {
                        userPrizeRecord.PrizeTime = new DateTime?(DateTime.Now);
                        VshopBrowser.UpdatePrizeRecord(userPrizeRecord);
                    }
                }
                catch (Exception exception)
                {
                    context.Response.Write("{\"success\":false, \"msg\":\"" + exception.Message + "\"}");
                    return;
                }
                context.Response.Write("{\"success\":true, \"msg\":\"恭喜你获得" + userPrizeRecord.Prizelevel + "\"}");
            }
        }

        private void AddUserPrize(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int result = 1;
            int.TryParse(context.Request["activityid"], out result);
            string str = context.Request["prize"];
            LotteryActivityInfo lotteryActivity = VshopBrowser.GetLotteryActivity(result);
            PrizeRecordInfo model = new PrizeRecordInfo
            {
                PrizeTime = new DateTime?(DateTime.Now),
                UserID = Globals.GetCurrentMemberUserId(),
                ActivityName = lotteryActivity.ActivityName,
                ActivityID = result,
                Prizelevel = str
            };
            switch (str)
            {
                case "一等奖":
                    model.PrizeName = lotteryActivity.PrizeSettingList[0].PrizeName;
                    model.IsPrize = true;
                    break;

                case "二等奖":
                    model.PrizeName = model.PrizeName = lotteryActivity.PrizeSettingList[1].PrizeName;
                    model.IsPrize = true;
                    break;

                case "三等奖":
                    model.PrizeName = lotteryActivity.PrizeSettingList[2].PrizeName;
                    model.IsPrize = true;
                    break;

                case "四等奖":
                    model.PrizeName = lotteryActivity.PrizeSettingList[3].PrizeName;
                    model.IsPrize = true;
                    break;

                case "五等奖":
                    model.PrizeName = lotteryActivity.PrizeSettingList[4].PrizeName;
                    model.IsPrize = true;
                    break;

                case "六等奖":
                    model.PrizeName = lotteryActivity.PrizeSettingList[5].PrizeName;
                    model.IsPrize = true;
                    break;

                default:
                    model.IsPrize = false;
                    break;
            }
            VshopBrowser.AddPrizeRecord(model);
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            builder.Append("\"Status\":\"OK\"");
            builder.Append("}");
            context.Response.Write(builder);
        }

        private bool CheckAddCommissions(HttpContext context, ref string msg)
        {
            if (string.IsNullOrEmpty(context.Request["account"].Trim()))
            {
                msg = "{\"success\":false,\"msg\":\"支付宝账号不允许为空！\"}";
                return false;
            }
            if (string.IsNullOrEmpty(context.Request["commissionmoney"].Trim()))
            {
                msg = "{\"success\":false,\"msg\":\"提现金额不允许为空！\"}";
                return false;
            }
            if (decimal.Parse(context.Request["commissionmoney"].Trim()) <= 0M)
            {
                msg = "{\"success\":false,\"msg\":\"提现金额必须大于0的纯数字！\"}";
                return false;
            }
            decimal result = 0M;
            decimal.TryParse(SettingsManager.GetMasterSettings(false).MentionNowMoney, out result);
            if ((result > 0M) && ((decimal.Parse(context.Request["commissionmoney"].Trim()) % result) != 0M))
            {
                msg = "{\"success\":false,\"msg\":\"提现金额必须为" + result.ToString() + "的倍数！\"}";
                return false;
            }
            DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors();
            if (decimal.Parse(context.Request["commissionmoney"].Trim()) > currentDistributors.ReferralBlance)
            {
                msg = "{\"success\":false,\"msg\":\"提现金额必须为小于现有佣金余额！\"}";
                return false;
            }
            return true;
        }

        private void CheckFavorite(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else if (ProductBrowser.ExistsProduct(Convert.ToInt32(context.Request["ProductId"]), currentMember.UserId))
            {
                context.Response.Write("{\"success\":true}");
            }
            else
            {
                context.Response.Write("{\"success\":false}");
            }
        }

        private bool CheckRequestDistributors(HttpContext context, StringBuilder sb)
        {
            if (string.IsNullOrEmpty(context.Request["name"]))
            {
                sb.AppendFormat("请输入真实姓名!", new object[0]);
                return false;
            }
            if (string.IsNullOrEmpty(context.Request["weixin"]))
            {
                sb.AppendFormat("请输入手机号码!", new object[0]);
                return false;
            }
            if (string.IsNullOrEmpty(context.Request["weixin"]))
            {
                sb.AppendFormat("请输入微信号!", new object[0]);
                return false;
            }

            return true;
        }

        private bool CheckUpdateDistributors(HttpContext context, StringBuilder sb)
        {
            if (string.IsNullOrEmpty(context.Request["VDistributorInfo$txtstorename"]))
            {
                sb.AppendFormat("请输入店铺名称", new object[0]);
                return false;
            }
            if (context.Request["VDistributorInfo$txtstorename"].Length > 20)
            {
                sb.AppendFormat("请输入店铺名称字符不多于20个字符", new object[0]);
                return false;
            }
            if (string.IsNullOrEmpty(context.Request["VDistributorInfo$hdbackimg"]))
            {
                sb.AppendFormat("请选择店铺背景", new object[0]);
                return false;
            }
            if (!string.IsNullOrEmpty(context.Request["VDistributorInfo$txtdescription"]) && (context.Request["VDistributorInfo$txtdescription"].Trim().Length > 30))
            {
                sb.AppendFormat("店铺描述字不能多于30个字", new object[0]);
                return false;
            }
            return true;
        }

        private void DeleteDistributorProducts(HttpContext context)
        {
            if (!string.IsNullOrEmpty(context.Request["Params"]))
            {
                string json = context.Request["Params"];
                JObject source = JObject.Parse(json);
                if (source.Count > 0)
                {
                    DistributorsBrower.DeleteDistributorProductIds((from s in source.Values() select Convert.ToInt32(s)).ToList<int>());
                }
            }
            context.Response.Write("{\"success\":\"true\",\"msg\":\"保存成功\"}");
            context.Response.End();
        }

        private void DelFavorite(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            if (ProductBrowser.DeleteFavorite(Convert.ToInt32(context.Request["favoriteId"])) == 1)
            {
                context.Response.Write("{\"success\":true}");
            }
            else
            {
                context.Response.Write("{\"success\":false, \"msg\":\"取消失败\"}");
            }
        }

        private void DelShippingAddress(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                int userId = currentMember.UserId;
                if (MemberProcessor.DelShippingAddress(Convert.ToInt32(context.Request.Form["shippingid"]), userId))
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false}");
                }
            }
        }

        private void FinishOrder(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(Convert.ToString(context.Request["orderId"]));
            if ((orderInfo != null) && MemberProcessor.ConfirmOrderFinish(orderInfo))
            {
                DistributorsBrower.UpdateCalculationCommission(orderInfo);
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                MemberProcessor.RemoveUserCache(orderInfo.UserId);
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                DistributorsInfo userIdDistributors = new DistributorsInfo();
                userIdDistributors = DistributorsBrower.GetUserIdDistributors(orderInfo.UserId);
                int num = 0;
                if ((masterSettings.IsRequestDistributor && ((userIdDistributors == null) || (userIdDistributors.UserId == 0))) && (!string.IsNullOrEmpty(masterSettings.FinishedOrderMoney.ToString()) && (currentMember.Expenditure >= masterSettings.FinishedOrderMoney)))
                {
                    num = 1;
                }
                context.Response.Write("{\"success\":true,\"isapply\":" + num + "}");
            }
            else
            {
                context.Response.Write("{\"success\":false, \"msg\":\"订单当前状态不允许完成\"}");
            }
        }

        private string GenerateOrderId()
        {
            string str = string.Empty;
            Random random = new Random();
            for (int i = 0; i < 7; i++)
            {
                int num = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num % 10)))).ToString();
            }
            return (DateTime.Now.ToString("yyyyMMdd") + str);
        }

        private void GetPrize(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int result = 1;
            int.TryParse(context.Request["activityid"], out result);
            LotteryActivityInfo lotteryActivity = VshopBrowser.GetLotteryActivity(result);
            int userPrizeCount = VshopBrowser.GetUserPrizeCount(result);
            if (MemberProcessor.GetCurrentMember() == null)
            {
                MemberInfo member = new MemberInfo();
                string generateId = Globals.GetGenerateId();
                member.GradeId = MemberProcessor.GetDefaultMemberGrade();
                member.UserName = "";
                member.OpenId = "";
                member.CreateDate = DateTime.Now;
                member.SessionId = generateId;
                member.SessionEndTime = DateTime.Now;
                MemberProcessor.CreateMember(member);
                member = MemberProcessor.GetMember(generateId);
                HttpCookie cookie = new HttpCookie("Vshop-Member")
                {
                    Value = member.UserId.ToString(),
                    Expires = DateTime.Now.AddYears(10)
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (userPrizeCount >= lotteryActivity.MaxNum)
            {
                builder.Append("\"No\":\"-1\"");
                builder.Append("}");
                context.Response.Write(builder.ToString());
            }
            else if ((DateTime.Now < lotteryActivity.StartTime) || (DateTime.Now > lotteryActivity.EndTime))
            {
                builder.Append("\"No\":\"-3\"");
                builder.Append("}");
                context.Response.Write(builder.ToString());
            }
            else
            {
                PrizeQuery page = new PrizeQuery
                {
                    ActivityId = result
                };
                List<PrizeRecordInfo> prizeList = VshopBrowser.GetPrizeList(page);
                int num3 = 0;
                int num4 = 0;
                int num5 = 0;
                int num6 = 0;
                int num7 = 0;
                int num8 = 0;
                if ((prizeList != null) && (prizeList.Count > 0))
                {
                    num3 = prizeList.Count<PrizeRecordInfo>(a => a.Prizelevel == "一等奖");
                    num4 = prizeList.Count<PrizeRecordInfo>(a => a.Prizelevel == "二等奖");
                    num5 = prizeList.Count<PrizeRecordInfo>(a => a.Prizelevel == "三等奖");
                }
                PrizeRecordInfo model = new PrizeRecordInfo
                {
                    PrizeTime = new DateTime?(DateTime.Now),
                    UserID = Globals.GetCurrentMemberUserId(),
                    ActivityName = lotteryActivity.ActivityName,
                    ActivityID = result,
                    IsPrize = true
                };
                List<PrizeSetting> prizeSettingList = lotteryActivity.PrizeSettingList;
                decimal num9 = prizeSettingList[0].Probability * 100M;
                decimal num10 = prizeSettingList[1].Probability * 100M;
                decimal num11 = prizeSettingList[2].Probability * 100M;
                int num15 = new Random(Guid.NewGuid().GetHashCode()).Next(1, 0x2711);
                if (prizeSettingList.Count > 3)
                {
                    decimal num12 = prizeSettingList[3].Probability * 100M;
                    decimal num13 = prizeSettingList[4].Probability * 100M;
                    decimal num14 = prizeSettingList[5].Probability * 100M;
                    num6 = prizeList.Count<PrizeRecordInfo>(a => a.Prizelevel == "四等奖");
                    num7 = prizeList.Count<PrizeRecordInfo>(a => a.Prizelevel == "五等奖");
                    num8 = prizeList.Count<PrizeRecordInfo>(a => a.Prizelevel == "六等奖");
                    if ((num15 < num9) && (prizeSettingList[0].PrizeNum > num3))
                    {
                        builder.Append("\"No\":\"9\"");
                        model.Prizelevel = "一等奖";
                        model.PrizeName = prizeSettingList[0].PrizeName;
                    }
                    else if ((num15 < num10) && (prizeSettingList[1].PrizeNum > num4))
                    {
                        builder.Append("\"No\":\"11\"");
                        model.Prizelevel = "二等奖";
                        model.PrizeName = prizeSettingList[1].PrizeName;
                    }
                    else if ((num15 < num11) && (prizeSettingList[2].PrizeNum > num5))
                    {
                        builder.Append("\"No\":\"1\"");
                        model.Prizelevel = "三等奖";
                        model.PrizeName = prizeSettingList[2].PrizeName;
                    }
                    else if ((num15 < num12) && (prizeSettingList[3].PrizeNum > num6))
                    {
                        builder.Append("\"No\":\"3\"");
                        model.Prizelevel = "四等奖";
                        model.PrizeName = prizeSettingList[3].PrizeName;
                    }
                    else if ((num15 < num13) && (prizeSettingList[4].PrizeNum > num7))
                    {
                        builder.Append("\"No\":\"5\"");
                        model.Prizelevel = "五等奖";
                        model.PrizeName = prizeSettingList[4].PrizeName;
                    }
                    else if ((num15 < num14) && (prizeSettingList[5].PrizeNum > num8))
                    {
                        builder.Append("\"No\":\"7\"");
                        model.Prizelevel = "六等奖";
                        model.PrizeName = prizeSettingList[5].PrizeName;
                    }
                    else
                    {
                        model.IsPrize = false;
                        builder.Append("\"No\":\"0\"");
                    }
                }
                else if ((num15 < num9) && (prizeSettingList[0].PrizeNum > num3))
                {
                    builder.Append("\"No\":\"9\"");
                    model.Prizelevel = "一等奖";
                    model.PrizeName = prizeSettingList[0].PrizeName;
                }
                else if ((num15 < num10) && (prizeSettingList[1].PrizeNum > num4))
                {
                    builder.Append("\"No\":\"11\"");
                    model.Prizelevel = "二等奖";
                    model.PrizeName = prizeSettingList[1].PrizeName;
                }
                else if ((num15 < num11) && (prizeSettingList[2].PrizeNum > num5))
                {
                    builder.Append("\"No\":\"1\"");
                    model.Prizelevel = "三等奖";
                    model.PrizeName = prizeSettingList[2].PrizeName;
                }
                else
                {
                    model.IsPrize = false;
                    builder.Append("\"No\":\"0\"");
                }
                builder.Append("}");
                if (context.Request["activitytype"] != "scratch")
                {
                    VshopBrowser.AddPrizeRecord(model);
                }
                context.Response.Write(builder.ToString());
            }
        }

        public void GetShippingTypes(HttpContext context)
        {
            int num2;
            int regionId = Convert.ToInt32(context.Request["regionId"]);
            int groupbuyId = !string.IsNullOrWhiteSpace(context.Request["groupBuyId"]) ? Convert.ToInt32(context.Request["groupBuyId"]) : 0;
            ShoppingCartInfo shoppingCart = null;
            if (int.TryParse(context.Request["buyAmount"], out num2) && !string.IsNullOrWhiteSpace(context.Request["productSku"]))
            {
                string productSkuId = Convert.ToString(context.Request["productSku"]);
                if (groupbuyId > 0)
                {
                    shoppingCart = ShoppingCartProcessor.GetGroupBuyShoppingCart(groupbuyId, productSkuId, num2);
                }
                else
                {
                    shoppingCart = ShoppingCartProcessor.GetShoppingCart(productSkuId, num2);
                }
            }
            else
            {
                shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            }
            IEnumerable<int> source = from item in ShoppingProcessor.GetShippingModes() select item.ModeId;
            StringBuilder builder = new StringBuilder();
            if ((source != null) && (source.Count<int>() > 0))
            {
                foreach (int num4 in source)
                {
                    ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(num4, true);
                    decimal num5 = 0M;
                    if (shoppingCart.LineItems.Count != shoppingCart.LineItems.Count<ShoppingCartItemInfo>(a => a.IsfreeShipping))
                    {
                        num5 = ShoppingProcessor.CalcFreight(regionId, shoppingCart.Weight, shippingMode);
                    }
                    builder.Append(",{\"modelId\":\"" + shippingMode.ModeId.ToString() + "\",\"text\":\"" + shippingMode.Name + "： ￥" + num5.ToString("F2") + "\",\"freight\":\"" + num5.ToString("F2") + "\"}");
                }
                if (builder.Length > 0)
                {
                    builder.Remove(0, 1);
                }
            }
            builder.Insert(0, "{\"data\":[").Append("]}");
            context.Response.ContentType = "application/json";
            context.Response.Write(builder.ToString());
        }

        private void ProcessAddToCartBySkus(HttpContext context)
        {
            if (MemberProcessor.GetCurrentMember() == null)
            {
                context.Response.Write("{\"Status\":-1}");
            }
            else
            {
                context.Response.ContentType = "application/json";
                int quantity = int.Parse(context.Request["quantity"], NumberStyles.None);
                string skuId = context.Request["productSkuId"];
                ShoppingCartProcessor.AddLineItem(skuId, quantity);
                ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
                context.Response.Write("{\"Status\":\"OK\",\"TotalMoney\":\"" + shoppingCart.GetTotal().ToString(".00") + "\",\"Quantity\":\"" + shoppingCart.GetQuantity().ToString() + "\"}");
            }
        }

        private void ProcessChageQuantity(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string skuId = context.Request["skuId"];
            int result = 1;
            int.TryParse(context.Request["quantity"], out result);
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            int skuStock = ShoppingCartProcessor.GetSkuStock(skuId);
            if (result > skuStock)
            {
                builder.AppendFormat("\"Status\":\"{0}\"", skuStock);
                result = skuStock;
            }
            else
            {
                builder.Append("\"Status\":\"OK\",");
                ShoppingCartProcessor.UpdateLineItemQuantity(skuId, (result > 0) ? result : 1);
                builder.AppendFormat("\"TotalPrice\":\"{0}\"", ShoppingCartProcessor.GetShoppingCart().GetAmount());
            }
            builder.Append("}");
            context.Response.ContentType = "application/json";
            context.Response.Write(builder.ToString());
        }

        private void ProcessDeleteCartProduct(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string skuId = context.Request["skuId"];
            StringBuilder builder = new StringBuilder();
            ShoppingCartProcessor.RemoveLineItem(skuId);
            builder.Append("{");
            builder.Append("\"Status\":\"OK\"");
            builder.Append("}");
            context.Response.ContentType = "application/json";
            context.Response.Write(builder.ToString());
        }

        private void ProcessGetSkuByOptions(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int productId = int.Parse(context.Request["productId"], NumberStyles.None);
            string str = context.Request["options"];
            if (string.IsNullOrEmpty(str))
            {
                context.Response.Write("{\"Status\":\"0\"}");
            }
            else
            {
                if (str.EndsWith(","))
                {
                    str = str.Substring(0, str.Length - 1);
                }
                SKUItem item = ShoppingProcessor.GetProductAndSku(MemberProcessor.GetCurrentMember(), productId, str);
                if (item == null)
                {
                    context.Response.Write("{\"Status\":\"1\"}");
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("{");
                    builder.Append("\"Status\":\"OK\",");
                    builder.AppendFormat("\"SkuId\":\"{0}\",", item.SkuId);
                    builder.AppendFormat("\"SKU\":\"{0}\",", item.SKU);
                    builder.AppendFormat("\"Weight\":\"{0}\",", item.Weight);
                    builder.AppendFormat("\"Stock\":\"{0}\",", item.Stock);
                    builder.AppendFormat("\"SalePrice\":\"{0}\"", item.SalePrice.ToString("F2"));
                    builder.Append("}");
                    context.Response.ContentType = "application/json";
                    context.Response.Write(builder.ToString());
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            switch (context.Request["action"])
            {
                case "AddToCartBySkus":
                    this.ProcessAddToCartBySkus(context);
                    return;

                case "GetSkuByOptions":
                    this.ProcessGetSkuByOptions(context);
                    return;

                case "DeleteCartProduct":
                    this.ProcessDeleteCartProduct(context);
                    return;

                case "ChageQuantity":
                    this.ProcessChageQuantity(context);
                    return;

                case "Submmitorder":
                    this.ProcessSubmmitorder(context);
                    return;

                case "SubmitMemberCard":
                    this.ProcessSubmitMemberCard(context);
                    return;

                case "AddShippingAddress":
                    this.AddShippingAddress(context);
                    return;

                case "DelShippingAddress":
                    this.DelShippingAddress(context);
                    return;

                case "SetDefaultShippingAddress":
                    this.SetDefaultShippingAddress(context);
                    return;

                case "UpdateShippingAddress":
                    this.UpdateShippingAddress(context);
                    return;

                case "GetPrize":
                    this.GetPrize(context);
                    return;

                case "Vote":
                    this.Vote(context);
                    return;

                case "SubmitActivity":
                    this.SubmitActivity(context);
                    return;

                case "AddSignUp":
                    this.AddSignUp(context);
                    return;

                case "AddTicket":
                    this.AddTicket(context);
                    return;

                case "FinishOrder":
                    this.FinishOrder(context);
                    return;

                case "AddUserPrize":
                    this.AddUserPrize(context);
                    return;

                case "SubmitWinnerInfo":
                    this.SubmitWinnerInfo(context);
                    return;

                case "SetUserName":
                    this.SetUserName(context);
                    return;

                case "AddProductConsultations":
                    this.AddProductConsultations(context);
                    return;

                case "AddProductReview":
                    this.AddProductReview(context);
                    return;

                case "AddFavorite":
                    this.AddFavorite(context);
                    return;

                case "DelFavorite":
                    this.DelFavorite(context);
                    return;

                case "CheckFavorite":
                    this.CheckFavorite(context);
                    return;

                case "Logistic":
                    this.SearchExpressData(context);
                    return;

                case "GetShippingTypes":
                    this.GetShippingTypes(context);
                    return;

                case "UserLogin":
                    this.UserLogin(context);
                    return;

                case "RegisterUser":
                    this.RegisterUser(context);
                    return;

                case "AddDistributor":
                    this.AddDistributor(context);
                    return;

                case "SetDistributorMsg":
                    this.SetDistributorMsg(context);
                    return;

                case "DeleteProducts":
                    this.DeleteDistributorProducts(context);
                    return;

                case "AddDistributorProducts":
                    this.AddDistributorProducts(context);
                    return;

                case "UpdateDistributor":
                    this.UpdateDistributor(context);
                    return;

                case "AddCommissions":
                    this.AddCommissions(context);
                    return;
            }
        }

        private void ProcessSubmitMemberCard(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                currentMember.Address = context.Request.Form.Get("address");
                currentMember.RealName = context.Request.Form.Get("name");
                currentMember.CellPhone = context.Request.Form.Get("phone");
                currentMember.QQ = context.Request.Form.Get("qq");
                if (!string.IsNullOrEmpty(currentMember.QQ))
                {
                    currentMember.Email = currentMember.QQ + "@qq.com";
                }
                currentMember.VipCardNumber = SettingsManager.GetMasterSettings(true).VipCardPrefix + currentMember.UserId.ToString();
                currentMember.VipCardDate = new DateTime?(DateTime.Now);
                string s = MemberProcessor.UpdateMember(currentMember) ? "{\"success\":true}" : "{\"success\":false}";
                context.Response.Write(s);
            }
        }

        private void ProcessSubmmitorder(HttpContext context)
        {
            int num4;
            int num5;
            context.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            int result = 0;
            int num2 = 0;
            int shippingId = 0;
            string str = context.Request["couponCode"];
            shippingId = int.Parse(context.Request["shippingId"]);
            bool flag = int.TryParse(context.Request["groupbuyId"], out num4);
            string str3 = context.Request["remark"];
            ShoppingCartInfo shoppingCart = null;
            if (((int.TryParse(context.Request["buyAmount"], out num5) && !string.IsNullOrEmpty(context.Request["productSku"])) && !string.IsNullOrEmpty(context.Request["from"])) && ((context.Request["from"] == "signBuy") || (context.Request["from"] == "groupBuy")))
            {
                string productSkuId = context.Request["productSku"];
                if (context.Request["from"] == "signBuy")
                {
                    shoppingCart = ShoppingCartProcessor.GetShoppingCart(productSkuId, num5);
                }
                else
                {
                    shoppingCart = ShoppingCartProcessor.GetGroupBuyShoppingCart(num4, productSkuId, num5);
                }
            }
            else
            {
                shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            }
            OrderInfo orderInfo = ShoppingProcessor.ConvertShoppingCartToOrder(shoppingCart, false, false);
            if (orderInfo == null)
            {
                builder.Append("\"Status\":\"None\"");
                goto Label_0564;
            }
            orderInfo.OrderId = this.GenerateOrderId();
            orderInfo.OrderDate = DateTime.Now;
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            orderInfo.UserId = currentMember.UserId;
            orderInfo.Username = currentMember.UserName;
            orderInfo.EmailAddress = currentMember.Email;
            orderInfo.RealName = currentMember.RealName;
            orderInfo.QQ = currentMember.QQ;
            orderInfo.Remark = str3;
            if (flag)
            {
                GroupBuyInfo groupBuy = GroupBuyBrowser.GetGroupBuy(num4);
                orderInfo.GroupBuyId = num4;
                orderInfo.NeedPrice = groupBuy.NeedPrice;
                orderInfo.GroupBuyStatus = groupBuy.Status;
            }
            orderInfo.OrderStatus = OrderStatus.WaitBuyerPay;
            orderInfo.RefundStatus = RefundStatus.None;
            orderInfo.ShipToDate = context.Request["shiptoDate"];
            if (HttpContext.Current.Request.Cookies["Vshop-ReferralId"] != null)
            {
                orderInfo.ReferralUserId = int.Parse(HttpContext.Current.Request.Cookies.Get("Vshop-ReferralId").Value);
            }
            else
            {
                orderInfo.ReferralUserId = 0;
            }
            ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(shippingId);
            if (shippingAddress != null)
            {
                orderInfo.ShippingRegion = RegionHelper.GetFullRegion(shippingAddress.RegionId, "，");
                orderInfo.RegionId = shippingAddress.RegionId;
                orderInfo.Address = shippingAddress.Address;
                orderInfo.ZipCode = shippingAddress.Zipcode;
                orderInfo.ShipTo = shippingAddress.ShipTo;
                orderInfo.TelPhone = shippingAddress.TelPhone;
                orderInfo.CellPhone = shippingAddress.CellPhone;
                MemberProcessor.SetDefaultShippingAddress(shippingId, MemberProcessor.GetCurrentMember().UserId);
            }
            if (int.TryParse(context.Request["shippingType"], out result))
            {
                ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(result, true);
                if (shippingMode != null)
                {
                    orderInfo.ShippingModeId = shippingMode.ModeId;
                    orderInfo.ModeName = shippingMode.Name;
                    if (shoppingCart.LineItems.Count != shoppingCart.LineItems.Count<ShoppingCartItemInfo>(a => a.IsfreeShipping))
                    {
                        orderInfo.AdjustedFreight = orderInfo.Freight = ShoppingProcessor.CalcFreight(orderInfo.RegionId, shoppingCart.Weight, shippingMode);
                    }
                    else
                    {
                        orderInfo.AdjustedFreight = orderInfo.Freight = 0M;
                    }
                }
            }
            if (int.TryParse(context.Request["paymentType"], out num2))
            {
                orderInfo.PaymentTypeId = num2;
                switch (num2)
                {
                    case 0:
                    case -1:
                        orderInfo.PaymentType = "货到付款";
                        orderInfo.Gateway = "hishop.plugins.payment.podrequest";
                        goto Label_0488;

                    case 0x58:
                        orderInfo.PaymentType = "微信支付";
                        orderInfo.Gateway = "hishop.plugins.payment.weixinrequest";
                        goto Label_0488;

                    case 0x63:
                        orderInfo.PaymentType = "线下付款";
                        orderInfo.Gateway = "hishop.plugins.payment.offlinerequest";
                        goto Label_0488;
                }
                PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(num2);
                if (paymentMode != null)
                {
                    orderInfo.PaymentTypeId = paymentMode.ModeId;
                    orderInfo.PaymentType = paymentMode.Name;
                    orderInfo.Gateway = paymentMode.Gateway;
                }
            }
        Label_0488:
            if (!string.IsNullOrEmpty(str))
            {
                CouponInfo info8 = ShoppingProcessor.UseCoupon(shoppingCart.GetTotal(), str);
                orderInfo.CouponName = info8.Name;
                if (info8.Amount.HasValue)
                {
                    orderInfo.CouponAmount = info8.Amount.Value;
                }
                orderInfo.CouponCode = str;
                orderInfo.CouponValue = info8.DiscountValue;
            }
            try
            {
                if (ShoppingProcessor.CreatOrder(orderInfo))
                {
                    ShoppingCartProcessor.ClearShoppingCart();
                    Messenger.OrderCreated(orderInfo, currentMember);
                    builder.Append("\"Status\":\"OK\",");
                    builder.AppendFormat("\"OrderId\":\"{0}\"", orderInfo.OrderId);
                }
                else
                {
                    builder.Append("\"Status\":\"Error\"");
                }
            }
            catch (OrderException exception)
            {
                builder.Append("\"Status\":\"Error\"");
                builder.AppendFormat(",\"ErrorMsg\":\"{0}\"", exception.Message);
            }
        Label_0564:
            builder.Append("}");
            context.Response.ContentType = "application/json";
            context.Response.Write(builder.ToString());
        }

        public void RegisterUser(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string username = context.Request["userName"];
            string sourceData = context.Request["password"];
            string str3 = context.Request["passagain"];

            int referralId = 0;
            HttpCookie cookie2 = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
            if (!((cookie2 == null) || string.IsNullOrEmpty(cookie2.Value)))
            {
                referralId = int.Parse(cookie2.Value);
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (sourceData == str3)
            {
                MemberInfo info = new MemberInfo();
                if (MemberProcessor.GetusernameMember(username) == null)
                {
                    MemberInfo member = new MemberInfo();
                    string generateId = Globals.GetGenerateId();
                    member.GradeId = MemberProcessor.GetDefaultMemberGrade();
                    member.UserName = username;
                    member.CreateDate = DateTime.Now;
                    member.SessionId = generateId;
                    member.SessionEndTime = DateTime.Now.AddYears(10);
                    member.Password = HiCryptographer.Md5Encrypt(sourceData);
                    member.ReferralUserId = referralId;

                    MemberProcessor.CreateMember(member);
                    MemberInfo info3 = MemberProcessor.GetMember(generateId);
                    if (HttpContext.Current.Request.Cookies["Vshop-Member"] != null)
                    {
                        HttpContext.Current.Response.Cookies["Vshop-Member"].Expires = DateTime.Now.AddDays(-1.0);
                        HttpCookie cookie = new HttpCookie("Vshop-Member")
                        {
                            Value = info3.UserId.ToString(),
                            Expires = DateTime.Now.AddYears(10)
                        };
                        HttpContext.Current.Response.Cookies.Add(cookie);
                    }
                    else
                    {
                        HttpCookie cookie3 = new HttpCookie("Vshop-Member")
                        {
                            Value = info3.UserId.ToString(),
                            Expires = DateTime.Now.AddYears(10)
                        };
                        HttpContext.Current.Response.Cookies.Add(cookie3);
                    }
                    context.Session["userid"] = info3.UserId.ToString();
                    builder.Append("\"Status\":\"OK\"");
                }
                else
                {
                    builder.Append("\"Status\":\"-1\"");
                }
            }
            else
            {
                builder.Append("\"Status\":\"-2\"");
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        private void SearchExpressData(HttpContext context)
        {
            string s = string.Empty;
            if (!string.IsNullOrEmpty(context.Request["OrderId"]))
            {
                string orderId = context.Request["OrderId"];
                OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
                if (((orderInfo != null) && ((orderInfo.OrderStatus == OrderStatus.SellerAlreadySent) || (orderInfo.OrderStatus == OrderStatus.Finished))) && !string.IsNullOrEmpty(orderInfo.ExpressCompanyAbb))
                {
                    s = Express.GetExpressData(orderInfo.ExpressCompanyAbb, orderInfo.ShipOrderNumber, 0);
                }
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(s);
            context.Response.End();
        }

        private void SetDefaultShippingAddress(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                int userId = currentMember.UserId;
                if (MemberProcessor.SetDefaultShippingAddress(Convert.ToInt32(context.Request.Form["shippingid"]), userId))
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false}");
                }
            }
        }

        public void SetDistributorMsg(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            currentMember.VipCardDate = new DateTime?(DateTime.Now);
            currentMember.CellPhone = context.Request["CellPhone"];
            currentMember.MicroSignal = context.Request["MicroSignal"];
            currentMember.RealName = context.Request["RealName"];
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (MemberProcessor.UpdateMember(currentMember))
            {
                builder.Append("\"Status\":\"OK\"");
            }
            else
            {
                builder.Append("\"Status\":\"Error\"");
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        public void SetUserName(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            currentMember.UserName = context.Request["userName"];
            currentMember.VipCardDate = new DateTime?(DateTime.Now);
            currentMember.CellPhone = context.Request["CellPhone"];
            currentMember.QQ = context.Request["QQ"];
            if (!string.IsNullOrEmpty(currentMember.QQ))
            {
                currentMember.Email = currentMember.QQ + "@qq.com";
            }
            currentMember.RealName = context.Request["RealName"];
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (MemberProcessor.UpdateMember(currentMember))
            {
                builder.Append("\"Status\":\"OK\"");
            }
            else
            {
                builder.Append("\"Status\":\"Error\"");
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        private void SubmitActivity(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                ActivityInfo activity = VshopBrowser.GetActivity(Convert.ToInt32(context.Request.Form.Get("id")));
                if ((DateTime.Now < activity.StartDate) || (DateTime.Now > activity.EndDate))
                {
                    context.Response.Write("{\"success\":false, \"msg\":\"报名还未开始或已结束\"}");
                }
                else
                {
                    ActivitySignUpInfo info = new ActivitySignUpInfo
                    {
                        ActivityId = Convert.ToInt32(context.Request.Form.Get("id")),
                        Item1 = context.Request.Form.Get("item1"),
                        Item2 = context.Request.Form.Get("item2"),
                        Item3 = context.Request.Form.Get("item3"),
                        Item4 = context.Request.Form.Get("item4"),
                        Item5 = context.Request.Form.Get("item5"),
                        RealName = currentMember.RealName,
                        SignUpDate = DateTime.Now,
                        UserId = currentMember.UserId,
                        UserName = currentMember.UserName
                    };
                    string s = VshopBrowser.SaveActivitySignUp(info) ? "{\"success\":true}" : "{\"success\":false, \"msg\":\"你已经报过名了,请勿重复报名\"}";
                    context.Response.Write(s);
                }
            }
        }

        private void SubmitWinnerInfo(HttpContext context)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                int activityId = Convert.ToInt32(context.Request.Form.Get("id"));
                string realName = context.Request.Form.Get("name");
                string cellPhone = context.Request.Form.Get("phone");
                string s = VshopBrowser.UpdatePrizeRecord(activityId, currentMember.UserId, realName, cellPhone) ? "{\"success\":true}" : "{\"success\":false}";
                context.Response.ContentType = "application/json";
                context.Response.Write(s);
            }
        }

        private void UpdateDistributor(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            StringBuilder sb = new StringBuilder();
            if (this.CheckUpdateDistributors(context, sb))
            {
                DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(Globals.GetCurrentMemberUserId());
                currentDistributors.StoreName = context.Request["VDistributorInfo$txtstorename"].Trim();
                currentDistributors.StoreDescription = context.Request["VDistributorInfo$txtdescription"].Trim();
                currentDistributors.BackImage = context.Request["VDistributorInfo$hdbackimg"].Trim();
                HttpPostedFile file = context.Request.Files["logo"];
                if ((file != null) && !string.IsNullOrEmpty(file.FileName))
                {
                    currentDistributors.Logo = this.UploadFileImages(context, file);
                }
                if (DistributorsBrower.UpdateDistributor(currentDistributors))
                {
                    context.Response.Write("OK");
                    context.Response.End();
                }
                else
                {
                    context.Response.Write("添加失败");
                    context.Response.End();
                }
            }
            else
            {
                context.Response.Write(sb.ToString() ?? "");
                context.Response.End();
            }
        }

        private void UpdateShippingAddress(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                ShippingAddressInfo shippingAddress = new ShippingAddressInfo
                {
                    Address = context.Request.Form["address"],
                    CellPhone = context.Request.Form["cellphone"],
                    ShipTo = context.Request.Form["shipTo"],
                    Zipcode = "12345",
                    UserId = currentMember.UserId,
                    ShippingId = Convert.ToInt32(context.Request.Form["shippingid"]),
                    RegionId = Convert.ToInt32(context.Request.Form["regionSelectorValue"])
                };
                if (MemberProcessor.UpdateShippingAddress(shippingAddress))
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false}");
                }
            }
        }

        private string UploadFileImages(HttpContext context, HttpPostedFile file)
        {
            string virtualPath = string.Empty;
            if ((file != null) && !string.IsNullOrEmpty(file.FileName))
            {
                string str2 = Globals.GetStoragePath() + "/Logo";
                string str3 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + Path.GetExtension(file.FileName);
                virtualPath = str2 + "/" + str3;
                string str4 = Path.GetExtension(file.FileName).ToLower();
                if ((!str4.Equals(".gif") && !str4.Equals(".jpg")) && (!str4.Equals(".png") && !str4.Equals(".bmp")))
                {
                    context.Response.Write("你上传的文件格式不正确！上传格式有(.gif、.jpg、.png、.bmp)");
                    context.Response.End();
                }
                if (file.ContentLength > 0x100000)
                {
                    context.Response.Write("你上传的文件不能大于1048576KB!请重新上传！");
                    context.Response.End();
                }
                file.SaveAs(context.Request.MapPath(virtualPath));
                return virtualPath;
            }
            context.Response.Write("图片上传失败!");
            context.Response.End();
            return virtualPath;
        }

        public void UserLogin(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo usernameMember = new MemberInfo();
            string username = context.Request["userName"];
            string sourceData = context.Request["password"];
            usernameMember = MemberProcessor.GetusernameMember(username);
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (usernameMember == null)
            {
                builder.Append("\"Status\":\"-1\"");
                builder.Append("}");
                context.Response.Write(builder.ToString());
            }
            else
            {
                if (usernameMember.Password == HiCryptographer.Md5Encrypt(sourceData))
                {
                    DistributorsInfo userIdDistributors = new DistributorsInfo();
                    userIdDistributors = DistributorsBrower.GetUserIdDistributors(usernameMember.UserId);
                    if ((userIdDistributors != null) && (userIdDistributors.UserId > 0))
                    {
                        HttpContext.Current.Response.Cookies["Vshop-ReferralId"].Expires = DateTime.Now.AddDays(-1.0);
                        HttpCookie cookie = new HttpCookie("Vshop-ReferralId")
                        {
                            Value = Globals.UrlEncode(userIdDistributors.UserId.ToString()),
                            Expires = DateTime.Now.AddYears(1)
                        };
                        HttpContext.Current.Response.Cookies.Add(cookie);
                    }
                    if (HttpContext.Current.Request.Cookies["Vshop-Member"] != null)
                    {
                        HttpContext.Current.Response.Cookies["Vshop-Member"].Expires = DateTime.Now.AddDays(-1.0);
                        HttpCookie cookie3 = new HttpCookie("Vshop-Member")
                        {
                            Value = usernameMember.UserId.ToString(),
                            Expires = DateTime.Now.AddYears(10)
                        };
                        HttpContext.Current.Response.Cookies.Add(cookie3);
                    }
                    else
                    {
                        HttpCookie cookie4 = new HttpCookie("Vshop-Member")
                        {
                            Value = Globals.UrlEncode(usernameMember.UserId.ToString()),
                            Expires = DateTime.Now.AddYears(1)
                        };
                        HttpContext.Current.Response.Cookies.Add(cookie4);
                    }
                    context.Session["userid"] = usernameMember.UserId.ToString();
                    builder.Append("\"Status\":\"OK\"");
                }
                else
                {
                    builder.Append("\"Status\":\"-2\"");
                }
                builder.Append("}");
                context.Response.Write(builder.ToString());
            }
        }

        private void Vote(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int result = 1;
            int.TryParse(context.Request["voteId"], out result);
            string itemIds = context.Request["itemIds"];
            itemIds = itemIds.Remove(itemIds.Length - 1);
            if (MemberProcessor.GetCurrentMember() == null)
            {
                MemberInfo member = new MemberInfo();
                string generateId = Globals.GetGenerateId();
                member.GradeId = MemberProcessor.GetDefaultMemberGrade();
                member.UserName = "";
                member.OpenId = "";
                member.CreateDate = DateTime.Now;
                member.SessionId = generateId;
                member.SessionEndTime = DateTime.Now;
                MemberProcessor.CreateMember(member);
                member = MemberProcessor.GetMember(generateId);
                HttpCookie cookie = new HttpCookie("Vshop-Member")
                {
                    Value = member.UserId.ToString(),
                    Expires = DateTime.Now.AddYears(10)
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (VshopBrowser.Vote(result, itemIds))
            {
                builder.Append("\"Status\":\"OK\"");
            }
            else
            {
                builder.Append("\"Status\":\"Error\"");
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

