using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Store
{
    public class MenuQxInfo
    {
        public virtual int id { get; set; }

        public virtual string qx { get; set; }

        public virtual string category { get; set; }

        public virtual string title { get; set; }

        public virtual string link { get; set; }
    }
}
