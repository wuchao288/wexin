<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="distributors well">
    <div class="content-right">
        <div>
            <%# Eval("StoreName")%> - <%# Eval("UserName")%></div>
        <div>
            <%# Eval("CreateTime","{0:d}")%>注册</div>
        <div class="price">
            销售额<%# Eval("OrderTotal", "{0:F2}")%><strong class="text-muted">(元)</strong><span>贡献佣金
                <em>
                    <%# Eval("CommTotal", "{0:F2}")%></em><strong class="text-muted">(元)</strong></span></div>
        <a type="button" class="btn btn-danger" href='Default.aspx?ReferralId=<%#Eval("UserId") %>'>去逛逛</a>
    </div>
</div>
