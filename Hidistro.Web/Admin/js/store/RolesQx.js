$(function () {
    $(".tb input").click(function () {
        var myid = $(this).attr("id");
        var is_check = $(this).is(":checked");
        
        var strs_id = myid.split('_');
        var level = strs_id.length;
        var search_id = "";

        if (level == 1) {
            //第一级
            search_id = strs_id[0] + "_";
        }
        else if (level == 2) {
            //第二级
            search_id = strs_id[0] + "_" + strs_id[1] + "_";
        }
        else {
            //第三级
            checkIndeterminate(strs_id);
            return;
        }

        if (is_check) {
            $(".tb input[id^='" + search_id + "']").attr("checked", true);

        }
        else {
            $(".tb input[id^='" + search_id + "']").attr("checked", false);
        }

        checkIndeterminate(strs_id);
    });

    //加载权限
    if (qx != "") {
        var qxs = qx.split(',');
        for (var i = 0; i < qxs.length; i++) {
            $("#" + qxs[i]).attr("checked",true);
        }

        //检查第二级是否选中
        $(".tb input").each(function () {

            var myid = $(this).attr("id");

            if (myid.split('_').length != 2) {
                return;
            }

            checkIndeterminate(myid.split('_'));
        });
    }
})

function checkIndeterminate(strs_id) {
    //检查第二级
    var l2 = strs_id[0] + "_" + strs_id[1];

    var status1 = checkChildIndeterminate(l2);
    if (status1 == 2) {
        $("#" + l2)[0].indeterminate = true;
    }
    else if (status1 == 1) {
        $("#" + l2).attr("checked",true);
        $("#" + l2)[0].indeterminate = false;
    }
    else {
        $("#" + l2).attr("checked", false);
        $("#" + l2)[0].indeterminate = false;
    }

    //检查第一级
    var status2 = checkChildIndeterminate(strs_id[0]);
    if (status2 == 2) {
        $("#" + strs_id[0])[0].indeterminate = true;
    }
    else if (status1 == 1) {
        $("#" + strs_id[0]).attr("checked", true);
        $("#" + strs_id[0])[0].indeterminate = false;
    }
    else {
        $("#" + strs_id[0]).attr("checked", false);
        $("#" + strs_id[0])[0].indeterminate = false;
    }
}

//检测子级是否全选中
function checkChildIndeterminate(id) {
    var count = 0;
    var total = $(".tb input[id^='" + id + "_']").length;
    
    $(".tb input[id^='" + id + "_']").each(function () {
        if ($(this).is(":checked")) {
            count++;
        }
    });

    if (count == 0) {   //未选中
        return 0;
    }

    if (count == total) {   //全选中
        return 1;
    }

    if (count < total) {   //未全选中
        return 2;
    }

}

function save() {
    var qx = "";

    $(".tb input").each(function () {
        
        var myid = $(this).attr("id");
        if (myid.split('_').length < 3) {
            return;
        }

        if ($(this).is(":checked")) {
            console.log(myid);
            if (qx == "") {
                qx = myid;
            }
            else {
                qx += "," + myid;
            }
        }
    });

    var url = "/Admin/store/QxHandler.ashx";
    $.post(url, {roleid:roleid, qx: qx }, function (result) {
        if (result == "ok") {
            alert("保存成功!");
            history.back();
        }
        else {
            alert("保存失败：" + result);
        }
    });
}