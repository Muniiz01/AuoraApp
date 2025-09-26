using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace Auora.ConDB
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;
        public UserService(IMongoDatabase database) 
        {
            _users = database.GetCollection<User>("users");
        }
        public async Task CreateAsync(User user)
        {
            await _users.InsertOneAsync(user);
        }
        public async Task<List<User>> GetAsync() =>
            await _users.Find(_ => true).ToListAsync();

        public async Task<User?> ValidateUser(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = await _users.Find(u => u.Email == email).FirstOrDefaultAsync();

            if (user == null)
                return null;

            
            if (!VerifyPassword(password, user.Password))
                return null;

            return user;
        }

        public static bool IsPasswordValid(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                return false;

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }

        public static string HashPassword(string password)
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[16];
            rng.GetBytes(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

       public static bool VerifyPassword(string password, string storedHash)
        {
            var parts = storedHash.Split('.');
            if (parts.Length != 2)
                return false;
            
            var salt = Convert.FromBase64String(parts[0]);
            var hash = Convert.FromBase64String(parts[1]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            var hashToCompare = pbkdf2.GetBytes(32);

            return hash.SequenceEqual(hashToCompare);
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _users.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateProfileAsync(string id, User updatedUser, int st)
        {
            if (st == 1)
            {
                var update = Builders<User>.Update
                    .Set(u => u.imgPath, updatedUser.imgPath)
                    .Set(u => u.UpdatedAt, DateTime.UtcNow);
                await _users.UpdateOneAsync(u => u.Id == id, update);
                
            }
            else { 
                var update = Builders<User>.Update
                    .Set(u => u.Name, updatedUser.Name)
                    .Set(u => u.Street, updatedUser.Street)
                    .Set(u => u.PostalCode, updatedUser.PostalCode)
                    .Set(u => u.City, updatedUser.City)
                    .Set(u => u.Country, updatedUser.Country)
                    .Set(u => u.PhoneNumber, updatedUser.PhoneNumber)
                    .Set(u => u.BirthDate, updatedUser.BirthDate)
                    .Set(u => u.UpdatedAt, DateTime.UtcNow);

            await _users.UpdateOneAsync(u => u.Id == id, update);
            }
        }
    }

    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name is mandatory")]
        [StringLength(100, ErrorMessage = "Name must be between 3 and 100 characters long", MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is mandatory")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is mandatory")]
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters long", MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

       
        public string? imgPath { get; set; }

        [StringLength(200, ErrorMessage = "O endereço não pode exceder {1} caracteres")]
        public string? Street { get; set; }

        [StringLength(20, ErrorMessage = "O PostalCode não pode exceder {1} caracteres")]
        public string? PostalCode { get; set; }

        [StringLength(100, ErrorMessage = "A cidade não pode exceder {1} caracteres")]
        public string? City { get; set; }

        [StringLength(200, ErrorMessage = "O pais não pode exceder {1} caracteres")]
        public string? Country { get; set; }

        [Phone(ErrorMessage = "Número de telefone inválido")]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
