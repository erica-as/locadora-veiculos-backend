using Locadora.Api.Domain.Entities;
using Locadora.Api.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VeiculosController : ControllerBase
{
    private readonly IVeiculoService _service;

    public VeiculosController(IVeiculoService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var veiculos = await _service.ObterTodosAsync();
        return Ok(veiculos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var veiculo = await _service.ObterPorIdAsync(id);
        return Ok(veiculo);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Veiculo veiculo)
    {
        ValidarVeiculo(veiculo);
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        await _service.AdicionarAsync(veiculo);
        return CreatedAtAction(nameof(GetById), new { id = veiculo.Id }, veiculo);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Veiculo veiculo)
    {
        if (id != veiculo.Id)
            return BadRequest(new { mensagem = "O id da rota deve ser igual ao id do corpo." });

        ValidarVeiculo(veiculo);
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        await _service.ObterPorIdAsync(id);
        await _service.AtualizarAsync(veiculo);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.RemoverAsync(id);
        return NoContent();
    }

    private void ValidarVeiculo(Veiculo veiculo)
    {
        if (veiculo.AnoFabricacao < 1950 || veiculo.AnoFabricacao > DateTime.UtcNow.Year + 1)
            ModelState.AddModelError(nameof(veiculo.AnoFabricacao), "Ano de fabricacao invalido.");

        if (veiculo.Quilometragem < 0)
            ModelState.AddModelError(nameof(veiculo.Quilometragem), "Quilometragem nao pode ser negativa.");

        if (veiculo.FabricanteId <= 0)
            ModelState.AddModelError(nameof(veiculo.FabricanteId), "FabricanteId deve ser maior que zero.");

        if (veiculo.CategoriaId <= 0)
            ModelState.AddModelError(nameof(veiculo.CategoriaId), "CategoriaId deve ser maior que zero.");
    }
}