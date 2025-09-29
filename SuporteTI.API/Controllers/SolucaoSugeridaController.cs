using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuporteTI.API.DTOs;
using SuporteTI.Data.Models;

namespace SuporteTI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolucaoSugeridaController : ControllerBase
    {
        private readonly SuporteTiDbContext _context;

        public SolucaoSugeridaController(SuporteTiDbContext context)
        {
            _context = context;
        }

        // 🔹 GET: api/SolucaoSugerida/{chamadoId}
        [HttpGet("{chamadoId}")]
        public async Task<ActionResult<IEnumerable<SolucaoSugeridaReadDto>>> Listar(int chamadoId)
        {
            var chamado = await _context.Chamados.FindAsync(chamadoId);
            if (chamado == null)
                return NotFound("Chamado não encontrado.");

            var solucoes = await _context.SolucaoSugerida
                .Where(s => s.IdChamado == chamadoId)
                .ToListAsync();

            if (!solucoes.Any())
                return NotFound("Nenhuma solução sugerida encontrada para este chamado.");

            var solucoesDto = solucoes.Select(s => new SolucaoSugeridaReadDto
            {
                IdSolucao = s.IdSolucao,
                IdChamado = s.IdChamado,
                Titulo = s.Titulo,
                Conteudo = s.Conteudo,
                Aceita = s.Aceita ?? false
            }).ToList();

            return Ok(solucoesDto);
        }

        // 🔹 POST: api/SolucaoSugerida
        [HttpPost]
        public async Task<ActionResult<SolucaoSugeridaReadDto>> Criar([FromBody] SolucaoSugeridaCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var chamado = await _context.Chamados.FindAsync(dto.IdChamado);
            if (chamado == null)
                return NotFound("Chamado não encontrado.");

            var solucao = new SolucaoSugeridum
            {
                IdChamado = dto.IdChamado,
                Titulo = dto.Titulo,
                Conteudo = dto.Conteudo,
                Aceita = null, // ainda não aceita nem rejeitada
                DataCriacao = DateTime.Now
            };

            _context.SolucaoSugerida.Add(solucao);
            await _context.SaveChangesAsync();

            var solucaoDto = new SolucaoSugeridaReadDto
            {
                IdSolucao = solucao.IdSolucao,
                IdChamado = solucao.IdChamado,
                Titulo = solucao.Titulo,
                Conteudo = solucao.Conteudo,
                Aceita = solucao.Aceita ?? false
            };

            return CreatedAtAction(nameof(Listar), new { chamadoId = dto.IdChamado }, solucaoDto);
        }

        // 🔹 PUT: api/SolucaoSugerida/aceitar/{id}
        [HttpPut("aceitar/{id}")]
        public async Task<ActionResult<SolucaoSugeridaReadDto>> Aceitar(int id)
        {
            var solucao = await _context.SolucaoSugerida.FindAsync(id);
            if (solucao == null)
                return NotFound("Solução não encontrada.");

            solucao.Aceita = true;
            await _context.SaveChangesAsync();

            var dto = new SolucaoSugeridaReadDto
            {
                IdSolucao = solucao.IdSolucao,
                IdChamado = solucao.IdChamado,
                Titulo = solucao.Titulo,
                Conteudo = solucao.Conteudo,
                Aceita = solucao.Aceita ?? false
            };

            return Ok(new { mensagem = "Solução aceita com sucesso.", solucao = dto });
        }

        // 🔹 PUT: api/SolucaoSugerida/rejeitar/{id}
        [HttpPut("rejeitar/{id}")]
        public async Task<ActionResult<SolucaoSugeridaReadDto>> Rejeitar(int id)
        {
            var solucao = await _context.SolucaoSugerida.FindAsync(id);
            if (solucao == null)
                return NotFound("Solução não encontrada.");

            solucao.Aceita = false;
            await _context.SaveChangesAsync();

            var dto = new SolucaoSugeridaReadDto
            {
                IdSolucao = solucao.IdSolucao,
                IdChamado = solucao.IdChamado,
                Titulo = solucao.Titulo,
                Conteudo = solucao.Conteudo,
                Aceita = solucao.Aceita ?? false
            };

            return Ok(new { mensagem = "Solução rejeitada com sucesso.", solucao = dto });
        }
    }
}
