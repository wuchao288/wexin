<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="DistributorDetails.aspx.cs" Inherits="Hidistro.UI.Web.Admin.distributor.DistributorDetails" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" Runat="Server">

<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/04.gif" width="32" height="32" /></em>
            <h1>查看“<asp:Literal runat="server" ID="litUserName" />”分销商信息</h1>
            <span>分销商的详细信息</span>
          </div>
        <div class="formitem clearfix">
          <ul>
          <li style="display:none;"><span class="formitemtitle Pw_110">分店名称：</span><asp:Literal runat="server" ID="lblStoreName" /></li>
          <li><span class="formitemtitle Pw_110">联系人：</span> <asp:Literal runat="server" ID="litRealName" /></li>
          <li><span class="formitemtitle Pw_110">手机号码：</span><asp:Literal runat="server" ID="litCellPhone" /></li>
          <li><span class="formitemtitle Pw_110">QQ：</span> <asp:Literal runat="server" ID="litQQ" /></li>
          <li><span class="formitemtitle Pw_110">微信：</span><asp:Literal runat="server" ID="litMicroSignal" /></li>
          <li><span class="formitemtitle Pw_110">等级：</span><asp:Literal runat="server" ID="litGreade" /></li>   
          <li><span class="formitemtitle Pw_110">有效推广订单：</span><asp:Literal runat="server" ID="litOrders" /></li>        
          <li><span class="formitemtitle Pw_110">有效推广佣金：</span><asp:Literal runat="server" ID="litCommission" /></li>
          <li><span class="formitemtitle Pw_110">所属上级：</span><asp:Literal runat="server" ID="litUpGrade" /> </li>
          <li><span class="formitemtitle Pw_110">拥有下级数：</span><asp:Literal runat="server" ID="litDownGradeNum" /> </li>
      </ul>
      <ul class="btn Pa_110">
        <asp:Button runat="server" ID="btnSubmit" CssClass="submit_DAqueding" Text="返回" />
        </ul>
      </div>

      </div>
  </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" Runat="Server">

</asp:Content>