<%@ Page Title="" Language="C#" MasterPageFile="~/Vshop/BasePage.Master"
    AutoEventWireup="true" CodeBehind="CategoryProduct.aspx.cs"
    Inherits="Hidistro.Web.Vshop.CategoryProduct" EnableViewState="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../web/category_product/style.css?v=3" rel="stylesheet" />
    <script>
        $(function () {
            var height = document.documentElement.clientHeight - $("header").outerHeight();
            $(".list-wrap").height(height);

            $("#txtKeywords").keypress(function (e) {
                if (e.keyCode == 13) {
                    var key = $("#txtKeywords").val();
                    if (key == "") {
                        return false;
                    }

                    var url = "/Vshop/ProductList.aspx?keyWord=" + encodeURI(key);

                    window.location.href = url;
                    return false;

                }
            });

            $(".list-items li").click(function () {
                $(".list-items li").removeClass("cur");

                $(this).addClass("cur");
                $(".list-detail ul").text("");
                var categoryId = $(this).attr("categoryId");

                $.get("/Handlers/ProductHandler.ashx", { categoryId: categoryId }, function (result) {
                    var objdata = eval("(" + result + ")");

                    var str = "";

                    for (var i = 0; i < objdata.length; i++) {
                        str += '<li>';
                        str += '        <a href="/Vshop/ProductDetails.aspx?ProductId=' + objdata[i].ProductId + '">';
                        str += '            <img src="' + objdata[i].ThumbnailUrl220 + '" />';
                        str += '            <span>' + objdata[i].ProductName + '</span>';
                        str += '        </a>';
                        str += '    </li>';
                    }

                    $(".list-detail ul").append(str);
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <header>
        <div style="width: 0.48rem; margin-left: 15.456px; margin-right: 15.456px;">
            <a href="javascript:history.back();" class="back"></a>
        </div>

        <div style="-webkit-box-flex: 1; overflow: hidden">
            <input type="text" id="txtKeywords" placeholder="请输入关键字" />
        </div>
    </header>
    <div id="body" class="list-wrap wbox">
        <div class="list-items overtouch">
            <ul>
                <li class="cur" categoryid="">热门推荐
                </li>
                <asp:Repeater ID="categoryRepeater" runat="server">
                    <ItemTemplate>
                        <li categoryid="<%#Eval("CategoryId") %>">
                            <%#Eval("Name") %>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>

        <div class="list-details wbox-flex overtouch">
            <div class="list-detail">
                <ul>
                    <asp:Repeater ID="productRepeater" runat="server">
                        <ItemTemplate>
                            <li>
                                <a href="/Vshop/ProductDetails.aspx?ProductId=<%# Eval("ProductId") %>">
                                    <img src="<%#Eval("ThumbnailUrl220") %>" />

                                    <span><%#Eval("ProductName") %></span>

                                </a>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>

            </div>
        </div>
    </div>
</asp:Content>
