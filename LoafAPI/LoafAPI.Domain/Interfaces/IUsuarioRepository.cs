using LoafAPI.Domain.Entities;

namespace LoafAPI.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<IEnumerable<Usuario>> ObterTodosAsync();
    Task<Usuario> ObterPorIdAsync(int id);
    Task<Usuario> ObterPorEmailAsync(string email);
    Task AdicionarAsync(Usuario usuario);
}
