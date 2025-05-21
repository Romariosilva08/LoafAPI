using Microsoft.AspNetCore.Mvc;
using LoafAPI.Application.Services;
using LoafAPI.Domain.Entities;
using LoafAPI.LoafAPI.Application.DTOs;
using LoafAPI.LoafAPI.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using LoafAPI.Application.Mappers;

namespace LoafAPI.API.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly UsuarioService _service;
    private readonly TokenService _tokenService;

    public UsuariosController(UsuarioService service, TokenService tokenService)
    {
        _service = service;
        _tokenService = tokenService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioDTO>>> Get()
    {
        var usuarios = await _service.ListarUsuarios();
        var usuariosDTO = usuarios.Select(u => u.ToDTO());
        return Ok(usuariosDTO);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioDTO>> Get(int id)
    {
        var usuario = await _service.ObterUsuario(id);
        if (usuario == null) return NotFound();

        return usuario.ToDTO();
    }

    [HttpPost]
    public async Task<ActionResult<UsuarioDTO>> Post(Usuario usuario)
    {
        var criado = await _service.CriarUsuario(usuario);
        return CreatedAtAction(nameof(Get), new { id = criado.Id }, criado.ToDTO());
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO login)
    {
        var usuario = await _service.Login(login);
        if (usuario == null) return Unauthorized("Credenciais inválidas.");

        var token = _tokenService.GerarToken(usuario);
        return Ok(new { Token = token, Nome = usuario.Nome });
    }
}
