using ProvaPortal.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ProvaPortal.Controllers
{
    [PaginaUsuarioLogado]
    public class RestritoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
