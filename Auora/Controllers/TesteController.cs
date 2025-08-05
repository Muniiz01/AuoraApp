using Auora.ConDB;
using Microsoft.AspNetCore.Mvc;

namespace Auora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TesteController : ControllerBase
    {
        private readonly ProdutoService _service;

        public TesteController(ProdutoService service)
        {
            _service = service;
        }

        [HttpGet("produtos")]
        public async Task<IActionResult> GetProdutos()
        {
            var produtos = await _service.GetAsync();
            return Ok(produtos);
        }
    }
}
