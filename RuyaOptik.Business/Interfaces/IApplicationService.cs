using RuyaOptik.Entity.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuyaOptik.Business.Interfaces
{
    public interface IApplicationService
    {
        Task<List<Menu>> GetAuthorizeDefinitionEndpoints(Type type);
    }
}
