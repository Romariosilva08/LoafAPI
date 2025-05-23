using System.ComponentModel.DataAnnotations;

namespace LoafAPI.LoafAPI.Application.DTOs;

public class LoginDTO
{
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Formato de e-mail inválido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Senha é obrigatória")]
    [StringLength(20, MinimumLength = 4, ErrorMessage = "A senha deve ter entre 4 e 20 caracteres.")]
    [DataType(DataType.Password)]
    public string Senha { get; set; }
}
