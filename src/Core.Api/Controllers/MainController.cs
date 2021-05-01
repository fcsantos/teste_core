using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Core.Business.Intefaces;
using Core.Business.Notifications;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Core.Api.Controllers
{
    [ApiController] 
    public abstract class MainController : ControllerBase
    {
        private readonly INotifier _notification;
        public readonly IUser AppUser;

        protected Guid UserId { get; set; }
        protected bool UserAuthenticated { get; set; }

        protected MainController(INotifier notification,
                                 IUser appUser)
        {
            _notification = notification;
            AppUser = appUser;

            if (appUser.IsAuthenticated())
            {
                UserId = appUser.GetUserId();
                UserAuthenticated = true;
            }
        }

        protected bool ValidOperation()
        {
            return !_notification.HasNotification();
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (ValidOperation())
            {
                return Ok(result);
            }
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Messages", _notification.GetNotifications().Select(n => n.Message).ToArray() }
            }));
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotifyErrorModelInvalida(modelState);
            return CustomResponse();
        }

        protected void NotifyErrorModelInvalida(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotifyError(errorMsg);
            }
        }

        protected void NotifyError(string message)
        {
            _notification.Handle(new Notification(message));
        }
    }
}
