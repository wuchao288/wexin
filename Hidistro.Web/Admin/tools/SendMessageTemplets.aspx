﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendMessageTemplets.aspx.cs" Inherits="Hidistro.UI.Web.Admin.tools.SendMessageTemplets"  MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
	  <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
	    <h1>消息提醒设置</h1>
	    <span>在这里可以管理微信的发送，以及配置和相关的模版设置。</span>
     </div>
	  <!-- 添加按钮-->
	  <!--结束-->
	  <!--数据列表区域-->
<div class="datalist">
	    <div class="search clearfix searcha">
			<ul>
            <li><img src="../images/wechat.jpg"  /></li>
                <li>微信</li>
                <li><a href="WeixinSettings.html" target="_blank">配置</a></li>
			</ul>
	</div>
	  <UI:Grid ID="grdEmailTemplets" runat="server" ShowHeader="true" DataKeyNames="MessageType" AutoGenerateColumns="false" GridLines="None" Width="100%">
	    <HeaderStyle CssClass="table_title" />
            <Columns>
                <asp:BoundField HeaderText="消息类型" ItemStyle-Width="200px" HeaderStyle-CssClass="td_right td_left" DataField="Name" />
                <asp:TemplateField HeaderText="微信" HeaderStyle-CssClass="td_left td_right_fff">                                                                                                       
                    <ItemTemplate>  
                    <table cellpadding="0" cellspacing="0" style="border:none;">
                    <tr>
                        <td style="border:none; width:10px;">
                            <asp:CheckBox runat="server" ID="chkWeixinMessage" MessageType='<%# Eval("Name")%>'  Checked='<%# Eval("SendWeixin")%>' /></td>
                        <td style="border:none;"><span class="submit_bianji"  MessageType='<%# Eval("Name")%>'><a href='<%# "EditTemplateId.aspx?MessageType="+Eval("MessageType")%>'>编辑</a></span></td>
                    </tr>
                    </table>                          
                    </ItemTemplate>
                </asp:TemplateField>                             
            </Columns>
        </UI:Grid>        
    <div class="Pg_15 Pg_010" style="text-align:center;"><asp:Button ID="btnSaveSendSetting" runat="server" OnClientClick="return PageIsValid();" Text="保存设置"  CssClass="submit_DAqueding"/></div>
</div>
</div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>
<script>
    $(function () {
        $('[MessageType="会员注册时"]').hide();

    });

</script>	
</asp:Content>