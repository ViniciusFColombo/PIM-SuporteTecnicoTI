using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuporteTI.Data.Models;
using SuporteTI.API.DTOs;

namespace SuporteTI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvaliacaoController : ControllerBase
    {
        private readonly SuporteTiDbContext _context;

        public AvaliacaoController(SuporteTiDbContext context)
        {
            _context = context;
        }

        // 🔹 POST: api/Avaliacao
        [HttpPost]
        public async Task<ActionResult<AvaliacaoReadDto>> PostAvaliacao([FromBody] AvaliacaoCreateDto dto)
        {
            if (dto == null)
                return BadRequest("Dados inválidos.");

            var chamado = await _context.Chamados
                .Include(c => c.Avaliacaos)
                .FirstOrDefaultAsync(c => c.IdChamado == dto.IdChamado);

            if (chamado == null)
                return NotFound("Chamado não encontrado.");

            // Valida se o chamado pode ser avaliado
            var status = chamado.StatusChamado.ToLower();
            if (status != "fechado" && status != "resolvido")
                return BadRequest("O chamado precisa estar fechado ou resolvido antes de avaliar.");

            // Valida nota
            if (dto.Nota < 1 || dto.Nota > 5)
                return BadRequest("A nota deve ser entre 1 e 5.");

            // Impede múltiplas avaliações no mesmo chamado
            var avaliacaoExistente = await _context.Avaliacoes
                .AnyAsync(a => a.IdChamado == dto.IdChamado);
            if (avaliacaoExistente)
                return Conflict("Este chamado já possui uma avaliação.");

            // Cria entidade
            var avaliacao = new Avaliacao
            {
                IdChamado = dto.IdChamado,
                Nota = dto.Nota,
                Comentario = dto.Comentario,
            };

            _context.Avaliacoes.Add(avaliacao);
            await _context.SaveChangesAsync();

            // Retorna DTO
            var readDto = new AvaliacaoReadDto
            {
                IdAvaliacao = avaliacao.IdAvaliacao,
                IdChamado = avaliacao.IdChamado,
                Nota = avaliacao.Nota,
                Comentario = avaliacao.Comentario,
            };

            return CreatedAtAction(nameof(GetAvaliacaoPorChamado), new { chamadoId = dto.IdChamado }, readDto);
        }

        // 🔹 GET: api/Avaliacao/{chamadoId}
        [HttpGet("{chamadoId}")]
        public async Task<ActionResult<AvaliacaoReadDto>> GetAvaliacaoPorChamado(int chamadoId)
        {
            var avaliacao = await _context.Avaliacoes
                .FirstOrDefaultAsync(a => a.IdChamado == chamadoId);

            if (avaliacao == null)
                return NotFound("Nenhuma avaliação encontrada para este chamado.");

            var dto = new AvaliacaoReadDto
            {
                IdAvaliacao = avaliacao.IdAvaliacao,
                IdChamado = avaliacao.IdChamado,
                Nota = avaliacao.Nota,
                Comentario = avaliacao.Comentario,
            };

            return Ok(dto);
        }
    }
}
