using RuyaOptik.Entity.Common;
using RuyaOptik.Entity.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RuyaOptik.Entity.Configurations;

namespace RuyaOptik.Entity.Concrete
{
    public class Endpoint : BaseEntity
    {
        public Endpoint()
        {
            Roles = new HashSet<AspRole>();
        }
        public string ActionType { get; set; }
        public string HttpType { get; set; }
        public string Definition { get; set; }
        public string Code { get; set; }

        public Menu Menu { get; set; }
        public ICollection<AspRole> Roles { get; set; }
    }
}
