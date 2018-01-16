<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="well goods-box">
<Hi:ProductDetailsLink  runat="server" ProductId='<%# Eval("ProductId") %>' ProductName='<%# Eval("ProductName") %>'>
    
        <Hi:ListImage runat="server" DataField="ThumbnailUrl60" />
        <div class="info">
            <div class="name font-xl bcolor">
                <%# Eval("ProductName")%></div>
            <div class="intro font-m text-muted">
                <%# Eval("ShortDescription")%></div>
            <div class="price text-danger">
                ¥<%# Eval("SalePrice", "{0:F2}")%>
            </div>
        </div>
 </Hi:ProductDetailsLink><a href="javascript:void(0)" onclick="Submit('<%# Eval("FavoriteId")%>')"><span
        class="glyphicon glyphicon-remove-circle move"></span></a>
</div>
