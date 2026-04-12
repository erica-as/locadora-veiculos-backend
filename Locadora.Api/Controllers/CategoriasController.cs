using Locadora.Api.Domain.Entities;
using Locadora.Api.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _service;

    public CategoriasController(ICategoriaService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var categorias = await _service.ObterTodosAsync();
        return Ok(categorias);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var categoria = await _service.ObterPorIdAsync(id);
        return Ok(categoria);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Categoria categoria)
    {
        ValidarCategoria(categoria);
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        await _service.AdicionarAsync(categoria);
        return CreatedAtAction(nameof(GetById), new { id = categoria.Id }, categoria);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Categoria categoria)
    {
        if (id != categoria.Id)
            return BadRequest(new { mensagem = "O id da rota deve ser igual ao id do corpo." });

        ValidarCategoria(categoria);
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        await _service.ObterPorIdAsync(id);
        await _service.AtualizarAsync(categoria);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.RemoverAsync(id);
        return NoContent();
    }

    private void ValidarCategoria(Categoria categoria)
    {
        if (string.IsNullOrWhiteSpace(categoria.Nome))
            ModelState.AddModelError(nameof(categoria.Nome), "Nome da categoria e obrigatorio.");

        if (categoria.ValorBaseDiaria < 0)
            ModelState.AddModelError(nameof(categoria.ValorBaseDiaria), "ValorBaseDiaria nao pode ser negativo.");
    }
}

