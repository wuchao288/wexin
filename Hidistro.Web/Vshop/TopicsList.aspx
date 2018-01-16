<%@ Page Title="" Language="C#" MasterPageFile="~/Vshop/BasePage.Master"
    AutoEventWireup="true" CodeBehind="TopicsList.aspx.cs" Inherits="Hidistro.Web.Vshop.TopicsList"
    EnableViewState="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div id="ui-header">

        <div class="fixed">
            <a class="popmenu">本站专题</a>
            <a class="ui-btn-left_pre" href="javascript:history.back()"></a>

        </div>

    </div>

    <div style="height: 46px;"></div>

    <div class="panel_container">
        <div class="panel_content">
            <!--默认样式图片列表-->
            <div class="piclist_container">

                <asp:Repeater ID="Repeater1" runat="server">
                    <ItemTemplate>
                        <div class="piclist">
                            <a href="/Vshop/Topics.aspx?TopicId=<%#Eval("TopicId") %>">
                                <div class="list_bd">
                                    <div class="cover" style="background: url(<%# Eval("IconUrl") %>) no-repeat center #EDEDED; background-size: cover;">
                                    </div>
                                    <div class="discribe">
                                        <p>
                                            <%#Eval("title")%>                              
                                        </p>
                                    </div>
                                </div>
                            </a>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

            </div>

            <div style="clear: both; margin-top: 12px;"></div>
            <div id="copyright">
                <p>
                    <a href="http://mp.weixin.qq.com/s?__biz=MjM5MzIwMjQwNA==&mid=210160185&idx=1&sn=0c26c26cd8bd91ac6f86823ae3aaf2eb&scene=18#rd">特物网络 | 技术支持</a>
                </p>
            </div>
        </div>
    </div>
</asp:Content>
