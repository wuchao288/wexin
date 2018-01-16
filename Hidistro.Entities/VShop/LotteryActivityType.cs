namespace Hidistro.Entities.VShop
{
    using System;

    public enum LotteryActivityType
    {
        [EnumShowText("刮刮卡")]
        Scratch = 2,
        [EnumShowText("微报名")]
        SignUp = 5,
        [EnumShowText("砸金蛋")]
        SmashEgg = 3,
        [EnumShowText("微抽奖")]
        Ticket = 4,
        [EnumShowText("大转盘")]
        Wheel = 1
    }
}

