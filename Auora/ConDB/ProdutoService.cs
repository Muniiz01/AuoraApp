using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Auora.ConDB
{
    public class ProdutoService
    {
        private readonly IMongoCollection<Produto> _produtos;

        public ProdutoService(IMongoDatabase database)
        {
            _produtos = database.GetCollection<Produto>("produtos");
        }

        public async Task<List<Produto>> GetAsync() =>
            await _produtos.Find(_ => true).ToListAsync();

        public async Task CreateAsync(Produto produto) =>
            await _produtos.InsertOneAsync(produto);

        public async Task<Produto?> GetByIdAsync(string id)
        {
            var filter = Builders<Produto>.Filter.Eq("_id", ObjectId.Parse(id));
            return await _produtos.Find(filter).FirstOrDefaultAsync();
        }
    }
    [BsonIgnoreExtraElements] 
    public class Produto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("stock")]
        public int Stock { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("brand")]
        public string Brand { get; set; }

        [BsonElement("images")]
        public List<string> Images { get; set; }

        [BsonElement("attributes")]
        public ProdutoAttributes Attributes { get; set; }

        [BsonElement("isActive")]
        public bool IsActive { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }
        [BsonElement("resume")]
        public string Resume { get; set; }
    }

    public class ProdutoAttributes
    {
        [BsonElement("volume")]
        public string Volume { get; set; }

        [BsonElement("tipo_pele")]
        public string TipoPele { get; set; }

        [BsonElement("textura")]
        public string Textura { get; set; }
    }

}