<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="panel panel-default">
              <div class="panel-heading">
                <h3 class="panel-title orders-subtitle"><span>订单号:<%# Eval("OrderId")%></span><em>  <Hi:OrderStatusLabel ID="OrderStatusLabel1" OrderStatusCode='<%# Eval("OrderStatus") %>'
                    runat="server" /></em></h3>
              </div>
              <div class="panel-body ordersinfo">
                <asp:Repeater ID="rporderitems" runat="server" DataSource='<%# Eval("OrderItems") %>'>
                <ItemTemplate>
                <div class="fir well clearfix">
                   <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailsUrl" />
                    <div>
                       <span><%# Eval("ItemDescription")%></span>
                       <p>价格:<%# Eval("ItemListPrice","{0:F2}")%>元<em>数量:<%# Eval("Quantity")%></em></p>
                    </div>
                 </div>
                </ItemTemplate>
            </asp:Repeater>
                <p class="clearfix">订单成交金额:<%# Eval("OrderTotal","{0:F2}")%>元</p>
                <p class="clearfix">订单成交时间:<%# Convert.ToDateTime(Eval("OrderDate")).ToString("yyyy-MM-dd")%>
                <strong>佣金<em><%# Eval("CommTotal","{0:F2}")%></em><i>(元)</i></strong></p>
              </div>
        </div>