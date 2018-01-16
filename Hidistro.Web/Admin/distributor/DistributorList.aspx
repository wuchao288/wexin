<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="DistributorList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.distributor.DistributorList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script src="/admin/js/distributor/distributorlist.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/04.gif" width="32" height="32" /></em>
            <h1>分销商列表</h1>
            <span>对分销商进行管理，您可以查询分销商的佣金和详细信息。</span>

            <div class="btn">
                <a href="javascript:void(0);" class="submit_jia">添加分销商</a>
            </div>
        </div>

        <div id="divadd" style="display:none;">
            <div class="frame-content">
                <p><span class="frame-span frame-input90"><em>*</em>&nbsp;会员ID：</span><input  type="text" id="userid"/></p>
                <p><span class="frame-span frame-input90"><em>*</em>&nbsp;真实姓名：</span><input type="text" id="username" /></p>
                <p><span class="frame-span frame-input90"><em>*</em>&nbsp;手机：</span><input type="text" id="mobile" /></p>
                <p><span class="frame-span frame-input90"><em>*</em>&nbsp;微信：</span><input type="text" id="weixin" /></p>
                <p><span class="frame-span frame-input90"><em>*</em>&nbsp;邮箱：</span><input type="text" id="email" /></p>
                <p><span class="frame-span frame-input90">QQ：</span><input type="text" id="qq" /></p>
                <p><span class="frame-span frame-input90">推荐人ID：</span><input  type="text" id="parent_id"/></p>
                <p><span class="frame-span frame-input90">联系地址：</span><input type="text" id="address" /></p>
            </div>
        </div>
        <a href="javascript:void(0);" id="btnAdd" onclick="add();" style="display:none;">新增</a>

        <!--搜索-->
        <!--数据列表区域-->
        <div class="datalist">
            <div class="searcharea clearfix br_search">
                <ul>
                    <li><span>联系人：</span> <span>
                        <asp:TextBox ID="txtRealName" CssClass="forminput" runat="server" /></span>
                    </li>
                    <li><span>手机号码：</span> <span>
                        <asp:TextBox ID="txtCellPhone" CssClass="forminput" runat="server" /></span>
                    </li>
                    <li><span>微信号：</span> <span>
                        <asp:TextBox ID="txtMicroSignal" CssClass="forminput" runat="server" /></span>
                    </li>
                    <li><span>分销等级：</span> <span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="DrGrade" runat="server">
                                <asp:ListItem Value="0">全部</asp:ListItem>
                                <asp:ListItem Value="1">一级</asp:ListItem>
                                <asp:ListItem Value="2">二级</asp:ListItem>
                                <asp:ListItem Value="3">三级</asp:ListItem>
                            </asp:DropDownList>
                        </abbr>
                    </span></li>
                    <li style="display: none"><span>状态：</span> <span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="DrStatus" runat="server">
                                <asp:ListItem Value="0">全部</asp:ListItem>
                                <asp:ListItem Value="1">未审核</asp:ListItem>
                                <asp:ListItem Value="2">通过审核</asp:ListItem>
                                <asp:ListItem Value="3">拒绝审核</asp:ListItem>
                            </asp:DropDownList></abbr></span></li>
                    <li>
                        <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="搜索" />
                    </li>

                </ul>
            </div>
            <div class="functionHandleArea m_none">
            </div>
            <table class="table_title">
                <thead>
                    <tr>
                        <td style="width: 60px;">用户ID
                        </td>
                        
                        <%--                       <td>
                            分销等级
                        </td>--%>
                        <td>联系人</td>
                        <td>微信昵称</td>
                        <td>手机号码</td>
                        <td>QQ</td>
                        <td>微信号</td>
                        <%--<td>推荐人id</td>--%>
                        <td>推荐人名称</td>
                        <td>状态</td>

                        <td>操作</td>
                    </tr>
                </thead>
                <asp:Repeater ID="reDistributor" runat="server">
                    <ItemTemplate>
                        <tbody>
                            <tr>
                                <td><%# Eval("UserId")%></td>
                                </td>
                                <%--<td>
                                     &nbsp; <%# Eval("GradeId").ToString() == "1" ? "一级" : Eval("GradeId").ToString() == "2"?"二级":"三级"%>
                                </td>--%>
                                <td>&nbsp; <%# Eval("RealName")%></td>
                                <td>&nbsp; <%# Eval("UserName")%></td>
                                <td>&nbsp;<%# Eval("CellPhone")%>
                                </td>
                                <td>&nbsp;<%# Eval("QQ")%>
                                </td>
                                <td>&nbsp;<%# Eval("MicroSignal")%></td>
                                <%--<td>&nbsp;<%# Eval("ReferralUserId")%></td>--%>
                                <td>&nbsp;<%# Eval("ReferralUserName")%></td>
                                <td>
                                    <span class="submit_status" status="<%# Eval("Status")%>" userid ="<%# Eval("UserId")%>">
                                        <a href="javascript:void(0);" class="close"></a>
                                    </span>
                                </td>
                                <td>&nbsp;
                                 <%--<span class="submit_bianji"><asp:HyperLink ID="lkbView" runat="server" Text="编辑" NavigateUrl='<%# "EditDistributor.aspx?UserId="+Eval("UserId")%>' ></asp:HyperLink></span>--%>
                                    <span class="submit_bianji">
                                        <asp:HyperLink ID="lkbView1" runat="server" Text="详细" NavigateUrl='<%# "DistributorDetails.aspx?UserId="+Eval("UserId")%>'></asp:HyperLink></span>
                                    <span class="submit_bianji">
                                        <asp:HyperLink ID="HyperLink1" runat="server" Text="佣金明细" NavigateUrl='<%# "CommissionsList.aspx?UserId="+Eval("UserId")%>'></asp:HyperLink></span>



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
