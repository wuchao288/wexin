namespace Hidistro.Entities.VShop
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Web;

    public class TplCfgInfo
    {
        public int BannerId { get; set; }

        public int DisplaySequence { get; set; }

        public int Id
        {
            get
            {
                return this.BannerId;
            }
            set
            {
                this.Id = this.BannerId;
            }
        }

        public string ImageUrl { get; set; }

        public bool IsDisable { get; set; }

        public Hidistro.Entities.VShop.LocationType LocationType { get; set; }

        public virtual string LoctionUrl
        {
            get
            {
                int port = HttpContext.Current.Request.Url.Port;
                string str = HttpContext.Current.Request.Url.Host + ((port == 80) ? "" : (":" + port.ToString()));
                string url = string.Empty;
                switch (this.LocationType)
                {
                    case Hidistro.Entities.VShop.LocationType.Topic:
                        return string.Format("http://{0}/Vshop/Topics.aspx?TopicId={1}", str, this.Url);

                    case Hidistro.Entities.VShop.LocationType.Vote:
                        return string.Format("http://{0}/Vshop/Vote.aspx?VoteId={1}", str, this.Url);

                    case Hidistro.Entities.VShop.LocationType.Activity:
                    {
                        string[] strArray = this.Url.Split(new char[] { ',' });
                        switch (((LotteryActivityType) Enum.Parse(typeof(LotteryActivityType), strArray[0])))
                        {
                            case LotteryActivityType.Wheel:
                                return string.Format("http://{0}/Vshop/BigWheel.aspx?activityid={1}", str, strArray[1]);

                            case LotteryActivityType.Scratch:
                                return string.Format("http://{0}/Vshop/Scratch.aspx?activityid={1}", str, strArray[1]);

                            case LotteryActivityType.SmashEgg:
                                return string.Format("http://{0}/Vshop/SmashEgg.aspx?activityid={1}", str, strArray[1]);

                            case LotteryActivityType.Ticket:
                                return string.Format("http://{0}/Vshop/SignUp.aspx?id={1}", str, strArray[1]);

                            case LotteryActivityType.SignUp:
                                return string.Format("http://{0}/Vshop/Activity.aspx?id={1}", str, strArray[1]);
                        }
                        return url;
                    }
                    case Hidistro.Entities.VShop.LocationType.Home:
                        return string.Format("http://{0}/Vshop/Default.aspx", str);

                    case Hidistro.Entities.VShop.LocationType.Category:
                        return string.Format("http://{0}/Vshop/ProductList.aspx", str);

                    case Hidistro.Entities.VShop.LocationType.ShoppingCart:
                        return string.Format("http://{0}/Vshop/ShoppingCart.aspx", str);

                    case Hidistro.Entities.VShop.LocationType.OrderCenter:
                        return string.Format("http://{0}/Vshop/MemberCenter.aspx", str);

                    case Hidistro.Entities.VShop.LocationType.VipCard:
                        return url;

                    case Hidistro.Entities.VShop.LocationType.Link:
                        url = "http://" + this.Url;
                        if (this.Url.IndexOf("http") > -1)
                        {
                            url = this.Url;
                        }
                        return url;

                    case Hidistro.Entities.VShop.LocationType.Phone:
                        url = "tel://" + this.Url;
                        if (this.Url.IndexOf("tel") > -1)
                        {
                            url = this.Url;
                        }
                        return url;

                    case Hidistro.Entities.VShop.LocationType.Address:
                        return this.Url;

                    case Hidistro.Entities.VShop.LocationType.GroupBuy:
                        return "/vshop/GroupBuyList.aspx";

                    case Hidistro.Entities.VShop.LocationType.Brand:
                        return "/vshop/BrandList.aspx";
                }
                return url;
            }
        }

        public string ShortDesc { get; set; }

        public int Type { get; set; }

        public string Url { get; set; }
    }
}

