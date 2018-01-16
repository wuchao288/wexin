<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<tr>
    <td>
        <%# Eval("OrderId") %>
    </td>
    <td>
        [<Hi:DistributorGradeLitral runat="server" GradeId='<%# Eval("GradeId")%>'></Hi:DistributorGradeLitral>]<%# Eval("StoreName")%>
    </td>
    <td>
        <%# Eval("CommTotal","{0:F2}")%>元
    </td>
</tr>
