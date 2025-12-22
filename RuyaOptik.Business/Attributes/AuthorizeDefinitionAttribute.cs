using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RuyaOptik.Entity.Enums;

namespace RuyaOptik.Business.Attributes
{
    public  class AuthorizeDefinitionAttribute : Attribute
    {
        public string Menu {  get; set; }
        public string Definition {  get; set; }
        public ActionType Action { get; set; }
    }
}
