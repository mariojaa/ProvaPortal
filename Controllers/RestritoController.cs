using ProvaPortal.Filters;
using Microsoft.AspNetCore.Mvc;
using ProvaPortal.Models;

namespace ProvaPortal.Controllers
{
    [PaginaUsuarioLogado]
    //[LogAction]
    public class RestritoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
