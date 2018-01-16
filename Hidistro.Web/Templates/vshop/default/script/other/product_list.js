$(function () {
    $("#" + sort_filed).addClass("red");

    //显示排序方式
    if (sort_direction == "desc") {
        $("#" + sort_filed).append("<span class='glyphicon glyphicon-arrow-down'></span>");
    }
    else {
        $("#" + sort_filed).append("<span class='glyphicon glyphicon-arrow-up'></span>");
    }

    //类别
    category_id = getParam('categoryId');

    //搜索的词
    $("#txtKeywords").val(keyWord);

    //显示类别
    $("#btnCategory").click(function () {
        $("#category").css("left", -60);
        $("#category").show();

        $("#category").stop().animate({ left: 0, opacity: 1 }, 200);
    });

    //隐藏类别
    $("#category .title a").click(function () {
        $("#category").stop().animate({ left: -10, opacity: 0 }, 100, function () {
            $("#category").hide();
        });

    });

    $("#backtop").click(function () {
        $("body,html").animate({ "scrollTop": "0" }, 400);
    })

    $("#category ul li a").click(function () {
        $("#category").hide();
    });

    $("#txtKeywords").change(function (e) {
        searchs();
    });

    $(".rank-item").click(function () {
        sort_filed = $(this).attr("id");
        if ($(this).find("span").length == 0) {
            //默认方式
            sort_direction = $(this).attr("sort");
            searchs();
            return;
        }

        if (sort_direction == "desc") {
            sort_direction = "asc";
        }
        else {
            sort_direction = "desc";
        }

        searchs();
    });

    var showtype = "grid";
    $("#show_type").click(function () {
        if (showtype == "list") {
            showtype = "grid";
            showProductGrid();
        }
        else {
            showtype = "list";
            showProductList();
        }
    });

    showProductGrid();
    backtop2();
    backTop();
})

function searchs() {

    var key = $("#txtKeywords").val();
    var url = "/Vshop/ProductList.aspx?categoryId=" + category_id + "&keyWord=" + encodeURI(key);
    url += "&sort=" + sort_filed;
    url += "&order=" + sort_direction;

    window.location.href = url;
}


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

function showProductGrid() {
    $("#productListContainer").html("");

    var str = "<div class=\"goods-list-grid clearfix\" id=\"goodslist\">";

    var obj = eval("(" + list + ")");

    for (var i = 0; i < obj.length; i++) {
        str += "<a href='/Vshop/ProductDetails.aspx?ProductId=" + obj[i].ProductId + "'>";
        str += "    <div>";
        str += "        <div>";
        str += "        <img src='" + obj[i].ThumbnailUrl220 + "' style='height:175px;border-width:0px;' />";
        str += "            <div class='info'>";
        str += "                <div class='name bcolor'>" + obj[i].ProductName + "</div>";
        str += "                <div class='price font-s text-danger'>";
        str += "                    ¥" + obj[i].SalePrice.toFixed(2) + " <del class='text-muted font-xs'>¥" + obj[i].SalePrice.toFixed(2) + " </del>";
        str += "                </div>";
        str += "                <div class='sales text-muted font-xs'>已售" + obj[i].SaleCounts + "件</div>";
        str += "            </div>";
        str += "        </div>";
        str += "    </div>";
        str += "</a>";
    }

    str += "</div>";

    $("#productListContainer").html(str);

}

function showProductList() {
    $("#productListContainer").html("");

    var str = "<div class=\"product_list clearfix\">";

    var obj = eval("(" + list + ")");

    for (var i = 0; i < obj.length; i++) {
        str += "<li>";
        str += "<a href='/Vshop/ProductDetails.aspx?ProductId=" + obj[i].ProductId + "'>";
        str += "    <div class='row_box'>";
        str += "        <div class='rb' style='width: 130px;'>";
        str += "            <img  class='p_img' src='" + obj[i].ThumbnailUrl220 + "' style='border-width:0px;' />";
        str += "        </div>";
        str += "        <div class='rb' style='-webkit-box-flex: 1;'>";
        str += "            <div class='title'>";
        str += "                " + obj[i].ProductName;
        str += "            </div>";
        str += "            <div class='price text-danger'>";
        str += "                ¥" + obj[i].SalePrice.toFixed(2);
        str += "            </div>";
        str += "";
        str += "            <div class='stock'>";
        str += "                <span class='text-muted'>已售" + obj[i].SaleCounts + "件</span>";
        str += "            </div>";
        str += "        </div>";
        str += "    </div>";
        str += "</a>";
        str += "</li>";
    }

    str += "</div>";

    $("#productListContainer").html(str);
}