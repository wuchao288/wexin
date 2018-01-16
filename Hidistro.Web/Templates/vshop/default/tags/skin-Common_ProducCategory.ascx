<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li>
    <a href="/Vshop/ProductList.aspx?categoryId=<%# Eval("CategoryId") %>"><%# Eval("Name") %></a>
</li>
