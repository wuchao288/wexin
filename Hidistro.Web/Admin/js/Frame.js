// JavaScript Document

//选择点击一级菜单显示
//selecttype
function ShowMenuLeft(firstnode, secondnode, threenode) {
    $.ajax({
        url: "MenuHandler.ashx",
        data: { module: secondnode },
        dataType: "json",
        type: "POST",
        async: false,
        timeout: 10000,
        error: function (xm, msg) {
            alert("loading xml error");
        },
        success: function (result) {
            $("#menu_left").html('');

            var curenturl = null;

            switch (secondnode) {
                case "v1":
                    //
                    //curenturl = "vshop/VServerConfig.aspx";
                    curenturl = "help/data.html";
                    
                    break;

                case "v2":
                    alert(11);
                    curenturl = "Admin/help/data.html";
                    //curenturl = "member/managemembers.aspx";
                    break;

                case "v3":
                    curenturl = "promotion/newcoupons.aspx";
                    break;

                case "v4":
                    curenturl = "product/productonsales.aspx";
                    break;

                case "v5":
                    curenturl = "distributor/distributorapplyset.aspx";
                    break;

                case "v6":
                    curenturl = "sales/manageorder.aspx";
                    break;

                case "v7":
                    curenturl = "sales/salereport.aspx";
                    break;

                case "v8":
                    curenturl = "store/managelogs.aspx";
                    break;
            }

            for (var obj in result) {
                var item = result[obj];
                menutitle = $('<div class="hishop_menutitle"></div>');
                menuaspan = $("<span onclick='ShowSecond(this)'>" + obj + "</span>"); //获取二级分类名称
                menutitle.append(menuaspan);

                for (var i = 0; i < item.length; i++) {
                    var link_href = item[i].link;
                    var link_title = item[i].title;
                    $alink = $("<a href='" + link_href + "' target='frammain'>" + link_title + "</a>");

                    if (link_href == curenturl) {
                        $alink.addClass("curent");
                    }
                    menutitle.append($alink);
                    menutitle.append('<div class="clean"></div>');
                }
                $("#menu_left").append(menutitle);
            }

            $("#menu_arrow").attr("class", "open_arrow");
            $("#menu_arrow").css("display", "block");
            $(".hishop_menu_scroll").css("display", "block");
            $(".hishop_content_r").css("left", 173);
            if (threenode != null) {
                curenturl = threenode;
            }
            $("#frammain").attr("src", curenturl);
            $("#frammain").width($(window).width() - 168);
        }
    });
    $(".hishop_menu a:contains('" + firstnode + "')").addClass("hishop_curent").siblings().removeClass("hishop_curent");
}



//点击左右关闭树
function ExpendMenuLeft() {
    var clientwidth = $(window).width() - 7;
    if ($(".hishop_menu_scroll").is(":hidden")) {//点击展开
        $("#menu_arrow").attr("class", "open_arrow");
        $(".hishop_menu_scroll").css("display", "block");
        $(".hishop_content_r").css("left", 173);
        $("#frammain").width(clientwidth - 168);
    } else {//点击隐藏
        $("#menu_arrow").attr("class", "close_arrow");
        $(".hishop_menu_scroll").css("display", "none");
        $(".hishop_content_r").css("left", 7);
        $("#frammain").width(clientwidth)
    }

}

//点击二级菜单
function ShowSecond(sencond) {
    if ($(sencond).siblings("a:hidden") != null && $(sencond).siblings("a:hidden").length > 0) {
        $(sencond).siblings("a").css("display", "block");
    } else {
        $(sencond).siblings("a").css("display", "none");
    }
}

//自适应高度
function AutoHeight() {
    var clientheight = $(this).height() - 87;
    var clientwidth = $(this).width() - 15;
    $(".hishop_menu_scroll").height(clientheight);
    $(".hishop_content_r").height(clientheight);
    if (!$(".hishop_menu_scroll").is(":hidden")) {
        clientwidth = clientwidth - 168;
    }
    $("#frammain").width(clientwidth);
}


//窗口变化
$(window).resize(function () {
    AutoHeight();
});

//窗口加载
$(function () {
    AutoHeight();
    $("#menu_left a").live("click", function () {
        $("#menu_left a").removeClass("curent");
        $(this).addClass("curent");
    });
    LoadTopLink();

    if ($.cookie("guide") == null || $.cookie("guide") == "undefined" || $.cookie("guide") != 1) {
        DialogFrame('help/vshopindex.html', '新手向导', 750, 450);
    }
});


function LoadTopLink() {
    $.ajax({
        url: 'LoginUser.ashx?action=login&timestamp=' + new Date().getTime(),
        dataType: 'json',
        type: 'GET',
        timeout: 5000,
        //		error: function(xm, msg) {
        //			alert(msg);
        //		},
        success: function (siteinfo) {
            document.title = siteinfo.sitename + " v1.1";
            $(".hishop_banneritem a:eq(0)").text(siteinfo.sitename);
            $(".hishop_banneritem a:eq(1)").text(siteinfo.username);

        }
    });
}
