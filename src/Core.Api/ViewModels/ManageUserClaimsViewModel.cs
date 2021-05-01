using System;

namespace Core.Api.ViewModels
{
    public class AllUsersViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string UserType { get; set; }
    }

    public class UserClaimsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public bool asClaim { get; set; }
    }

    public class ControllerActionsViewModel
    {
        public string Id { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string UserId { get; set; }

    }



}
