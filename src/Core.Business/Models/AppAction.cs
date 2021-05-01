using System;

namespace Core.Business.Models
{
    public class AppAction : Entity
    {
        public string ActionName { get; set; }
        public Guid ControllerId { get; set; }

        /* EF Relations */
        public AppController Controller { get; set; }
    }
}