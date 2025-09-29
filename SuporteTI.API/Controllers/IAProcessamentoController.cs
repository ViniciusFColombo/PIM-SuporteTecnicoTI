using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuporteTI.Data.Models;
using SuporteTI.API.DTOs;

namespace SuporteTI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IAProcessamentoController : ControllerBase
    {
        private readonly SuporteTiDbContext _context;

        public IAProcessamentoController(SuporteTiDbContext context)
        {
            _context = context;
        }

        // 🔹 POST: api/IAProcessamento
        [HttpPost]
        public async Task<ActionResult<IAResponseDto>> ProcessarChamado([FromBody] IARequestDto dto)
        {
            if (dto == null || dto.IdChamado <= 0)
                return BadRequest("Dados inválidos. Informe um chamado válido e o texto de entrada.");

            var chamado = await _context.Chamados
                .Include(c => c.IdCategoria)
                .FirstOrDefaultAsync(c => c.IdChamado == dto.IdChamado);

            if (chamado == null)
                return NotFound("Chamado não encontrado.");

            // 🔹 Simulação de IA — no futuro substituir por integração real
            var categoriaSugerida = "Hardware";
            var solucaoSugerida = "Verifique se o cabo de energia está conectado corretamente.";

            // 🔹 Registra o processamento no banco
            var processamento = new Iaprocessamento
            {
                IdChamado = dto.IdChamado,
                EntradaTexto = dto.TextoEntrada ?? chamado.Descricao,
                SaidaClassificacao = categoriaSugerida,
                SolucaoSugerida = solucaoSugerida,
                DataProcessamento = DateTime.Now
            };

            _context.Iaprocessamentos.Add(processamento);
            await _context.SaveChangesAsync();

            // 🔹 Retorna DTO de resposta
            var resposta = new IAResponseDto
            {
                IdChamado = dto.IdChamado,
                Classificacao = categoriaSugerida,
                SolucaoSugerida = solucaoSugerida
            };

            return Ok(resposta);
        }

        // 🔹 GET: api/IAProcessamento/{chamadoId}
        [HttpGet("{chamadoId}")]
        public async Task<ActionResult<IEnumerable<IAResponseDto>>> ObterHistorico(int chamadoId)
        {
            var chamado = await _context.Chamados.FindAsync(chamadoId);
            if (chamado == null)
                return NotFound("Chamado não encontrado.");

            var historico = await _context.Iaprocessamentos
                .Where(p => p.IdChamado == chamadoId)
                .OrderByDescending(p => p.DataProcessamento)
                .ToListAsync();

            if (!historico.Any())
                return NotFound("Nenhum histórico de processamento encontrado para este chamado.");

            var historicoDto = historico.Select(p => new IAResponseDto
            {
                IdChamado = (int)p.IdChamado,
                Classificacao = p.SaidaClassificacao ?? string.Empty,
                SolucaoSugerida = p.SolucaoSugerida ?? string.Empty
            }).ToList();

            return Ok(historicoDto);
        }
    }
}
