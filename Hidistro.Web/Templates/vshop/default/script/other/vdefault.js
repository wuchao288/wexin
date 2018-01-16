$(function () {
    $("#star").click(function () {
        $("#mask").show();
        $("#shoucang").show();
    });

    $("#mask").click(function () {
        $(this).hide();
        $("#shoucang").hide();
    });

    $("#shoucang").click(function () {
        $("#mask").hide();
        $("#shoucang").hide();
    });

    $("#backtop").click(function () {
        $("body,html").animate({ "scrollTop": "0" }, 400);
    })


    backtop2();
    backTop();
})

function backTop() {
    $(window).bind("scroll resize", function () {
        backtop2();
    })
}

function backtop2() {
    var C = $(window).width(), B = $(window).height();

    if ($(window).scrollTop() > (B / 2)) {
        $("#backtop").fadeIn();
    } else {
        $("#backtop").hide();
    }
}