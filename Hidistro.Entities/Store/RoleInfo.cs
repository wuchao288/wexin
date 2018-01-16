namespace Hidistro.Entities.Store
{
    using System;
    using System.Runtime.CompilerServices;

    public class RoleInfo
    {
        public virtual string Description { get; set; }

        public virtual int RoleId { get; set; }

        public virtual string RoleName { get; set; }
       

        string GetQxName(int v) {
            switch (v) {
                case 1:

                    return "配置";
                case 2:

                    return "会员";
                case 3:

                    return "营销";
                case 4:

                    return "商品";
                case 5:

                    return "分销";
                case 6:

                    return "订单";
                case 7:

                    return "统计";
                case 8:

                    return "系统";

                default:

                    return "";
            }
        }

    }
}

