using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuporteTI.Data.Models;
using SuporteTI.API.DTOs;

namespace SuporteTI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChamadoController : ControllerBase
    {
        private readonly SuporteTiDbContext _context;

        public ChamadoController(SuporteTiDbContext context)
        {
            _context = context;
        }

        // 🔹 GET: api/Chamado
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChamadoReadDto>>> GetChamados()
        {
            var chamados = await _context.Chamados
                .Include(c => c.IdUsuarioNavigation)
                .Include(c => c.IdCategoria)
                .ToListAsync();

            // Mapeamento manual para DTO de leitura
            var chamadosDto = chamados.Select(c => new ChamadoReadDto
            {
                IdChamado = c.IdChamado,
                Titulo = c.Titulo,
                Descricao = c.Descricao,
                Prioridade = c.Prioridade,
                StatusChamado = c.StatusChamado,
                DataAbertura = (DateTime)c.DataAbertura,
                DataFechamento = c.DataFechamento,
                Usuario = c.IdUsuarioNavigation == null ? null : new UsuarioReadDto
                {
                    IdUsuario = c.IdUsuarioNavigation.IdUsuario,
                    Nome = c.IdUsuarioNavigation.Nome,
                    Email = c.IdUsuarioNavigation.Email,
                    Tipo = c.IdUsuarioNavigation.Tipo,
                    Ativo = c.IdUsuarioNavigation.Ativo ?? false,
                    Cpf = c.IdUsuarioNavigation.Cpf,
                    Telefone = c.IdUsuarioNavigation.Telefone
                },
                Categorias = c.IdCategoria?.Select(cat => new CategoriaReadDto
                {
                    IdCategoria = cat.IdCategoria,
                    Nome = cat.Nome
                }).ToList()
            }).ToList();

            return Ok(chamadosDto);
        }

        // 🔹 GET: api/Chamado/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ChamadoReadDto>> GetChamado(int id)
        {
            var chamado = await _context.Chamados
                .Include(c => c.IdUsuarioNavigation)
                .Include(c => c.IdCategoria)
                .FirstOrDefaultAsync(c => c.IdChamado == id);

            if (chamado == null)
                return NotFound();

            var dto = new ChamadoReadDto
            {
                IdChamado = chamado.IdChamado,
                Titulo = chamado.Titulo,
                Descricao = chamado.Descricao,
                Prioridade = chamado.Prioridade,
                StatusChamado = chamado.StatusChamado,
                DataAbertura = (DateTime)chamado.DataAbertura,
                DataFechamento = chamado.DataFechamento,
                Usuario = chamado.IdUsuarioNavigation == null ? null : new UsuarioReadDto
                {
                    IdUsuario = chamado.IdUsuarioNavigation.IdUsuario,
                    Nome = chamado.IdUsuarioNavigation.Nome,
                    Email = chamado.IdUsuarioNavigation.Email,
                    Tipo = chamado.IdUsuarioNavigation.Tipo,
                    Ativo = chamado.IdUsuarioNavigation.Ativo ?? false,
                    Cpf = chamado.IdUsuarioNavigation.Cpf,
                    Telefone = chamado.IdUsuarioNavigation.Telefone
                },
                Categorias = chamado.IdCategoria?.Select(cat => new CategoriaReadDto
                {
                    IdCategoria = cat.IdCategoria,
                    Nome = cat.Nome
                }).ToList()
            };

            return Ok(dto);
        }

        // 🔹 POST: api/Chamado
        [HttpPost]
        public async Task<ActionResult<ChamadoReadDto>> PostChamado([FromBody] ChamadoCreateDto dto)
        {
            if (dto == null)
                return BadRequest("Chamado inválido.");

            var usuario = await _context.Usuarios.FindAsync(dto.IdUsuario);
            if (usuario == null)
                return BadRequest("Usuário informado não existe.");

            var chamado = new Chamado
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Prioridade = dto.Prioridade,
                StatusChamado = "Aberto",
                DataAbertura = DateTime.Now,
                IdUsuario = dto.IdUsuario
            };

            // 🔸 Adiciona categorias
            if (dto.IdCategorias != null && dto.IdCategorias.Count > 0)
            {
                chamado.IdCategoria = new List<Categorium>();
                foreach (var catId in dto.IdCategorias)
                {
                    var categoria = await _context.Categoria.FindAsync(catId);
                    if (categoria != null)
                        chamado.IdCategoria.Add(categoria);
                }
            }

            _context.Chamados.Add(chamado);
            await _context.SaveChangesAsync();

            // Retorna DTO de leitura
            var readDto = new ChamadoReadDto
            {
                IdChamado = chamado.IdChamado,
                Titulo = chamado.Titulo,
                Descricao = chamado.Descricao,
                Prioridade = chamado.Prioridade,
                StatusChamado = chamado.StatusChamado,
                DataAbertura = (DateTime)chamado.DataAbertura,
                DataFechamento = chamado.DataFechamento,
                Usuario = new UsuarioReadDto
                {
                    IdUsuario = usuario.IdUsuario,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Tipo = usuario.Tipo,
                    Ativo = usuario.Ativo ?? false,
                    Cpf = usuario.Cpf,
                    Telefone = usuario.Telefone
                },
                Categorias = chamado.IdCategoria?.Select(cat => new CategoriaReadDto
                {
                    IdCategoria = cat.IdCategoria,
                    Nome = cat.Nome
                }).ToList()
            };

            return CreatedAtAction(nameof(GetChamado), new { id = chamado.IdChamado }, readDto);
        }

        // 🔹 PUT: api/Chamado/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChamado(int id, [FromBody] ChamadoUpdateDto dto)
        {
            if (dto == null)
                return BadRequest("Dados inválidos.");

            var chamado = await _context.Chamados
                .Include(c => c.IdCategoria)
                .FirstOrDefaultAsync(c => c.IdChamado == id);

            if (chamado == null)
                return NotFound();

            // Atualiza campos permitidos
            chamado.Titulo = dto.Titulo;
            chamado.Descricao = dto.Descricao;
            chamado.Prioridade = dto.Prioridade;
            chamado.StatusChamado = dto.StatusChamado;

            // Define DataFechamento se status mudou para Fechado
            if (dto.StatusChamado == "Fechado")
                chamado.DataFechamento = DateTime.Now;
            else
                chamado.DataFechamento = null;

            // Atualiza categorias (se enviadas)
            if (dto.IdCategorias != null)
            {
                chamado.IdCategoria.Clear();
                foreach (var catId in dto.IdCategorias)
                {
                    var categoria = await _context.Categoria.FindAsync(catId);
                    if (categoria != null)
                        chamado.IdCategoria.Add(categoria);
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // 🔹 DELETE: api/Chamado/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChamado(int id)
        {
            var chamado = await _context.Chamados.FindAsync(id);
            if (chamado == null)
                return NotFound();

            _context.Chamados.Remove(chamado);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
