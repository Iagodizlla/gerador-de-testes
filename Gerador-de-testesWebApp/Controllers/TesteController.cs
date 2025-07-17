using Gerador_de_testes.Infraestrutura.Orm.Compartilhado;
using Gerador_de_testes.ModuloDeTestes;
using Gerador_de_testesWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gerador_de_testesWebApp.Controllers
{
    [Route("testes")]
    public class TesteController : Controller
    {
        private readonly IRepositorioTestes repositorioTestes;

        public TesteController(IRepositorioTestes repositorioTestes)
        {
            this.repositorioTestes = repositorioTestes;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var registros = repositorioTestes.SelecionarRegistros();

            var visualizarVM = new VisualizarTesteViewModel(registros);

            return View(visualizarVM);
        }

    }
}
