$(function () {
    $(".submit_status").each(function () {
        var status = $(this).attr("status");
        setRowStatus($(this), status);
    });

    $(".submit_jia").click(function () {
        arrytext = null;
        formtype = "add";
        setArryText('userid', "");
        setArryText('username', "");
        setArryText('mobile', "");
        setArryText('weixin', "");
        setArryText('parent_id', "");
        setArryText('email', "");
        setArryText('qq', "");
        setArryText('address', "");
        DialogShow('添加分销商', 'divaddroles', 'divadd', 'btnAdd');
    });

    $(".submit_status").click(function () {
        var status = $(this).attr("status");
        var userid = $(this).attr("userid");
    
        if (status == "0") {
            setRowStatus($(this), "1");
            status = "1";

        } else {
            setRowStatus($(this), "0");
            status = "0";
        }

        $.post("/admin/distributor/DistributorStatus.ashx", { userid: userid, status: status }, function () {

        });
    });
});

function setRowStatus(row, status) {
    row.attr("status", status);

    if (status == "1") {
        row.find("a").attr("class", "close");
    }
    else {
        row.status = status;
        row.find("a").attr("class", "open");
    }
}

function validatorForm() {
    if ($("#userid").val() == "") {
        alert("请输入会员ID");
        $("#userid").focus();
        return false;
    }

    if ($("#username").val() == "") {
        alert("请输入会员姓名");
        $("#username").focus();
        return false;
    }

    if ($("#mobile").val() == "") {
        alert("请输入会员手机");
        $("#mobile").focus();
        return false;
    }

    if ($("#weixin").val() == "") {
        alert("请输入会员微信");
        $("#weixin").focus();
        return false;
    }

    if ($("#email").val() == "") {
        alert("请输入会员邮箱");
        $("#email").focus();
        return false;
    }
    
    return true;
}

function add() {
    $.post("/Admin/distributor/DistributorAdd.ashx", {
        userid: $("#userid").val(),
        username: $("#username").val(),
        mobile: $("#mobile").val(),
        weixin: $("#weixin").val(),
        parent_id: $("#parent_id").val(),
        email: $("#email").val(),
        qq: $("#qq").val(),
        address: $("#address").val(),
    }, function (result) {
        if (result != "ok") {
            alert(result);
        }
        else {
            location.reload();
        }
    });
}