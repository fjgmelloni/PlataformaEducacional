using System.ComponentModel.DataAnnotations;

namespace PlataformaEducacional.Api.Requests.Authentication
{
    public class RegisterUserRequest
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(255, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        [Display(Name = "Senha")]
        public string Password { get; set; } = null!;

        [Compare("Password", ErrorMessage = "As senhas não conferem.")]
        [Display(Name = "Confirmação da senha")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
