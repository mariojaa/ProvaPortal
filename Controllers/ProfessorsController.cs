using Microsoft.AspNetCore.Mvc;
using ProvaPortal.Repository.Interface;
using ProvaPortal.Filters;

namespace ProvaPortal.Controllers
{
    [PaginaSomenteAdmin]
    public class ProfessorController : Controller
    {
        private readonly IProfessorRepository _professorRepository;
        private readonly HttpClient _httpClient;
        private readonly string API_ENDPOINT = "https://localhost:44389/api/professor";

        public ProfessorController(IProfessorRepository professorRepository)
        {
            _professorRepository = professorRepository;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(API_ENDPOINT)
            };
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("/api/professor");
                response.EnsureSuccessStatusCode();

                var professores = await response.Content.ReadAsAsync<IEnumerable<ProfessorDTO>>();
                return View(professores);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Erro ao buscar professores: " + ex.Message;
                return View(new List<ProfessorDTO>());
            }
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"/api/professor/{id}");
                response.EnsureSuccessStatusCode();

                var professor = await response.Content.ReadAsAsync<ProfessorDTO>();
                return View(professor);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Erro ao buscar detalhes do professor: " + ex.Message;
                return View(new ProfessorDTO());
            }
        }

    }
}