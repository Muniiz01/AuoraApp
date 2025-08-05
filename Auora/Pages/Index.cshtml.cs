using Microsoft.AspNetCore.Mvc.RazorPages;
using Auora.ConDB;

namespace Auora.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ProdutoService _service;

        public IndexModel(ILogger<IndexModel> logger, ProdutoService service)
        {
            _logger = logger;
            _service = service;
        }

        public List<Produto> Produtos { get; set; } = new List<Produto>();

        public async Task OnGetAsync()
        {
            Produtos = await _service.GetAsync();
        }
    }
}
