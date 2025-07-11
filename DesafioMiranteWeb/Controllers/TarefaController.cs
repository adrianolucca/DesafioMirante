using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DesafioMirante.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaService _service;

        public TarefaController(ITarefaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tarefas = await _service.ObterTodasAsync();
            return Ok(tarefas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var tarefa = await _service.ObterPorIdAsync(id);
            if (tarefa == null)
                return NotFound();

            return Ok(tarefa);
        }

        [HttpGet("filtrar")]
        public async Task<IActionResult> Filtrar([FromQuery] int? statusId, [FromQuery] DateTime? dataVencimento)
        {
            var tarefas = await _service.FiltrarAsync(statusId, dataVencimento);
            return Ok(tarefas);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TarefaDto dto)
        {
            await _service.AdicionarAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TarefaDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID informado não confere com o corpo da requisição.");

            await _service.AtualizarAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.RemoverAsync(id);
            return NoContent();
        }
    }
}
