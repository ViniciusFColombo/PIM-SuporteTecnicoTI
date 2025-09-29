using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuporteTI.API.DTOs;
using SuporteTI.Data.Models;

namespace SuporteTI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatorioController : ControllerBase
    {
        private readonly SuporteTiDbContext _context;

        public RelatorioController(SuporteTiDbContext context)
        {
            _context = context;
        }

        // GET: api/Relatorio/chamados-status?status=aberto
        [HttpGet("chamados-status")]
        public async Task<IActionResult> RelatorioPorStatus([FromQuery] string? status = null)
        {
            // 🔹 Consulta base com includes
            var query = _context.Chamados
                .Include(c => c.IdUsuarioNavigation)
                .Include(c => c.IdTecnicoNavigation)
                .Include(c => c.IdCategoria)
                .AsQueryable();

            // 🔹 Filtro opcional por status
            if (!string.IsNullOrWhiteSpace(status))
            {
                var statusLower = status.ToLower();
                query = query.Where(c => c.StatusChamado.ToLower() == statusLower);
            }

            var chamados = await query.ToListAsync();

            // 🔹 Mapeamento para DTO
            var chamadosDto = chamados.Select(c => new ChamadoReadDto
            {
                IdChamado = c.IdChamado,
                Titulo = c.Titulo,
                Descricao = c.Descricao,
                Prioridade = c.Prioridade ?? "Média",
                StatusChamado = c.StatusChamado ?? "Aberto",
                DataAbertura = (DateTime)c.DataAbertura,
                DataFechamento = c.DataFechamento,
                Usuario = c.IdUsuarioNavigation != null ? new UsuarioReadDto
                {
                    IdUsuario = c.IdUsuarioNavigation.IdUsuario,
                    Nome = c.IdUsuarioNavigation.Nome,
                    Email = c.IdUsuarioNavigation.Email,
                    Tipo = c.IdUsuarioNavigation.Tipo
                } : null,
                Categorias = c.IdCategoria != null
                    ? c.IdCategoria.Select(cat => new CategoriaReadDto
                    {
                        IdCategoria = cat.IdCategoria,
                        Nome = cat.Nome
                    }).ToList()
                    : null
            }).ToList();

            // 🔹 Contagem por status
            var total = chamadosDto.Count;
            var abertos = chamadosDto.Count(c => c.StatusChamado.ToLower() == "aberto");
            var fechados = chamadosDto.Count(c => c.StatusChamado.ToLower() == "fechado");

            return Ok(new
            {
                TotalChamados = total,
                ChamadosAbertos = abertos,
                ChamadosFechados = fechados,
                Chamados = chamadosDto
            });
        }

        // GET: api/Relatorio/chamados-prioridade?prioridade=Alta
        [HttpGet("chamados-prioridade")]
        public async Task<IActionResult> RelatorioPorPrioridade([FromQuery] string? prioridade = null)
        {
            // 🔹 Consulta base com includes
            var query = _context.Chamados
                .Include(c => c.IdUsuarioNavigation)
                .Include(c => c.IdTecnicoNavigation)
                .Include(c => c.IdCategoria)
                .AsQueryable();

            // 🔹 Filtro opcional por prioridade
            if (!string.IsNullOrWhiteSpace(prioridade))
            {
                var prioridadeLower = prioridade.ToLower();
                query = query.Where(c => c.Prioridade.ToLower() == prioridadeLower);
            }

            var chamados = await query.ToListAsync();

            // 🔹 Mapeia os resultados para DTO
            var chamadosDto = chamados.Select(c => new ChamadoReadDto
            {
                IdChamado = c.IdChamado,
                Titulo = c.Titulo,
                Descricao = c.Descricao,
                Prioridade = c.Prioridade ?? "Média",
                StatusChamado = c.StatusChamado ?? "Aberto",
                DataAbertura = (DateTime)c.DataAbertura,
                DataFechamento = c.DataFechamento,
                Usuario = c.IdUsuarioNavigation != null ? new UsuarioReadDto
                {
                    IdUsuario = c.IdUsuarioNavigation.IdUsuario,
                    Nome = c.IdUsuarioNavigation.Nome,
                    Email = c.IdUsuarioNavigation.Email,
                    Tipo = c.IdUsuarioNavigation.Tipo
                } : null,
                Categorias = c.IdCategoria != null
                    ? c.IdCategoria.Select(cat => new CategoriaReadDto
                    {
                        IdCategoria = cat.IdCategoria,
                        Nome = cat.Nome
                    }).ToList()
                    : null
            }).ToList();

            // 🔹 Estatísticas por prioridade
            var total = chamadosDto.Count;
            var alta = chamadosDto.Count(c => c.Prioridade.ToLower() == "alta");
            var media = chamadosDto.Count(c => c.Prioridade.ToLower() == "media");
            var baixa = chamadosDto.Count(c => c.Prioridade.ToLower() == "baixa");

            return Ok(new
            {
                TotalChamados = total,
                AltaPrioridade = alta,
                MediaPrioridade = media,
                BaixaPrioridade = baixa,
                Chamados = chamadosDto
            });
        }
        

        // 🔹 GET: api/Relatorio/avaliacoes
        [HttpGet("avaliacoes")]
        public async Task<IActionResult> Avaliacoes()
        {
            var avaliacoes = await _context.Avaliacoes
                .Include(a => a.IdChamadoNavigation)
                .Select(a => new
                {
                    ChamadoId = a.IdChamado,
                    TituloChamado = a.IdChamadoNavigation.Titulo,
                    Nota = a.Nota,
                    Comentario = a.Comentario
                })
                .ToListAsync();

            return Ok(avaliacoes);
        }

        // 🔹 GET: api/Relatorio/geral
        [HttpGet("geral")]
        public async Task<ActionResult<RelatorioReadDto>> RelatorioGeral()
        {
            var total = await _context.Chamados.CountAsync();
            var abertos = await _context.Chamados.CountAsync(c => c.StatusChamado.ToLower() == "aberto");
            var fechados = await _context.Chamados.CountAsync(c => c.StatusChamado.ToLower() == "fechado");

            var relatorio = new RelatorioReadDto
            {
                TotalChamados = total,
                ChamadosAbertos = abertos,
                ChamadosFechados = fechados
            };

            return Ok(relatorio);
        }
    }
}
