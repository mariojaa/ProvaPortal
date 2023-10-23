using Microsoft.AspNetCore.Mvc;
using ProvaPortal.Data;
using ProvaPortal.Models;
using ProvaPortal.Repository.Interface;

namespace ProvaPortal.Controllers
{
    public class LogController : Controller
    {
        public class CustomerController : Controller
        {
            private readonly ILogRepository _customLogger;
            private readonly ProvaPortalContext _context;

            public CustomerController(ILogRepository customLogger, ProvaPortalContext context)
            {
                _customLogger = customLogger;
                _context = context;
            }

            public IActionResult Create(ProfessorModel model)
            {
                // Lógica para criar um cliente no banco de dados
                _customLogger.LogDatabaseAction($"Criou um novo cliente: {model.Matricula}");
                return View();
            }

            public IActionResult Update(int customerId, ProfessorModel model)
            {
                // Lógica para atualizar um cliente no banco de dados
                _customLogger.LogDatabaseAction($"Atualizou o cliente ID {customerId}");
                return View();
            }

            // Outras ações relacionadas ao banco de dados
        }

    }
}
