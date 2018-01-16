//隐藏右上角菜单
document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
    WeixinJSBridge.call('showOptionMenu');
});
var selectProdcut = ($.cookie("SelectProcutId") == null || $.cookie("SelectProcutId")=="") ? {} : JSON.parse($.cookie("SelectProcutId"));

$(document).ready(function () {
    $("footer .glyphicon-refresh").click(function () {
        location.reload();
    })

    $("footer .glyphicon-arrow-left").click(function () {
        history.go(-1);
    })

    /*friends-circle*/
    var Width = $('.firends-circle .content div').width();
    $('.firends-circle .content div').height(0.5 * Width);
    
    /*default.html*/
   var ImgWidth = $('.index-content img').width();
 $('.index-content img').height(ImgWidth);
 

    /*fill-information*/
    var Width1 = $('.distributor .content div').width();
   $('.distributor .content div').height(0.5 * Width1);
   $('.distributor .content div.disactive').height((0.5 * Width1)-4);
    $('.distributor .content div').click(function () {
        if ($(this).hasClass("disactive")) {
            $(this).removeClass("disactive");

        } else {
            $(this).addClass("disactive");
            $(this).find('span').show();
            $(this).siblings().removeClass('disactive');
        }
    });

    /*my-goods*/
    $("input[type='checkbox']").iCheck({
        checkboxClass: 'icheckbox_flat-red',
        radioClass: 'iradio_flat-red'
    });

    $('.search_img').click(function () {
        $(this).toggleClass('color');
    })

    $("input[type='checkbox'][name='CheckAll']:eq(0)").on('ifChecked', function (a) {
        $("input[name='CheckGroup']").iCheck('check');
    });
    $("input[type='checkbox'][name='CheckAll']:eq(0)").on('ifUnchecked', function (a) {
        $("input[name='CheckGroup']").iCheck('uncheck');
        $("input[type='checkbox'][name='CheckAll']:eq(0)").attr("checked", false);
        $("input[type='checkbox'][name='CheckAll']:eq(0)").parent("div").removeClass("checked");
    });
    $("input[name='CheckGroup']").on('ifChecked', function (event) {
        selectProdcut["CheckGroup" + $(this).val()] = $(this).val();
        UpdateCookieProductId();



        var real_h;
        var right_H = $(this).parent().parent().prev().height();
        
        $(this).parent().parent().height(right_H);
        var right_W = $(this).parent().parent().prev().width();
        $(this).parent().parent().width(right_W);
        real_h = (right_H - $(this).parent().height()) * 0.5;
        $(this).parent().css({ position: 'absolute', top: real_h, left: right_W/2 - 10 });



        $(this).parent("div").parent("div").css("display", "block");
    });
    $("input[name='CheckGroup']").on('ifUnchecked', function (event) {
        $(this).parent("div").parent("div").css({ display: 'none' });
        delete selectProdcut["CheckGroup" + $(this).val()];
        UpdateCookieProductId();
    });

    $(".index-content").click(function () {
        $(this).next().find("input[type='checkbox']").iCheck('check');
    });
    $(".right").click(function () {
        $(this).css("display", "none");
        $(this).find("input[type='checkbox']").iCheck('uncheck');
    });

});
setTimeout("CheckShow()",150);
function CheckShow() {
    if (selectProdcut != null) {
        $.each(selectProdcut, function (index, items) {
            $("input[type='checkbox'][name='CheckGroup'][value='" + items + "']").iCheck('check');
        });
    }
 
}

function UpdateCookieProductId() {
    var valstr = JSON.stringify(selectProdcut);
    $.cookie("SelectProcutId", valstr);
}

function goUrl(url) {
    window.location.href = url;
}

//检查申请提现
function RequestCommissions() {
    if ($("#VRequestCommissions_txtaccount").val().replace(/\s/g, "")=="") {
        alert_h('请输入支付宝账号');
        return false;
    }
    if ($("#VRequestCommissions_txtmoney").val().replace(/\s/g, "") == "") {
        alert_h('请输入提现金额');
        return false;
    }
    if (isNaN($("#VRequestCommissions_txtmoney").val().replace(/\s/g, ""))
    || parseFloat($("#VRequestCommissions_txtmoney").val().replace(/\s/g, "")) <= 0
    || parseFloat($("#VRequestCommissions_txtmoney").val().replace(/\s/g, "")) <100) {
        alert_h('请输入大于等于100的纯数字类型');
        return false;
    }
    var commissions = $(".text-danger").text().replace(/[^0-9]/ig, "");
    if (parseFloat($("#VRequestCommissions_txtmoney").val()) > parseFloat(commissions)) {
        alert_h('提现金额不允许超过现有佣金金额');
        return false;
    }
    var placehoder=$("#VRequestCommissions_txtmoney").attr("placeholder").replace(/[^0-9]/ig, "");
    if (placehoder!=""&&placehoder!="undefined") {
        if ($("#VRequestCommissions_txtmoney").val().replace(/\s/g, "") % placehoder != 0) {
            alert_h('请输入' + placehoder + '倍数的金额,且最小金额为100元！');
            return false;
        }
    }
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post',
        dataType: 'json',
        timeout: 10000,
        data: {
            action: "AddCommissions",
            account: $("#VRequestCommissions_txtaccount").val(),
            commissionmoney: $("#VRequestCommissions_txtmoney").val()
        },
        success: function (resultData) {
            if (resultData.success) {
                alert_h(resultData.msg, function () {
                    location.href = "DistributorCenter.aspx";
                });
            }
            else {
                alert_h(resultData.msg);
            }
        }
    });

}


function myConfirm(title, content, ensureText, ensuredCallback) {
    var myConfirmCode = '<div class="modal fade" id="myConfirm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">\
                  <div class="modal-dialog">\
                    <div class="modal-content">\
                      <div class="modal-header">\
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>\
                        <h4 class="modal-title" id="myModalLabel">' + title + '</h4>\
                      </div>\
                      <div class="modal-body">\
                        ' + content + '\
                      </div>\
                      <div class="modal-footer">\
                        <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>\
                        <button type="button" class="btn btn-danger">' + ensureText + '</button>\
                      </div>\
                    </div>\
                  </div>\
                </div>';
    if ($("#myConfirm").length == 0) {
        $("body").append(myConfirmCode);
    }
    $('#myConfirm').modal();
    $('#myConfirm button.btn-danger').unbind("click", "");
    $('#myConfirm button.btn-danger').click(function (event) {
        if (ensuredCallback)
            ensuredCallback();
        $('#myConfirm').modal('hide')
    });
}



function alert_h(content, ensuredCallback) {
    var myConfirmCode = '<div class="modal fade" id="alert_h" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">\
                  <div class="modal-dialog">\
                    <div class="modal-content">\
                      <div class="modal-header">\
                        <h5 class="modal-title" id="myModalLabel">操作提示</h5>\
                      </div>\
                      <div class="modal-body" style="font-size:14px;">\
                        ' + content + '\
                      </div>\
                      <div class="modal-footer">\
                        <button type="button" class="btn btn-danger" data-dismiss="modal">确认</button>\
                      </div>\
                    </div>\
                  </div>\
                </div>';

    if ($("#alert_h").length != 0) {
        $('#alert_h').remove();
    }
    $("body").append(myConfirmCode);
    $('#alert_h').modal();

    $('#alert_h').off('hide.bs.modal').on('hide.bs.modal', function (e) {
        if (ensuredCallback)
            ensuredCallback();
    });
}


var pageLoadTime;
var passedSeconds = 0;

function GetRTime() {
    var d;
    var h;
    var m;
    var s;

    var startVal = document.getElementById("startTime").value;
    var endVal = document.getElementById("endTime").value;
    var startTime = new Date(startVal);
    var endTime = new Date(endVal); //截止时间 前端路上 http://www.51xuediannao.com/qd63/
    var nowTime = new Date($('#nowTime').val());
    nowTime.setSeconds(nowTime.getSeconds() + passedSeconds);
    passedSeconds++;
    var now_startTime = nowTime.getTime() - startTime.getTime();    //当前时间 减去开始时间
    var s_nTime = startTime.getTime() - nowTime.getTime();          //开始时间减去当前时间
    var start_endTime = endTime.getTime() - startTime.getTime();    //结束时间减去开始时间
    var now_endTime = endTime.getTime() - nowTime.getTime();     //结束时间减去当前时间
    var now_pTime = nowTime.getTime() - pageLoadTime;               //当前时间减去页面刷新时间
    var p_sTime = startTime.getTime() - pageLoadTime;               //开始时间减去页面刷新时间
    var wid = now_startTime / start_endTime * 100;                    //开始后离结束的时间比
    var wid1 = now_pTime / p_sTime * 100;                             //未开始离开始的时间比
    var tuan_button = document.getElementById("buyButton");
    var progress = document.getElementById("progress");
    var tuan_time = document.getElementById("tuan_time");
    function docu() {
        document.getElementById("t_d").innerHTML = d + "天";
        document.getElementById("t_h").innerHTML = h + "时";
        document.getElementById("t_m").innerHTML = m + "分";
        document.getElementById("t_s").innerHTML = s + "秒";
    }
    if (pageLoadTime == null) {
        pageLoadTime = nowTime;
    }
    if (100 >= wid1 >= 0 && wid < 0) {
        d = Math.floor(Math.abs(now_startTime) / 1000 / 60 / 60 / 24);
        h = Math.floor(Math.abs(now_startTime) / 1000 / 60 / 60 % 24);
        m = Math.floor(Math.abs(now_startTime) / 1000 / 60 % 60);
        s = Math.floor(Math.abs(now_startTime) / 1000 % 60);
        docu();
        tuan_time.innerHTML = "团购开始时间：";
        progress.style.width = wid1 + "%";
        tuan_button.disabled = true;
    }
    if (wid1 > 100 || wid1 < 0) {
        if (wid >= 0 && wid < 70) {
            d = Math.floor(now_endTime / 1000 / 60 / 60 / 24);
            h = Math.floor(now_endTime / 1000 / 60 / 60 % 24);
            m = Math.floor(now_endTime / 1000 / 60 % 60);
            s = Math.floor(now_endTime / 1000 % 60);
            docu();
            tuan_time.innerHTML = "团购结束时间：";
            progress.style.width = (100 - wid) + "%";
            tuan_button.disabled = false;
        } else if (wid >= 70 && wid < 90) {
            d = Math.floor(now_endTime / 1000 / 60 / 60 / 24);
            h = Math.floor(now_endTime / 1000 / 60 / 60 % 24);
            m = Math.floor(now_endTime / 1000 / 60 % 60);
            s = Math.floor(now_endTime / 1000 % 60);
            docu();
            tuan_time.innerHTML = "团购结束时间：";
            progress.className = "progress-bar progress-bar-warning";
            progress.style.width = (100 - wid) + "%";
            tuan_button.disabled = false;
        } else if (wid >= 90 && wid <= 100) {
            d = Math.floor(now_endTime / 1000 / 60 / 60 / 24);
            h = Math.floor(now_endTime / 1000 / 60 / 60 % 24);
            m = Math.floor(now_endTime / 1000 / 60 % 60);
            s = Math.floor(now_endTime / 1000 % 60);
            docu();
            tuan_time.innerHTML = "团购结束时间：";
            progress.style.width = (100 - wid) + "%";
            progress.className = "progress-bar progress-bar-danger";
            tuan_button.disabled = false;
        }

        if (wid > 100) {
            tuan_time.innerHTML = "团购已结束!";
            progress.style.width = 0;
            tuan_button.disabled = true;
        }
    }

}

function getParam(paramName) {
    paramValue = "";
    isFound = false;
    if (this.location.search.indexOf("?") == 0 && this.location.search.indexOf("=") > 1) {
        arrSource = unescape(this.location.search).substring(1, this.location.search.length).split("&");
        i = 0;
        while (i < arrSource.length && !isFound) {
            if (arrSource[i].indexOf("=") > 0) {
                if (arrSource[i].split("=")[0].toLowerCase() == paramName.toLowerCase()) {
                    paramValue = arrSource[i].split("=")[1];
                    isFound = true;
                }
            }
            i++;
        }
    }
    return paramValue;
}

