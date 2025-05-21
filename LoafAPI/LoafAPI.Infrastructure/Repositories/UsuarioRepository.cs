using Microsoft.EntityFrameworkCore;
using LoafAPI.Domain.Entities;
using LoafAPI.Domain.Interfaces;
using LoafAPI.LoafAPI.Infrastructure.Data;


namespace LoafAPI.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly MeuDbContext _context;

    public UsuarioRepository(MeuDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Usuario>> ObterTodosAsync() => await _context.Usuarios.ToListAsync();

    public async Task<Usuario> ObterPorIdAsync(int id) => await _context.Usuarios.FindAsync(id);

    public async Task<Usuario> ObterPorEmailAsync(string email) =>
        await _context.Usuarios.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

    public async Task AdicionarAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
    }
}
