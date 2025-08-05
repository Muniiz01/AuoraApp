using Microsoft.AspNetCore.Mvc;
using Auora.ConDB;

namespace Auora.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly ProdutoService _service;

        public ProdutosController(ProdutoService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var produtos = await _service.GetAsync();
            return View(produtos);
        }
    }
}
