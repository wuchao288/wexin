<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="qrcode.aspx.cs" Inherits="Hidistro.UI.Web.Admin.distributor.qrcode" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="title">
                <em>
                    <img src="../images/03.gif" width="32" height="32" /></em>
                <h1>二维码页面背景图</h1>
                <span class="font">上半截背景图</span>
            </div>
            <div class="formitem validator2">
                <ul>
                    <li><span class="formitemtitle Pw_100">图片上传：</span>
                        <asp:FileUpload ID="logoFile" runat="server" />
                    </li>
                    <li>
                        <span class="formitemtitle Pw_100">显示上传图片：</span>
                        建议上传600*342 png的图片
                      <p id="imgall" style="height:auto">
                          <img src='/Templates/vshop/default/images/qrcode/qrcode_01.png?<%=DateTime.Now.ToFileTime().ToString() %>'/>
                      </p>

                    </li>
                </ul>
                <ul class="btn Pa_100 clearfix">
                    <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="submit_DAqueding float" />
                </ul>
            </div>
        </div>

    </div>

    <div class="areacolumn clearfix" style="margin-top:10px;">
        <div class="columnright">
            <div class="title">
                <em>
                    <img src="../images/03.gif" width="32" height="32" /></em>
                <h1>二维码页面背景图</h1>
                <span class="font">下半截背景图</span>
            </div>
            <div class="formitem validator2">
                <ul>
                    <li><span class="formitemtitle Pw_100">图片上传：</span>
                        <asp:FileUpload ID="FileUpload1" runat="server" />
                    </li>
                    <li>
                        <span class="formitemtitle Pw_100">显示上传图片：</span>
                        建议上传886*451 png的图片
                      <p id="P1" style="height:auto">
                          <img src='/Templates/vshop/default/images/qrcode/qrcode_02.png?<%=DateTime.Now.ToFileTime().ToString() %>'/>
                      </p>

                    </li>
                </ul>
                <ul class="btn Pa_100 clearfix">
                    <asp:Button ID="Button1" runat="server" Text="保存" CssClass="submit_DAqueding float" />
                </ul>
            </div>
        </div>

    </div>

</asp:Content>
