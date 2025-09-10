using Microsoft.AspNetCore.Mvc.RazorPages;
using Auora.ConDB;

namespace Auora.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ProdutoService _service;
        private readonly UserService _userService;


        public IndexModel(ILogger<IndexModel> logger, ProdutoService service, UserService user)
        {
            _logger = logger;
            _service = service;
            _userService = user;

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
