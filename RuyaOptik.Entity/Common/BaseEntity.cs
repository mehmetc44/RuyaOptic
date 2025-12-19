using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuyaOptik.Entity.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}