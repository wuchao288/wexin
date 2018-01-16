$(function () {
    var url = location.href.split('#')[0];
    $.post("/API/WxHandler.ashx", { url: url }, function (result) {
        var objData = eval("(" + result + ")");
        wx.config({
            debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
            appId: 'wx0451bd8ab0150996', // 必填，公众号的唯一标识
            timestamp: objData.timestamp, // 必填，生成签名的时间戳
            nonceStr: objData.noncestr, // 必填，生成签名的随机串
            signature: objData.signature,// 必填，签名，见附录1
            jsApiList: ["onMenuShareAppMessage", "onMenuShareTimeline", "onMenuShareQQ", "onMenuShareWeibo"] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
        });

        wx.ready(function () {

            // config信息验证后会执行ready方法，所有接口调用都必须在config接口获得结果之后，config是一个客户端的异步操作，所以如果需要在页面加载时就调用相关接口，则须把相关接口放在ready函数中调用来确保正确执行。对于用户触发时才调用的接口，则可以直接调用，不需要放在ready函数中。
            //ReferralId
            var ReferralId = $.cookie('Vshop-ReferralId');

            var data = {
                title:document.title,
                desc: document.title,
                link: "http://ncp.tewuwang.com/Vshop/Default.aspx?ReferralId=" + ReferralId,
                imgUrl: "http://ncp.tewuwang.com/Utility/pics/headLogo.jpg"
            };

            if (url.indexOf("ProductDetails.aspx") != -1) {
                data.link = location.href;
            }

            //if (ReferralId == "") {
            //    alert("推荐人未识别到，请先登录后再分享.");
            //}

            wx.onMenuShareTimeline({
                title: data.title, // 分享标题
                link: data.link, // 分享链接
                imgUrl: data.imgUrl, // 分享图标
                success: function () {
                    // 用户确认分享后执行的回调函数
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });

            wx.onMenuShareAppMessage({
                title: data.title, // 分享标题
                desc: data.desc, // 分享描述
                link: data.link, // 分享链接
                imgUrl: data.imgUrl, // 分享图标
                type: '', // 分享类型,music、video或link，不填默认为link
                dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
                success: function () {
                    // 用户确认分享后执行的回调函数
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });

            wx.onMenuShareQQ({
                title: data.title, // 分享标题
                desc: data.desc, // 分享描述
                link: data.link, // 分享链接
                imgUrl: data.imgUrl, // 分享图标
                success: function () {
                    // 用户确认分享后执行的回调函数
                    alert("分享成功");
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });

            wx.onMenuShareWeibo({
                title: data.title, // 分享标题
                desc: data.desc, // 分享描述
                link: data.link, // 分享链接
                imgUrl: data.imgUrl, // 分享图标
                success: function () {
                    // 用户确认分享后执行的回调函数
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });

            wx.onMenuShareQZone({
                title: data.title, // 分享标题
                desc: data.desc, // 分享描述
                link: data.link, // 分享链接
                imgUrl: data.imgUrl, // 分享图标
                success: function () {
                    // 用户确认分享后执行的回调函数
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });
        });
    });
});