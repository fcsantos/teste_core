using Core.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Core.Web.Controllers
{
    public class MainController : Controller
    {
        protected bool ResponseHasErrors(ResponseResult response)
        {
            if (response != null && response.Errors.Messages.Any())
            {
                foreach (var message in response.Errors.Messages)
                {
                    ModelState.AddModelError(string.Empty, message);
                } 

                return true;
            }

            return false;
        }
    }
}