﻿namespace Hidistro.UI.Web.Admin
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
    public class EditVote : AdminPage
    {
        protected Button btnEditVote;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        protected FileUpload fileUpload;
        protected HiImage imgPic;
        protected TextBox txtAddVoteName;
        protected TextBox txtKeys;
        protected TextBox txtMaxCheck;
        protected TextBox txtValues;
        private long voteId;

        private void btnEditVote_Click(object sender, EventArgs e)
        {
            if (StoreHelper.GetVoteCounts(this.voteId) > 0)
            {
                this.ShowMsg("投票已经开始，不能再对投票选项进行任何操作", false);
            }
            else
            {
                VoteInfo voteById = StoreHelper.GetVoteById(this.voteId);
                if ((voteById.Keys != this.txtKeys.Text.Trim()) && ReplyHelper.HasReplyKey(this.txtKeys.Text.Trim()))
                {
                    this.ShowMsg("关键字重复!", false);
                }
                else
                {
                    if (this.fileUpload.HasFile)
                    {
                        try
                        {
                            ResourcesHelper.DeleteImage(voteById.ImageUrl);
                            this.imgPic.ImageUrl = voteById.ImageUrl = StoreHelper.UploadVoteImage(this.fileUpload.PostedFile);
                        }
                        catch
                        {
                            this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                            return;
                        }
                    }
                    voteById.VoteName = Globals.HtmlEncode(this.txtAddVoteName.Text.Trim());
                    voteById.Keys = this.txtKeys.Text.Trim();
                    int result = 1;
                    if (int.TryParse(this.txtMaxCheck.Text.Trim(), out result))
                    {
                        voteById.MaxCheck = result;
                    }
                    voteById.StartDate = this.calendarStartDate.SelectedDate.Value;
                    voteById.EndDate = this.calendarEndDate.SelectedDate.Value;
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
                    voteById.VoteItems = list;
                    if (this.ValidationVote(voteById))
                    {
                        if (StoreHelper.UpdateVote(voteById))
                        {
                            this.ShowMsg("修改投票成功", true);
                        }
                        else
                        {
                            this.ShowMsg("修改投票失败", false);
                        }
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!long.TryParse(this.Page.Request.QueryString["VoteId"], out this.voteId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnEditVote.Click += new EventHandler(this.btnEditVote_Click);
                if (!this.Page.IsPostBack)
                {
                    VoteInfo voteById = StoreHelper.GetVoteById(this.voteId);
                    IList<VoteItemInfo> voteItems = StoreHelper.GetVoteItems(this.voteId);
                    if (voteById == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.txtAddVoteName.Text = Globals.HtmlDecode(voteById.VoteName);
                        this.txtKeys.Text = voteById.Keys;
                        this.txtMaxCheck.Text = voteById.MaxCheck.ToString();
                        this.calendarStartDate.SelectedDate = new DateTime?(voteById.StartDate);
                        this.calendarEndDate.SelectedDate = new DateTime?(voteById.EndDate);
                        this.imgPic.ImageUrl = voteById.ImageUrl;
                        string str = "";
                        foreach (VoteItemInfo info2 in voteItems)
                        {
                            str = str + Globals.HtmlDecode(info2.VoteItemName) + "\r\n";
                        }
                        this.txtValues.Text = str;
                    }
                }
            }
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

