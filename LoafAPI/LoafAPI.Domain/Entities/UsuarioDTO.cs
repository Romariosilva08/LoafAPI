using System.ComponentModel.DataAnnotations;

namespace LoafAPI.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; }

    [Required]
    [MaxLength(150)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MaxLength(60)]
    public string Senha { get; set; }
}
