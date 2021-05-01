using Core.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Core.Web.Extensions
{
    public class PaginationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IPagedList modelPaged)
        {
            return View(modelPaged);
        }
    }
}
