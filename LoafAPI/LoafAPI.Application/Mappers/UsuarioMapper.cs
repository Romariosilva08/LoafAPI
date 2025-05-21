using LoafAPI.Domain.Entities;
using LoafAPI.LoafAPI.Application.DTOs;

namespace LoafAPI.Application.Mappers
{
    public static class UsuarioMapper
    {
        public static UsuarioDTO ToDTO(this Usuario usuario) =>
            new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email
            };
    }
}
