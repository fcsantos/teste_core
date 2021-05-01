using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Web.Models
{
    public class AppControllerViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Controller")]
        public string ControllerName { get; set; }
        [Display(Name = "Controller")]
        public IEnumerable<AppActionViewModel> Actions { get; set; }
    }

    public class AppActionViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid ControllerId { get; set; }
        [Display(Name = "Controller")]
        public string ControllerName { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Ação")]
        public string ActionName { get; set; }

    }

    public class ControllerActionsViewModel
    {
        public string Id { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string UserId { get; set; }
        public IEnumerable<UserClaimsViewModel> ListOfClaims { get; set; }
    }

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

}