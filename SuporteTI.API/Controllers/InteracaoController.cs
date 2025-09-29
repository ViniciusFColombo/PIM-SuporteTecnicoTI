using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuporteTI.Data.Models;
using SuporteTI.API.DTOs;

namespace SuporteTI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InteracaoController : ControllerBase
    {
        private readonly SuporteTiDbContext _context;

        public InteracaoController(SuporteTiDbContext context)
        {
            _context = context;
        }

        // 🔹 POST: api/Interacao
        [HttpPost]
        public async Task<ActionResult<InteracaoReadDto>> PostInteracao([FromBody] InteracaoCreateDto dto)
        {
            if (dto == null)
                return BadRequest("Dados inválidos.");

            // Verifica se o chamado existe
            var chamado = await _context.Chamados.FindAsync(dto.IdChamado);
            if (chamado == null)
                return NotFound($"Chamado com ID {dto.IdChamado} não encontrado.");

            // Verifica se o usuário existe
            var usuario = await _context.Usuarios.FindAsync(dto.IdUsuario);
            if (usuario == null)
                return NotFound($"Usuário com ID {dto.IdUsuario} não encontrado.");

            // Cria nova interação
            var interacao = new Interacao
            {
                IdChamado = dto.IdChamado,
                IdUsuario = dto.IdUsuario,
                Mensagem = dto.Mensagem,
                DataHora = DateTime.Now
            };

            _context.Interacoes.Add(interacao);
            await _context.SaveChangesAsync();

            // Retorna DTO de leitura
            var readDto = new InteracaoReadDto
            {
                IdInteracao = interacao.IdInteracao,
                IdUsuario = interacao.IdUsuario,
                Mensagem = interacao.Mensagem,
                DataHora = (DateTime)interacao.DataHora
            };

            return CreatedAtAction(nameof(GetInteracoesPorChamado), new { chamadoId = dto.IdChamado }, readDto);
        }

        // 🔹 GET: api/Interacao/{chamadoId}
        [HttpGet("{chamadoId}")]
        public async Task<ActionResult<IEnumerable<InteracaoReadDto>>> GetInteracoesPorChamado(int chamadoId)
        {
            var chamado = await _context.Chamados.FindAsync(chamadoId);
            if (chamado == null)
                return NotFound($"Chamado com ID {chamadoId} não encontrado.");

            var interacoes = await _context.Interacoes
                .Where(i => i.IdChamado == chamadoId)
                .OrderBy(i => i.DataHora)
                .ToListAsync();

            if (!interacoes.Any())
                return NotFound($"Nenhuma interação encontrada para o chamado {chamadoId}.");

            var interacoesDto = interacoes.Select(i => new InteracaoReadDto
            {
                IdInteracao = i.IdInteracao,
                IdUsuario = i.IdUsuario,
                Mensagem = i.Mensagem,
                DataHora = (DateTime)i.DataHora
            }).ToList();

            return Ok(interacoesDto);
        }
    }
}
