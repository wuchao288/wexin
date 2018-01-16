<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%
    string plink = Globals.GetCurrentDistributorId() > 0 ? "&&ReferralId=" + Globals.GetCurrentDistributorId() : "";
%>
<li>
    <a href='<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?ProductId=" + Eval("ProductId")%><%=plink %>'>
        <div class="row_box">
            <div class="rb" style="width: 130px;">
                <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl310" CssClass="p_img" />
            </div>
            <div class="rb" style="-webkit-box-flex: 1;">
                <div class="title">
                    <%# Eval("ProductName") %>
                </div>
                <div class="price text-danger">
                    ¥<%# Eval("SalePrice", "{0:F2}") %>
                </div>

                <div class="stock">
                    <span class="text-muted">已售<%# Eval("ShowSaleCounts")%>件</span>
                </div>
            </div>
        </div>
    </a>
</li>
