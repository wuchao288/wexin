<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="well goods-box goods-box-shopcart">
    <a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?productId=" + Eval("ProductId")%>">
        <Hi:ListImage runat="server" DataField="ThumbnailUrl60" />
        <div class="info">
            <div class="name font-xl bcolor">
                <%# Eval("Name")%></div>
            <div class="specification">
                <input id="skucontent" type="hidden" value="<%# Eval("SkuContent")%>" />
            </div>
            <div class="price text-danger">
                ¥<%# Eval("AdjustedPrice", "{0:F2}")%>
            </div>
        </div>
    </a><a href="javascript:void(0)" name="iDelete" skuid='<%# Eval("SkuId")%>'>
        <span class="glyphicon glyphicon-remove-circle move"></span></a>
    <div class="goods-num">
        <div name="spSub" class="shopcart-add">
            <span class="glyphicon glyphicon-minus-sign"></span></div>
        <input type="tel" class="ui_textinput" name="buyNum" value='<%# Eval("Quantity")%>'
            skuid='<%# Eval("SkuId")%>' />
              <div name="spAdd" class="shopcart-minus">
            <span class="glyphicon glyphicon-plus-sign"></span></div>
      
    </div>
</div>
