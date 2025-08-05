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
        public Produto ProdutoDestaque { get; set; }

        public async Task OnGetAsync()
        {
           var todosProdutos = await _service.GetAsync();

            ProdutoDestaque = todosProdutos.OrderByDescending(p => p.CreatedAt).FirstOrDefault();

            Produtos = todosProdutos;
        }
    }
}
