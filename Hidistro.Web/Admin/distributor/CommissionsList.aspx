<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="CommissionsList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.distributor.CommissionsList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/04.gif" width="32" height="32" /></em>
            <h1>
                佣金明细列表</h1>
            <span>管理员查询分销商下的佣金明细。</span>
        </div>
        <!--搜索-->
        <!--数据列表区域-->
        <div class="datalist">
            <table class="table_title">
                <thead>
                    <tr>
                        <td>
                            时间
                        </td>
                        <td>
                            佣金
                        </td>
                        <td>
                            订单金额
                        </td>
                        <td>
                            店铺信息
                        </td>
                         
                    </tr>
                </thead>
                <asp:Repeater ID="reCommissions" runat="server">
                    <ItemTemplate>
                        <tbody>
                            <tr>
                                <td width="180">
                                    &nbsp;<%# Eval("TradeTime", "{0:yyyy-MM-dd hh:mm:ss}")%>
                                </td>
                                <td>
                                    &nbsp;<%# Eval("CommTotal","{0:F2}")%>
                                </td>
                                <td>
                                    &nbsp;<%# Eval("OrderTotal", "{0:F2}")%>
                                </td>
                                <td>
                                    &nbsp;<%# Eval("GradeId").ToString() == "1" ? "【一级】" : Eval("GradeId").ToString() == "2" ? "【二级】" : "【三级】"%><%# Eval("StoreName")%>
                                </td>
                                
                            </tr>
                        </tbody>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <div class="blank12 clearfix">
            </div>
        </div>
        <!--数据列表底部功能区域-->
        <div class="bottomPageNumber clearfix">
            <div class="pageNumber">
                <div class="pagination" style="width: auto">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(function () {

        });
        
    </script>
</asp:Content>
