using Microsoft.AspNetCore.Mvc;

namespace Esport.Backend.Controllers
{
    public class DefaultController : ControllerBase
    {
        [Route(""), HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public RedirectResult RedirectToSwaggerUi()
        {
            return Redirect("/swagger/");
        }
    }
}
