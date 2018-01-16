$(function () {
    var url = "/Admin/DefaultHandler.ashx";
    $.post(url, {}, function (result) {
        $("#hishop_menu").append(result);
    });
});