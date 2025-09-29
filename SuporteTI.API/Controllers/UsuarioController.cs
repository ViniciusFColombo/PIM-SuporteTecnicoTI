using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuporteTI.Data.Models;
using SuporteTI.API.DTOs;

namespace SuporteTI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly SuporteTiDbContext _context;

        public UsuarioController(SuporteTiDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioReadDto>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();

            // Mapeamento manual para o DTO de leitura
            var usuariosDto = usuarios.Select(static u => new UsuarioReadDto
            {
                IdUsuario = u.IdUsuario,
                Nome = u.Nome,
                Email = u.Email,
                Tipo = u.Tipo,
                Ativo = u.Ativo ?? false,
                Cpf = u.Cpf,
                Telefone = u.Telefone
            }).ToList();

            return Ok(usuariosDto);
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioReadDto>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound();

            return Ok(new UsuarioReadDto
            {
                IdUsuario = usuario.IdUsuario,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Tipo = usuario.Tipo,
                Ativo = usuario.Ativo ?? false,
                Cpf = usuario.Cpf,
                Telefone = usuario.Telefone
            });
        }

        // POST: api/Usuario
        [HttpPost]
        public async Task<ActionResult<UsuarioReadDto>> PostUsuario([FromBody] UsuarioCreateDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Cpf) && !CpfValidator.IsValid(dto.Cpf))
                return BadRequest("CPF inválido.");

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = dto.Senha, // Em produção: aplicar hash!
                Tipo = dto.Tipo,
                Cpf = dto.Cpf,
                Telefone = dto.Telefone,
                Endereco = dto.Endereco,
                DataNascimento = dto.DataNascimento, // DateTime? → DateTime? (sem conversão)
                Ativo = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.IdUsuario }, new UsuarioReadDto
            {
                IdUsuario = usuario.IdUsuario,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Tipo = usuario.Tipo,
                Ativo = (bool)usuario.Ativo,
                Cpf = usuario.Cpf,
                Telefone = usuario.Telefone
            });
        }

        // PUT: api/Usuario/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, [FromBody] UsuarioUpdateDto dto)
        {
            if (id != dto.IdUsuario)
                return BadRequest("O ID informado não confere.");

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            if (!string.IsNullOrWhiteSpace(dto.Cpf) && !CpfValidator.IsValid(dto.Cpf))
                return BadRequest("CPF inválido.");

            // Atualiza somente os campos que foram enviados
            usuario.Nome = dto.Nome;
            if (!string.IsNullOrEmpty(dto.Email)) usuario.Email = dto.Email;
            usuario.Cpf = dto.Cpf ?? usuario.Cpf;
            usuario.Telefone = dto.Telefone ?? usuario.Telefone;

            if (dto.Ativo.HasValue)
                usuario.Ativo = dto.Ativo.Value;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Usuario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    // 🔹 Classe validadora de CPF
    public static class CpfValidator
    {
        public static bool IsValid(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            cpf = cpf.Replace(".", "").Replace("-", "").Trim();

            if (cpf.Length != 11 || cpf.All(c => c == cpf[0]))
                return false;

            int[] mult1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mult2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * mult1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            string digito = resto.ToString();
            tempCpf += digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * mult2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }
    }
}
