using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Auora.ConDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Auora.Pages
{
    public class ProductModel : PageModel
    {

        private readonly ILogger<ProductModel> _logger;
        private readonly ProdutoService _service;
        

        public ProductModel(ILogger<ProductModel> logger, ProdutoService service)
        {
            _logger = logger;
            _service = service;
            
        }

        public Produto Produto { get; set; } = new Produto();

        public async Task<ActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            Produto = await _service.GetByIdAsync(id);

                if (Produto == null) return NotFound();

                return Page();
            
        }
    }
}
