<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="QRCodeIndex.aspx.cs" Inherits="Hidistro.Web.Vshop.QRCodeIndex" MasterPageFile="~/Vshop/Site1.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .body {
            background-color: white;
        }
    </style>

    <div style="padding: 10px; margin-top:20px;">
        对不起，您还不是分销商，所以未能生成您的专属二维码，马上申请成为分销商。
    </div>

    <div style="text-align: center; height: 100px; margin-top: 20px; display: -webkit-box; padding: 10px;">
        <a href="javascript:history.back();" class="btn btn-danger" id="fenxiao" style="width: 100%; display: block; height: 50px; line-height: 50px; padding: 0;">
            点击返回
        </a>
    </div>
</asp:Content>
