<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<img src="<%# Eval("ImageUrl") %>" style="width: 100%;" onclick="location.href='<%# Eval("Url") %>'"/>
