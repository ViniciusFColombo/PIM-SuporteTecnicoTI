using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuporteTI.Data.Models;
using SuporteTI.API.DTOs;

namespace SuporteTI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnexoController : ControllerBase
    {
        private readonly SuporteTiDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AnexoController(SuporteTiDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // 🔹 POST: api/Anexo/{chamadoId}
        [HttpPost("{chamadoId}")]
        public async Task<ActionResult<AnexoReadDto>> Upload(int chamadoId, IFormFile arquivo)
        {
            // 🔸 Validações iniciais
            var chamado = await _context.Chamados.FindAsync(chamadoId);
            if (chamado == null)
                return NotFound("Chamado não encontrado.");

            if (arquivo == null || arquivo.Length == 0)
                return BadRequest("Nenhum arquivo enviado.");

            // 🔸 Cria pasta uploads, se não existir
            var pastaUploads = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(pastaUploads))
                Directory.CreateDirectory(pastaUploads);

            // 🔸 Gera nome único para evitar sobrescrita
            var nomeArquivoUnico = $"{Guid.NewGuid()}_{arquivo.FileName}";
            var caminhoCompleto = Path.Combine(pastaUploads, nomeArquivoUnico);

            // 🔸 Salva o arquivo fisicamente
            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            // 🔸 Cria entidade de anexo
            var anexo = new Anexo
            {
                IdChamado = chamadoId,
                NomeArquivo = arquivo.FileName,
                CaminhoArquivo = $"/uploads/{nomeArquivoUnico}",
                DataEnvio = DateTime.Now
            };

            _context.Anexos.Add(anexo);
            await _context.SaveChangesAsync();

            // 🔸 Mapeia para DTO
            var dto = new AnexoReadDto
            {
                IdAnexo = anexo.IdAnexo,
                NomeArquivo = anexo.NomeArquivo,
                CaminhoArquivo = anexo.CaminhoArquivo,
                DataEnvio = anexo.DataEnvio
            };

            return CreatedAtAction(nameof(GetAnexosPorChamado), new { chamadoId }, dto);
        }

        // 🔹 GET: api/Anexo/{chamadoId}
        [HttpGet("{chamadoId}")]
        public async Task<ActionResult<IEnumerable<AnexoReadDto>>> GetAnexosPorChamado(int chamadoId)
        {
            var chamado = await _context.Chamados.FindAsync(chamadoId);
            if (chamado == null)
                return NotFound("Chamado não encontrado.");

            var anexos = await _context.Anexos
                .Where(a => a.IdChamado == chamadoId)
                .OrderByDescending(a => a.DataEnvio)
                .ToListAsync();

            if (!anexos.Any())
                return NotFound("Nenhum anexo encontrado para este chamado.");

            var anexosDto = anexos.Select(a => new AnexoReadDto
            {
                IdAnexo = a.IdAnexo,
                NomeArquivo = a.NomeArquivo,
                CaminhoArquivo = a.CaminhoArquivo,
                DataEnvio = a.DataEnvio
            }).ToList();

            return Ok(anexosDto);
        }

        // 🔹 DELETE: api/Anexo/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnexo(int id)
        {
            var anexo = await _context.Anexos.FindAsync(id);
            if (anexo == null)
                return NotFound("Anexo não encontrado.");

            // 🔸 Deleta o arquivo físico se existir
            var caminhoCompleto = Path.Combine(_env.WebRootPath, anexo.CaminhoArquivo.TrimStart('/'));
            if (System.IO.File.Exists(caminhoCompleto))
                System.IO.File.Delete(caminhoCompleto);

            // 🔸 Remove do banco
            _context.Anexos.Remove(anexo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
