namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Votes)]
    public class AddVote : AdminPage
    {
        protected Button btnAddVote;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        protected CheckBox checkIsBackup;
        protected FileUpload fileUpload;
        protected TextBox txtAddVoteName;
        protected TextBox txtKeys;
        protected TextBox txtMaxCheck;
        protected TextBox txtValues;

        private void btnAddVote_Click(object sender, EventArgs e)
        {
            if (ReplyHelper.HasReplyKey(this.txtKeys.Text.Trim()))
            {
                this.ShowMsg("关键字重复!", false);
            }
            else
            {
                string str = string.Empty;
                if (!this.fileUpload.HasFile)
                {
                    this.ShowMsg("请上传一张封面图", false);
                }
                else
                {
                    try
                    {
                        str = StoreHelper.UploadVoteImage(this.fileUpload.PostedFile);
                    }
                    catch
                    {
                        this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                        return;
                    }
                    if (this.calendarStartDate.SelectedDate.HasValue)
                    {
                        if (!this.calendarEndDate.SelectedDate.HasValue)
                        {
                            this.ShowMsg("请选择活动结束时间", false);
                        }
                        else
                        {
                            VoteInfo vote = new VoteInfo {
                                VoteName = Globals.HtmlEncode(this.txtAddVoteName.Text.Trim()),
                                Keys = this.txtKeys.Text.Trim(),
                                IsBackup = this.checkIsBackup.Checked
                            };
                            int result = 1;
                            if (int.TryParse(this.txtMaxCheck.Text.Trim(), out result))
                            {
                                vote.MaxCheck = result;
                            }
                            vote.ImageUrl = str;
                            vote.StartDate = this.calendarStartDate.SelectedDate.Value;
                            vote.EndDate = this.calendarEndDate.SelectedDate.Value;
                            IList<VoteItemInfo> list = null;
                            if (!string.IsNullOrEmpty(this.txtValues.Text.Trim()))
                            {
                                list = new List<VoteItemInfo>();
                                string[] strArray = this.txtValues.Text.Trim().Replace("\r\n", "\n").Replace("\n", "*").Split(new char[] { '*' });
                                for (int i = 0; i < strArray.Length; i++)
                                {
                                    VoteItemInfo item = new VoteItemInfo();
                                    if (strArray[i].Length > 60)
                                    {
                                        this.ShowMsg("投票选项长度限制在60个字符以内", false);
                                        return;
                                    }
                                    item.VoteItemName = Globals.HtmlEncode(strArray[i]);
                                    list.Add(item);
                                }
                            }
                            else
                            {
                                this.ShowMsg("投票选项不能为空", false);
                                return;
                            }
                            vote.VoteItems = list;
                            if (this.ValidationVote(vote))
                            {
                                if (StoreHelper.CreateVote(vote) > 0)
                                {
                                    this.ShowMsg("成功的添加了一个投票", true);
                                    this.txtAddVoteName.Text = string.Empty;
                                    this.checkIsBackup.Checked = false;
                                    this.txtMaxCheck.Text = string.Empty;
                                    this.txtValues.Text = string.Empty;
                                }
                                else
                                {
                                    this.ShowMsg("添加投票失败", false);
                                }
                            }
                        }
                    }
                    else
                    {
                        this.ShowMsg("请选择活动开始时间", false);
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnAddVote.Click += new EventHandler(this.btnAddVote_Click);
        }

        private bool ValidationVote(VoteInfo vote)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<VoteInfo>(vote, new string[] { "ValVote" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}

