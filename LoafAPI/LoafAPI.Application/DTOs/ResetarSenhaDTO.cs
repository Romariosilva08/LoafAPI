using System.ComponentModel.DataAnnotations;

namespace LoafAPI.LoafAPI.Application.DTOs;

public class ResetarSenhaDTO
{
    [Required(ErrorMessage = "O token é obrigatório.")]
    public string Token { get; set; }

    [Required(ErrorMessage = "A nova senha é obrigatória.")]
    [StringLength(20, ErrorMessage = "A senha deve ter entre {2} e {1} caracteres.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string NovaSenha { get; set; }
}
