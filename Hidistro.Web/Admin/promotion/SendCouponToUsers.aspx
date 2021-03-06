﻿<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SendCouponToUsers.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SendCouponToUsers" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">

 
  <div class="dataarea mainwidth databody">
    <div class="title m_none td_bottom"> 
      <em><img src="../images/06.gif" width="32" height="32" /></em>
      <h1>发送优惠券</h1>
      <span>给店铺会员发送优惠券，您可以给某一个等级的所有会员发送，也可以给某一部分会员发送</span>
    </div>
    <div class="datafrom">
    <div class="formitem">
    <ul>
    <li><span class="formitemtitle Pw_100">发送对象：</span><input type="radio" name="rdoList" value="1" onclick="selectMemberName()" checked="true"  id="rdoName" runat="server" />发送给指定的会员<input type="radio" name="rdoList" value="2" onclick="selectRank()" id="rdoRank" runat="server"  />发送给指定的会员等级</li>
    </ul>
    <ul>
    <li><span class="formitemtitle Pw_100">会员等级：</span><Hi:MemberGradeDropDownList ID="rankList" class="formselect input100" runat="server" AllowNull="true" NullToDisplay="全部" /></li>
    </ul>
      <ul>
        <li> <span class="formitemtitle Pw_100">OpenID号：</span>
          <asp:TextBox ID="txtMemberNames" runat="server" TextMode="MultiLine" Rows="8" Columns="50" ></asp:TextBox>
          <p class="Pa_100">一行一个OpenID号，您可以在会员列表中导出选定的OpenID号并复制进来</p>
        </li>
        </ul>
                <ul class="btntf Pa_100">           
                <asp:Button runat="server" ID="btnSend" Text="发 送" class="submit_DAqueding inbnt" />
          </ul>
      </div>
    </div>
</div>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript">

         function selectRank(){
            document.getElementById("ctl00_contentHolder_rankList").disabled = false;
            document.getElementById("ctl00_contentHolder_txtMemberNames").disabled = true;
        }


        function selectMemberName() {
            document.getElementById("ctl00_contentHolder_rankList").disabled = true;
            document.getElementById("ctl00_contentHolder_txtMemberNames").disabled = false;
        }

        $(document).ready(function() { selectMemberName(); });
</script>

</asp:Content>
