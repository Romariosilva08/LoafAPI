using System.ComponentModel.DataAnnotations;

namespace LoafAPI.LoafAPI.Application.DTOs;

public class EsqueciSenhaDTO
{
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
    public string Email { get; set; }
}
