$(function () {
    $(".isopen").each(function () {
        var isopen = $(this).attr("isopen");
        if (isopen == "1") {
            $(this).text("已开通");
        }
    });
})