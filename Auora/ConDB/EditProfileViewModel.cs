using System.ComponentModel.DataAnnotations;

namespace Auora.ConDB
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
        public string Name { get; set; } = string.Empty;

        public string? imgPath { get; set; }

        [StringLength(200, ErrorMessage = "O endereço não pode exceder 200 caracteres")]
        public string? Address { get; set; }

        [StringLength(200, ErrorMessage = "O país não pode exceder 200 caracteres")]
        public string? Country { get; set; }

        [Phone(ErrorMessage = "Número de telefone inválido")]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
    }
}
