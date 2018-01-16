namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class AddLotteryTicket : AdminPage
    {
        protected Button btnAddActivity;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarOpenDate;
        protected WebCalendar calendarStartDate;
        protected CheckBoxList cbList;
        protected CheckBox ChkOpen;
        protected DropDownList ddlHours;
        protected FileUpload fileUpload;
        protected TextBox txtActiveName;
        protected TextBox txtCode;
        protected TextBox txtdesc;
        protected TextBox txtKeyword;
        protected TextBox txtMinValue;
        protected TextBox txtPrize1;
        protected TextBox txtPrize1Num;
        protected TextBox txtPrize2;
        protected TextBox txtPrize2Num;
        protected TextBox txtPrize3;
        protected TextBox txtPrize3Num;
        protected TextBox txtPrize4;
        protected TextBox txtPrize4Num;
        protected TextBox txtPrize5;
        protected TextBox txtPrize5Num;
        protected TextBox txtPrize6;
        protected TextBox txtPrize6Num;

        protected void btnAddActivity_Click(object sender, EventArgs e)
        {
            if (ReplyHelper.HasReplyKey(this.txtKeyword.Text.Trim()))
            {
                this.ShowMsg("关键字重复!", false);
            }
            else if (!this.calendarStartDate.SelectedDate.HasValue)
            {
                this.ShowMsg("请选择活动开始时间", false);
            }
            else if (!this.calendarOpenDate.SelectedDate.HasValue)
            {
                this.ShowMsg("请选择抽奖开始时间", false);
            }
            else if (!this.calendarEndDate.SelectedDate.HasValue)
            {
                this.ShowMsg("请选择活动结束时间", false);
            }
            else
            {
                string str = string.Empty;
                if (this.fileUpload.HasFile)
                {
                    try
                    {
                        str = VShopHelper.UploadTopicImage(this.fileUpload.PostedFile);
                    }
                    catch
                    {
                        this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                        return;
                    }
                }
                string str2 = string.Empty;
                for (int i = 0; i < this.cbList.Items.Count; i++)
                {
                    if (this.cbList.Items[i].Selected)
                    {
                        str2 = str2 + "," + this.cbList.Items[i].Value;
                    }
                }
                if (!string.IsNullOrEmpty(str2))
                {
                    LotteryTicketInfo info = new LotteryTicketInfo {
                        GradeIds = str2,
                        MinValue = Convert.ToInt32(this.txtMinValue.Text),
                        InvitationCode = this.txtCode.Text.Trim(),
                        ActivityName = this.txtActiveName.Text,
                        ActivityKey = this.txtKeyword.Text,
                        ActivityDesc = this.txtdesc.Text,
                        ActivityPic = str,
                        ActivityType = 4,
                        StartTime = this.calendarStartDate.SelectedDate.Value,
                        OpenTime = this.calendarOpenDate.SelectedDate.Value.AddHours((double) this.ddlHours.SelectedIndex),
                        EndTime = this.calendarEndDate.SelectedDate.Value,
                        PrizeSettingList = new List<PrizeSetting>()
                    };
                    try
                    {
                        PrizeSetting item = new PrizeSetting {
                            PrizeName = this.txtPrize1.Text,
                            PrizeNum = Convert.ToInt32(this.txtPrize1Num.Text),
                            PrizeLevel = "一等奖"
                        };
                        info.PrizeSettingList.Add(item);
                        PrizeSetting setting2 = new PrizeSetting {
                            PrizeName = this.txtPrize2.Text,
                            PrizeNum = Convert.ToInt32(this.txtPrize2Num.Text),
                            PrizeLevel = "二等奖"
                        };
                        info.PrizeSettingList.Add(setting2);
                        PrizeSetting setting3 = new PrizeSetting {
                            PrizeName = this.txtPrize3.Text,
                            PrizeNum = Convert.ToInt32(this.txtPrize3Num.Text),
                            PrizeLevel = "三等奖"
                        };
                        info.PrizeSettingList.Add(setting3);
                    }
                    catch (FormatException)
                    {
                        this.ShowMsg("奖品数量格式错误", false);
                        return;
                    }
                    if (this.ChkOpen.Checked)
                    {
                        try
                        {
                            PrizeSetting setting4 = new PrizeSetting {
                                PrizeName = this.txtPrize4.Text,
                                PrizeNum = Convert.ToInt32(this.txtPrize4Num.Text),
                                PrizeLevel = "四等奖"
                            };
                            info.PrizeSettingList.Add(setting4);
                            PrizeSetting setting5 = new PrizeSetting {
                                PrizeName = this.txtPrize5.Text,
                                PrizeNum = Convert.ToInt32(this.txtPrize5Num.Text),
                                PrizeLevel = "五等奖"
                            };
                            info.PrizeSettingList.Add(setting5);
                            PrizeSetting setting6 = new PrizeSetting {
                                PrizeName = this.txtPrize6.Text,
                                PrizeNum = Convert.ToInt32(this.txtPrize6Num.Text),
                                PrizeLevel = "六等奖"
                            };
                            info.PrizeSettingList.Add(setting6);
                        }
                        catch (FormatException)
                        {
                            this.ShowMsg("奖品数量格式错误", false);
                            return;
                        }
                    }
                    int num2 = VShopHelper.SaveLotteryTicket(info);
                    if (num2 > 0)
                    {
                        ReplyInfo reply = new TextReplyInfo {
                            Keys = info.ActivityKey,
                            MatchType = MatchType.Equal,
                            MessageType = MessageType.Text,
                            ReplyType = ReplyType.Ticket,
                            ActivityId = num2
                        };
                        ReplyHelper.SaveReply(reply);
                        base.Response.Redirect("ManageLotteryTicket.aspx");
                    }
                }
                else
                {
                    this.ShowMsg("请选择活动会员等级", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.cbList.DataSource = MemberHelper.GetMemberGrades();
                this.cbList.DataTextField = "Name";
                this.cbList.DataValueField = "GradeId";
                this.cbList.DataBind();
            }
        }
    }
}

