namespace Hidistro.Entities
{
    using System;
    using System.Runtime.CompilerServices;

    public class RefundInfo
    {
        public string AdminRemark { get; set; }

        public DateTime ApplyForTime { get; set; }

        public Handlestatus HandleStatus { get; set; }

        public DateTime HandleTime { get; set; }

        public string Operator { get; set; }

        public string OrderId { get; set; }

        public int RefundId { get; set; }

        public string RefundRemark { get; set; }

        public enum Handlestatus
        {
            Applied = 1,
            Refunded = 2,
            Refused = 3
        }
    }
}

