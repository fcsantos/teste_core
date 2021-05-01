using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.Models
{
    public class AppController : Entity
    {
        public string ControllerName { get; set; }

        /* EF Relations */
        public IEnumerable<AppAction> Actions { get; set; }
    }
}
