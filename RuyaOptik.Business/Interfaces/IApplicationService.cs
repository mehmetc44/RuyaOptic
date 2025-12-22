using RuyaOptik.DTO.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuyaOptik.Business.Interfaces
{
    public interface IApplicationService
    {
        List<Menu> getAuthorizeDefinitionEndpoints(Type type);
    }
}
