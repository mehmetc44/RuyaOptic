using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuyaOptik.Business.Exceptions
{
    public class NotFoundRoleException : Exception
    {
        public NotFoundRoleException() : base("Rol Bulunamadı")
        {
        }

        public NotFoundRoleException(string? message) : base(message)
        {
        }

        public NotFoundRoleException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
