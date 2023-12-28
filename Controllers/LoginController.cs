using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProvaPortal.Models;
using ProvaPortal.Repository.Interface;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace ProvaPortal.Controllers
{
    public class LoginController : Controller
    {
        private readonly IProfessorRepository _professorRepository;
        private readonly ISessao _sessao;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly string API_ENDPOINT = "https://localhost:44389/api/login/entrar";

        public LoginController(IProfessorRepository professorRepository, ISessao sessao, HttpClient httpClient, IMapper mapper)
        {
            _professorRepository = professorRepository;
            _sessao = sessao;
            _httpClient = httpClient;
            _mapper = mapper;
            _httpClient.BaseAddress = new Uri(API_ENDPOINT);
        }

        public IActionResult Index()
        {
            var usuarioLogado = _sessao.BuscarSessaoUsuario();
            if (usuarioLogado != null)
            {
                return RedirectToAction("Index", "Provas");
            }

            return View();
        }

        public IActionResult Sair()
        {
            _sessao.RemoverSessaoUsuario();
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<ProfessorDTO>> Entrar(LoginModel novoLogin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var loginModel = new LoginModel
                    {
                        Login = novoLogin.Login,
                        Senha = novoLogin.Senha,
                    };

                    var novoUsuarioJson = JsonConvert.SerializeObject(loginModel);

                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var content = new StringContent(novoUsuarioJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await _httpClient.PostAsync("/api/login/entrar", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        try
                        {
                            var responseData = JsonConvert.DeserializeObject<ProfessorDTO>(responseContent);

                            if (responseData != null)
                            {
                                _sessao.CriarSessaoUsuario(responseData);

                                TempData["MensagemSucesso"] = "Login feito com sucesso!";
                                return RedirectToAction("Index", "Provas");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Dados do professor não encontrados na resposta da API.");
                            }
                        }
                        catch (JsonException ex)
                        {
                            ModelState.AddModelError("", "Erro ao desserializar a resposta JSON da API: " + ex.Message);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Erro ao consultar o usuário na API. Status: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro ao consultar o usuário: " + ex.Message);
                }
            }

            return View(novoLogin);
        }
    }
}
