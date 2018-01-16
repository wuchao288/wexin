<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%
    string plink = Globals.GetCurrentDistributorId() > 0 ? "&&ReferralId=" + Globals.GetCurrentDistributorId() : "";
%>

