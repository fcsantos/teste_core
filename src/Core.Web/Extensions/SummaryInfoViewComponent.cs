using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Core.Web.Extensions
{
    public class SummaryInfoViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
