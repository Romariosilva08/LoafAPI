using LoafAPI.Domain.Entities;
using LoafAPI.Domain.Interfaces;
using LoafAPI.LoafAPI.Application.DTOs;

namespace LoafAPI.Application.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Usuario>> ListarUsuarios() =>
            await _repository.ObterTodosAsync();

        public async Task<Usuario> ObterUsuario(int id) =>
            await _repository.ObterPorIdAsync(id);

        public async Task<Usuario> CriarUsuario(Usuario usuario)
        {
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
            await _repository.AdicionarAsync(usuario);
            return usuario;
        }

        public async Task<Usuario> Login(LoginDTO login)
        {
            var user = await _repository.ObterPorEmailAsync(login.Email.ToLower().Trim());

            if (user == null || !BCrypt.Net.BCrypt.Verify(login.Senha, user.Senha))
                return null;

            return user;
        }
    }
}
