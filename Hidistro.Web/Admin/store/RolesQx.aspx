<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="RolesQx.aspx.cs" Inherits="Hidistro.UI.Web.Admin.RolesQx" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link href="../css/store/RolesQx.css" rel="stylesheet" />
    <script src="../js/store/RolesQx.js" type="text/javascript"></script>
    <script>
        var roleid = <%=RoleId %>;
        var qx = "<%=Qx %>";
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/02.gif" width="32" height="32" /></em>
            <h1><strong>角色名称</strong></h1>
            <span><%=RoleName %></span>

        </div>

        <div style="position:absolute;right:20px;top:20px;">
            <input type="button" value="保 存" id="Button2" class="submit_DAqueding" onclick="save()" />

            <input type="button" value="返 回" class="submit_DAqueding" onclick="history.back()" style="margin-left:20px;"/>
        </div>

        <div class="datalist">
            <table class="tb">
                <tr>
                    <th style="width: 100px;">一级</th>
                    <th style="width: 150px;">二级</th>
                    <th>权限配置</th>
                </tr>
                <tr>
                    <td rowspan="5">
                        <input type="checkbox" value="" id="v1" />
                        <label for="v1">配置</label>
                    </td>
                    <td>
                        <input type="checkbox" value="" id="v1_t1" />
                        <label for="v1_t1">微信配置</label>
                    </td>
                    <td>
                        <input type="checkbox" value="" id="v1_t1_s1" />
                        <label for="v1_t1_s1">商城配置</label>

                        <input type="checkbox" value="" id="v1_t1_s2" />
                        <label for="v1_t1_s2">微信专题</label>

                        <input type="checkbox" value="" id="v1_t1_s3" />
                        <label for="v1_t1_s3">自定义回复</label>

                        <input type="checkbox" value="" id="v1_t1_s4" />
                        <label for="v1_t1_s4">自定义菜单配置</label>

                        <input type="checkbox" value="" id="v1_t1_s5" />
                        <label for="v1_t1_s5">消息提醒服务</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input id="v1_t2" type="checkbox" value="" />
                        <label for="v1_t2">首页数据配置</label></td>
                    <td>
                        <input type="checkbox" value="" id="v1_t2_s1" />
                        <label for="v1_t2_s1">首页背景图</label>

                        <input type="checkbox" value="" id="v1_t2_s2" />
                        <label for="v1_t2_s2">轮播图配置</label>

                        <input type="checkbox" value="" id="v1_t2_s3" />
                        <label for="v1_t2_s3">图标配置</label>

                        <input type="checkbox" value="" id="v1_t2_s4" />
                        <label for="v1_t2_s4">商品配置</label>

                        <input type="checkbox" value="" id="v1_t2_s5" />
                        <label for="v1_t2_s5">专题配置</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input id="v1_t3" type="checkbox" value="" />
                        <label for="v1_t3">
                            商城模板
                        </label>
                    </td>
                    <td>
                        <input id="v1_t3_s1" type="checkbox" value="" />
                        <label for="v1_t3_s1">
                            二维码图片</label>

                        <input id="v1_t3_s2" type="checkbox" value="" />
                        <label for="v1_t3_s2">
                            模板选择</label></td>
                </tr>
                <tr>
                    <td>
                        <input id="v1_t4" type="checkbox" value="" />
                        <label for="v1_t4">
                            支付设置</label></td>
                    <td>
                        <input id="v1_t4_s1" type="checkbox" value="" />
                        <label for="v1_t4_s1">
                            微信支付设置</label>

                        <input id="v1_t4_s2" type="checkbox" value="" />
                        <label for="v1_t4_s2">
                            支付宝支付设置</label>

                        <input id="v1_t4_s3" type="checkbox" value="" />
                        <label for="v1_t4_s3">
                            盛付通设置</label>

                        <input id="v1_t4_s4" type="checkbox" value="" />
                        <label for="v1_t4_s4">
                            线下支付设置</label>

                        <input id="v1_t4_s5" type="checkbox" value="" />
                        <label for="v1_t4_s5">
                            添加新支付方式</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input id="v1_t5" type="checkbox" value="" />
                        <label for="v1_t5">
                            配送设置</label></td>
                    <td>
                        <input id="v1_t5_s1" type="checkbox" value="" />
                        <label for="v1_t5_s1">
                            配送方式列表</label>

                        <input id="v1_t5_s2" type="checkbox" value="" />
                        <label for="v1_t5_s2">
                            运费模板</label>

                        <input id="v1_t5_s3" type="checkbox" value="" />
                        <label for="v1_t5_s3">
                            物流公司</label>
                    </td>
                </tr>
                <tr>
                    <td rowspan="3">
                        <input type="checkbox" value="" id="v2" />
                        <label for="v2">会员</label>
                    </td>
                    <td>
                        <input id="v2_t1" type="checkbox" value="" />
                        <label for="v2_t1">
                            会员管理</label></td>
                    <td>
                        <input id="v2_t1_s1" type="checkbox" value="" />
                        <label for="v2_t1_s1">
                            会员列表</label>

                        <input id="v2_t1_s2" type="checkbox" value="" />
                        <label for="v2_t1_s2">
                            会员等级</label>

                        <input id="v2_t1_s3" type="checkbox" value="" />
                        <label for="v2_t1_s3">
                            添加会员等级</label>

                        <input id="v2_t1_s4" type="checkbox" value="" />
                        <label for="v2_t1_s4">
                            会员卡</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input id="v2_t2" type="checkbox" value="" />
                        <label for="v2_t2">
                            会员深度营销</label></td>
                    <td>
                        <input id="v2_t2_s1" type="checkbox" value="" />
                        <label for="v2_t2_s1">
                            客户分组</label>

                        <input id="v2_t2_s2" type="checkbox" value="" />
                        <label for="v2_t2_s2">
                            新客户</label>

                        <input id="v2_t2_s3" type="checkbox" value="" />
                        <label for="v2_t2_s3">
                            活跃客户</label>

                        <input id="v2_t2_s4" type="checkbox" value="" />
                        <label for="v2_t2_s4">
                            休眠客户</label></td>
                </tr>
                <tr>
                    <td>
                        <input id="v2_t3" type="checkbox" value="" />
                        <label for="v2_t3">
                            邮件短信营销</label></td>
                    <td>
                        <input id="v2_t3_s1" type="checkbox" value="" />
                        <label for="v2_t3_s1">
                            邮件短信设置</label>

                        <input id="v2_t3_s2" type="checkbox" value="" />
                        <label for="v2_t3_s2">
                            短信套餐购买</label>

                        <input id="v2_t3_s3" type="checkbox" value="" />
                        <label for="v2_t3_s3">
                            手机短信配置</label>

                        <input id="v2_t3_s4" type="checkbox" value="" />
                        <label for="v2_t3_s4">
                            邮件服务配置</label></td>
                </tr>

                <tr>
                    <td rowspan="6">
                        <input type="checkbox" value="" id="v3" />
                        <label for="v3">营销</label>
                    </td>
                    <td>
                        <input id="v3_t1" type="checkbox" value="" />
                        <label for="v3_t1">
                            积分商城</label></td>
                    <td>
                        <input id="v3_t1_s1" type="checkbox" value="" />
                        <label for="v3_t1_s1">
                            购物积分设置</label></td>
                </tr>

                <tr>
                    <td>
                        <input id="v3_t2" type="checkbox" value="" />
                        <label for="v3_t2">
                            店铺促销活动</label></td>
                    <td>
                        <input id="v3_t2_s1" type="checkbox" value="" />
                        <label for="v3_t2_s1">
                            团购</label></td>
                </tr>
                <tr>
                    <td>
                        <input id="v3_t3" type="checkbox" value="" />
                        <label for="v3_t3">
                            优惠劵</label></td>
                    <td>
                        <input id="v3_t3_s1" type="checkbox" value="" />
                        <label for="v3_t3_s1">
                            优惠劵列表</label>

                        <input id="v3_t3_s2" type="checkbox" value="" />
                        <label for="v3_t3_s2">
                            优惠劵活动效果</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input id="v3_t4" type="checkbox" value="" />
                        <label for="v3_t4">
                            团购</label></td>
                    <td>
                        <input id="v3_t4_s1" type="checkbox" value="" />
                        <label for="v3_t4_s1">
                            新增一个团购</label>

                        <input id="v3_t4_s2" type="checkbox" value="" />
                        <label for="v3_t4_s2">
                            团购列表</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input id="v3_t5" type="checkbox" value="" />
                        <label for="v3_t5">
                            积分设置</label></td>
                    <td>
                        <input id="v3_t5_s1" type="checkbox" value="" />
                        <label for="v3_t5_s1">
                            购物积分设置</label></td>
                </tr>
                <tr>
                    <td>
                        <input id="v3_t6" type="checkbox" value="" />
                        <label for="v3_t6">
                            微信活动</label></td>
                    <td>
                        <input id="v3_t6_s1" type="checkbox" value="" />
                        <label for="v3_t6_s1">
                            微投票</label>

                        <input id="v3_t6_s2" type="checkbox" value="" />
                        <label for="v3_t6_s2">
                            大转盘</label>

                        <input id="v3_t6_s3" type="checkbox" value="" />
                        <label for="v3_t6_s3">
                            刮刮卡</label>

                        <input id="v3_t6_s4" type="checkbox" value="" />
                        <label for="v3_t6_s4">
                            砸金蛋</label>

                        <input id="v3_t6_s5" type="checkbox" value="" />
                        <label for="v3_t6_s5">
                            微报名</label>

                        <input id="v3_t6_s6" type="checkbox" value="" />
                        <label for="v3_t6_s6">
                            微抽奖</label>
                    </td>
                </tr>
                <tr>
                    <td rowspan="3">
                        <input type="checkbox" value="" id="v4" />
                        <label for="v4">商品</label>
                    </td>
                    <td>
                        <input type="checkbox" value="" id="v4_t1" />
                        <label for="v4_t1">商品管理</label></td>
                    <td>
                        <input id="v4_t1_s1" type="checkbox" value="" />
                        <label for="v4_t1_s1">
                            上传新商品</label>

                        <input id="v4_t1_s2" type="checkbox" value="" />
                        <label for="v4_t1_s2">
                            商品列表</label>

                        <input id="v4_t1_s3" type="checkbox" value="" />
                        <label for="v4_t1_s3">
                            商品回收站</label>

                        <input id="v4_t1_s4" type="checkbox" value="" />
                        <label for="v4_t1_s4">
                            批量上传商品</label>

                        <input id="v4_t1_s5" type="checkbox" value="" />
                        <label for="v4_t1_s5">
                            批量导出商品</label></td>
                </tr>
                <tr>
                    <td>
                        <input id="v4_t2" type="checkbox" value="" />
                        <label for="v4_t2">
                            商品配置</label></td>
                    <td>
                        <input id="v4_t2_s1" type="checkbox" value="" />
                        <label for="v4_t2_s1">
                            类型配置</label>

                        <input id="v4_t2_s2" type="checkbox" value="" />
                        <label for="v4_t2_s2">
                            分类配置</label>

                        <input id="v4_t2_s3" type="checkbox" value="" />
                        <label for="v4_t2_s3">
                            品牌配置</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input id="v4_t3" type="checkbox" value="" />
                        <label for="v4_t3">
                            客户反馈</label></td>
                    <td>
                        <input id="v4_t3_s1" type="checkbox" value="" />
                        <label for="v4_t3_s1">
                            商品咨询</label>

                        <input id="v4_t3_s2" type="checkbox" value="" />
                        <label for="v4_t3_s2">
                            商品评论</label></td>
                </tr>
                <tr>
                    <td rowspan="3">
                        <input type="checkbox" value="" id="v5" />
                        <label for="v5">分销</label>
                    </td>
                    <td>
                        <input id="v5_t1" type="checkbox" value="" />
                        <label for="v5_t1">
                            分销商管理</label></td>
                    <td>
                        <input id="v5_t1_s1" type="checkbox" value="" />
                        <label for="v5_t1_s1">
                            分销商申请设置</label></td>
                </tr>
                <tr>
                    <td>
                        <input id="v5_t2" type="checkbox" value="" />
                        <label for="v5_t2">
                            商品朋友圈分享</label></td>
                    <td>
                        <input id="v5_t2_s1" type="checkbox" value="" />
                        <label for="v5_t2_s1">
                            朋友圈分享</label></td>
                </tr>
                <tr>
                    <td>
                        <input id="v5_t3" type="checkbox" value="" />
                        <label for="v5_t3">
                            售后服务</label></td>
                    <td>
                        <input id="v5_t3_s1" type="checkbox" value="" />
                        <label for="v5_t3_s1">
                            售后服务</label></td>
                </tr>
                <tr>
                    <td rowspan="4">
                        <input type="checkbox" value="" id="v6" />
                        <label for="v6">订单</label>
                    </td>
                    <td>
                        <input id="v6_t1" type="checkbox" value="" />
                        <label for="v6_t1">
                            订单管理</label></td>
                    <td>
                        <input id="v6_t1_s1" type="checkbox" value="" />
                        <label for="v6_t1_s1">
                            订单列表</label>

                        <input id="v6_t1_s2" type="checkbox" value="" />
                        <label for="v6_t1_s2">
                            订单设置</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input id="v6_t2" type="checkbox" value="" />
                        <label for="v6_t2">
                            单据管理</label></td>
                    <td>
                        <input id="v6_t2_s1" type="checkbox" value="" />
                        <label for="v6_t2_s1">
                            收款单</label>

                        <input id="v6_t2_s2" type="checkbox" value="" />
                        <label for="v6_t2_s2">
                            发货单</label></td>
                </tr>
                <tr>
                    <td>
                        <input id="v6_t3" type="checkbox" value="" />
                        <label for="v6_t3">
                            快递单管理</label></td>
                    <td>
                        <input id="v6_t3_s1" type="checkbox" value="" />
                        <label for="v6_t3_s1">
                            快递单模板</label>

                        <input id="v6_t3_s2" type="checkbox" value="" />
                        <label for="v6_t3_s2">
                            新增快递单模板</label>

                        <input id="v6_t3_s3" type="checkbox" value="" />
                        <label for="v6_t3_s3">
                            发货人信息管理</label>

                        <input id="v6_t3_s4" type="checkbox" value="" />
                        <label for="v6_t3_s4">
                            添加发货人信息</label>


                    </td>
                </tr>
                <tr>
                    <td>
                        <input id="v6_t4" type="checkbox" value="" />
                        <label for="v6_t4">
                            微信通知管理</label></td>
                    <td>
                        <input id="v6_t4_s1" type="checkbox" value="" />
                        <label for="v6_t4_s1">
                            反馈通知</label>

                        <input id="v6_t4_s2" type="checkbox" value="" />
                        <label for="v6_t4_s2">
                            告警通知</label></td>
                </tr>
                <tr>
                    <td rowspan="2">
                        <input type="checkbox" value="" id="v7" />
                        <label for="v7">统计</label>
                    </td>
                    <td>
                        <input id="v7_t1" type="checkbox" value="" />
                        <label for="v7_t1">
                            网站流量统计</label></td>
                    <td>
                        <input id="v7_t1_s1" type="checkbox" value="" />
                        <label for="v7_t1_s1">
                            统计功能配置</label>

                        <input id="v7_t1_s2" type="checkbox" value="" />
                        <label for="v7_t1_s2">
                            查看统计结果</label></td>
                </tr>
                <tr>
                    <td>
                        <input id="v7_t2" type="checkbox" value="" />
                        <label for="v7_t2">
                            业务统计</label></td>
                    <td>
                        <input id="v7_t2_s1" type="checkbox" value="" />
                        <label for="v7_t2_s1">
                            生意报告</label>

                        <input id="v7_t2_s2" type="checkbox" value="" />
                        <label for="v7_t2_s2">
                            订单统计</label>

                        <input id="v7_t2_s3" type="checkbox" value="" />
                        <label for="v7_t2_s3">
                            商品销售明细</label>

                        <input id="v7_t2_s4" type="checkbox" value="" />
                        <label for="v7_t2_s4">
                            销售指标分析</label>

                        <input id="v7_t2_s5" type="checkbox" value="" />
                        <label for="v7_t2_s5">
                            商品销售排行</label>

                        <input id="v7_t2_s6" type="checkbox" value="" />
                        <label for="v7_t2_s6">
                            商品访问与购买比</label>

                        <input id="v7_t2_s7" type="checkbox" value="" />
                        <label for="v7_t2_s7">
                            会员消费排行</label>

                        <input id="v7_t2_s8" type="checkbox" value="" />
                        <label for="v7_t2_s8">
                            会员地区分析</label>

                        <input id="v7_t2_s9" type="checkbox" value="" />
                        <label for="v7_t2_s9">
                            会员增长统计</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="checkbox" value="" id="v8" />
                        <label for="v8">系统</label>
                    </td>
                    <td>
                        <input id="v8_t1" type="checkbox" value="" />
                        <label for="v8_t1">
                            安全中心</label></td>
                    <td>
                        <input id="v8_t1_s1" type="checkbox" value="" />
                        <label for="v8_t1_s1">
                            部门管理</label>

                        <input id="v8_t1_s2" type="checkbox" value="" />
                        <label for="v8_t1_s2">
                            管理员管理</label>

                        <input id="v8_t1_s3" type="checkbox" value="" />
                        <label for="v8_t1_s3">
                            操作日志</label>
                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <input id="v8_t2" type="checkbox" value="" />
                        <label for="v8_t2">
                            数据库管理</label></td>
                    <td>
                        <input id="v8_t2_s1" type="checkbox" value="" />
                        <label for="v8_t2_s1">
                            数据库备份</label>

                        <input id="v8_t2_s2" type="checkbox" value="" />
                        <label for="v8_t2_s2">
                            数据库恢复</label></td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <input id="v8_t3" type="checkbox" value="" />
                        <label for="v8_t3">
                            图库管理</label></td>
                    <td>
                        <input id="v8_t3_s1" type="checkbox" value="" />
                        <label for="v8_t3_s1">
                            图片库</label>

                        <input id="v8_t3_s2" type="checkbox" value="" />
                        <label for="v8_t3_s2">
                            上传图片</label>

                        <input id="v8_t3_s3" type="checkbox" value="" />
                        <label for="v8_t3_s3">
                            图片分类管理</label>
                    </td>
                </tr>

            </table>

            <div style="padding: 20px;">
                <input type="button" value="保 存" id="ctl00_contentHolder_btnCreate" class="submit_DAqueding" onclick="save()" />

                <input type="button" value="返 回" class="submit_DAqueding" onclick="history.back()" style="margin-left:20px;"/>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
