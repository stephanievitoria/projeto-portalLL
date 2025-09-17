using Microsoft.AspNetCore.Mvc;
using PortalLL.Models;

namespace PortalLL.Controllers
{
    public class PortalController : Controller
    {
        private static List<Usuario> usuarios = new();
        private static List<LicaoAprendida> licoes = new();

        public IActionResult Index() => View();

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string email, string senha)
        {
            var usuario = usuarios.FirstOrDefault(u => u.Email == email && u.Senha == senha);
            if (usuario == null)
            {
                TempData["ErroLogin"] = "E-mail ou senha incorretos.";
                return RedirectToAction("Login");
            }

            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
            HttpContext.Session.SetString("UsuarioNome", usuario.Nome);
            HttpContext.Session.SetString("UsuarioNivel", usuario.Nivel);
            HttpContext.Session.SetString("UsuarioEquipe", usuario.Equipe);

            return RedirectToAction("Index");
        }

        public IActionResult Cadastrar() => View();

        [HttpPost]
        public IActionResult Cadastrar(string nome, string email, string equipe, string nivel, string senha)
        {
            var usuario = new Usuario
            {
                Nome = nome,
                Email = email,
                Equipe = equipe,
                Nivel = nivel,
                Senha = senha
            };
            usuario.Id = usuarios.Count + 1;
            usuarios.Add(usuario);

            TempData["Mensagem"] = "Usuário cadastrado com sucesso!";
            TempData["Origem"] = "cadastro";
            return RedirectToAction("Login");
        }

        public IActionResult Criar()
        {
            if (HttpContext.Session.GetInt32("UsuarioId") == null)
                return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        public IActionResult Criar(LicaoAprendida ll)
        {
            if (HttpContext.Session.GetInt32("UsuarioId") == null)
                return RedirectToAction("Login");

            ll.Id = licoes.Count + 1;
            ll.UsuarioId = HttpContext.Session.GetInt32("UsuarioId") ?? 0;
            ll.Status = StatusLicao.Pendente;
            licoes.Add(ll);

            return RedirectToAction("Index");
        }

        public IActionResult Clarificar()
{
    var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
    if (usuarioId == null)
        return RedirectToAction("Login");

    var usuario = usuarios.FirstOrDefault(u => u.Id == usuarioId);
    if (usuario == null)
        return RedirectToAction("Login");

    // 👇 Inclui lições Pendente ou Reprovada
    var licoesDaEquipe = licoes
        .Where(l => l.Equipe == usuario.Equipe && 
                   (l.Status == StatusLicao.Pendente || l.Status == StatusLicao.Reprovada))
        .ToList();

    ViewBag.NivelUsuario = usuario.Nivel;
    return View(licoesDaEquipe);
}

        [HttpPost]
        public IActionResult SalvarClarificacao(int id, string novaClarificacao, string acao)
        {
            var usuarioNivel = HttpContext.Session.GetString("UsuarioNivel");
            var licao = licoes.FirstOrDefault(l => l.Id == id);

            if (licao != null && !string.IsNullOrWhiteSpace(novaClarificacao))
            {
                if (usuarioNivel == "pleno" || usuarioNivel == "senior")
                {
                    licao.Clarificacoes.Add(novaClarificacao);

                    if (acao == "aprovar")
                        licao.Status = StatusLicao.Aprovada;
                    else if (acao == "reprovar")
                        licao.Status = StatusLicao.Reprovada;
                }
            }

            return RedirectToAction("Clarificar");
        }

        public IActionResult Consultar(string busca)
        {
            if (HttpContext.Session.GetInt32("UsuarioId") == null)
                return RedirectToAction("Login");

            var resultado = string.IsNullOrEmpty(busca)
                ? licoes.Where(l => l.Status == StatusLicao.Aprovada).ToList()
                : licoes.Where(l =>
                    (l.Titulo.Contains(busca, StringComparison.OrdinalIgnoreCase) ||
                     l.Descricao.Contains(busca, StringComparison.OrdinalIgnoreCase) ||
                     l.Equipe.Contains(busca, StringComparison.OrdinalIgnoreCase) ||
                     l.Software.Contains(busca, StringComparison.OrdinalIgnoreCase)) &&
                     l.Status == StatusLicao.Aprovada
                ).ToList();

            return View(resultado);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult Editar(int id)
        {
            if (HttpContext.Session.GetInt32("UsuarioId") == null)
                return RedirectToAction("Login");

            var licao = licoes.FirstOrDefault(l => l.Id == id);
            if (licao == null || licao.UsuarioId != HttpContext.Session.GetInt32("UsuarioId"))
                return RedirectToAction("Consultar");

            return View(licao);
        }

        [HttpPost]
        public IActionResult Editar(LicaoAprendida licaoEditada)
        {
            var original = licoes.FirstOrDefault(l => l.Id == licaoEditada.Id);
            if (original != null && original.UsuarioId == HttpContext.Session.GetInt32("UsuarioId"))
            {
                original.Titulo = licaoEditada.Titulo;
                original.Descricao = licaoEditada.Descricao;
                original.Software = licaoEditada.Software;
                original.Equipe = licaoEditada.Equipe;
                original.Status = StatusLicao.Pendente;
            }

            return RedirectToAction("Clarificar");
        }

        public IActionResult Excluir(int id)
        {
            var licao = licoes.FirstOrDefault(l => l.Id == id);
            if (licao != null && licao.UsuarioId == HttpContext.Session.GetInt32("UsuarioId"))
            {
                licoes.Remove(licao);
            }

            return RedirectToAction("Consultar");
        }
    }
}
