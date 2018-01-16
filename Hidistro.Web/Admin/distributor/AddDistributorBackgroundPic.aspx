<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddDistributorBackgroundPic.aspx.cs" Inherits="Hidistro.UI.Web.Admin.distributor.AddDistributorBackgroundPic" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <style type="text/css">
        .pic_link {
            position: absolute;
            left: 300px;
            top: 0;
            width: 400px;
            height: 38px;
            line-height: 38px;
        }

            .pic_link input {
                display: inline !important; height: 30px; width: 330px; padding-left: 5px;
            }
    </style>

    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="title">
                <em>
                    <img src="../images/03.gif" width="32" height="32" /></em>
                <h1>首页背景图片</h1>
                <span class="font">首页上面的背景图</span>
            </div>
            <div class="formitem validator2">
                <ul>
                    <li><span class="formitemtitle Pw_100">图片上传：</span>
                        <asp:FileUpload ID="logoFile" runat="server" />
                    </li>
                    <li>
                        <span class="formitemtitle Pw_100">显示上传图片：</span>
                        建议上传600*342 JPG的图片
                      <p id="imgall" style="height: auto">
                          <img src='/Utility/pics/default.jpg?<%=DateTime.Now.ToFileTime().ToString() %>' />
                      </p>

                    </li>
                </ul>
                <div class="btn Pa_100 clearfix" style="position: relative;">
                    <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="submit_DAqueding float" />

                    <div class="pic_link">
                        跳转链接：<asp:TextBox ID="txtDefaultLink" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>

    </div>

    <div class="areacolumn clearfix" style="margin-top: 10px;">
        <div class="columnright">
            <div class="title">
                <em>
                    <img src="../images/03.gif" width="32" height="32" /></em>
                <h1>广告图片</h1>
                <span class="font">首页的广告图</span>
            </div>
            <div class="formitem validator2">
                <ul>
                    <li><span class="formitemtitle Pw_100">图片上传：</span>
                        <asp:FileUpload ID="FileUpload1" runat="server" />
                    </li>
                    <li>
                        <span class="formitemtitle Pw_100">显示上传图片：</span>
                        建议上传650*320 JGP的图片
                      <p id="P1" style="height: auto">
                          <img src='/templates/vshop/default/images/11.jpg?<%=DateTime.Now.ToFileTime().ToString() %>' />
                      </p>

                    </li>
                </ul>
                <div class="btn Pa_100 clearfix" style="position: relative;">
                    <asp:Button ID="Button1" runat="server" Text="保存" CssClass="submit_DAqueding float" />

                    <div class="pic_link">
                        跳转链接：<asp:TextBox ID="txtGuanggaotuLink" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>

    </div>

</asp:Content>
