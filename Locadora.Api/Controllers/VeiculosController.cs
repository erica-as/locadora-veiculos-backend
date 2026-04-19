using Locadora.Api.Domain.Entities;
using Locadora.Api.Domain.Exceptions;
using Locadora.Api.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.Api.Controllers;

/// <summary>
/// Controller para gerenciar operações CRUD de Veículos
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class VeiculosController : ControllerBase
{
    private readonly IVeiculoService _service;

    public VeiculosController(IVeiculoService service) => _service = service;

    /// <summary>
    /// Obtém todos os veículos cadastrados
    /// </summary>
    /// <returns>Lista de veículos</returns>
    /// <response code="200">Retorna a lista de veículos</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Veiculo>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var veiculos = await _service.ObterTodosAsync();
        return Ok(veiculos);
    }

    /// <summary>
    /// Obtém um veículo específico pelo ID
    /// </summary>
    /// <param name="id">ID do veículo</param>
    /// <returns>Dados do veículo</returns>
    /// <response code="200">Retorna o veículo encontrado</response>
    /// <response code="404">Veículo não encontrado</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Veiculo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var veiculo = await _service.ObterPorIdAsync(id);
            return Ok(veiculo);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { mensagem = "Veículo não encontrado" });
        }
    }

    /// <summary>
    /// Cria um novo veículo
    /// </summary>
    /// <param name="veiculo">Dados do veículo a ser criado</param>
    /// <returns>Veículo criado</returns>
    /// <response code="201">Veículo criado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost]
    [ProducesResponseType(typeof(Veiculo), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] Veiculo veiculo)
    {
        try
        {
            await _service.AdicionarAsync(veiculo);
            return CreatedAtAction(nameof(GetById), new { id = veiculo.Id }, veiculo);
        }
        catch (ValidacaoException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza um veículo existente
    /// </summary>
    /// <param name="id">ID do veículo</param>
    /// <param name="veiculo">Dados atualizados do veículo</param>
    /// <returns>Sem conteúdo</returns>
    /// <response code="204">Veículo atualizado com sucesso</response>
    /// <response code="400">Dados inválidos ou ID não corresponde</response>
    /// <response code="404">Veículo não encontrado</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(int id, [FromBody] Veiculo veiculo)
    {
        if (id != veiculo.Id)
            return BadRequest(new { mensagem = "O id da rota deve ser igual ao id do corpo." });

        try
        {
            await _service.ObterPorIdAsync(id);
            await _service.AtualizarAsync(veiculo);
            return NoContent();
        }
        catch (ValidacaoException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { mensagem = "Veículo não encontrado" });
        }
    }

    /// <summary>
    /// Deleta um veículo existente
    /// </summary>
    /// <param name="id">ID do veículo a ser deletado</param>
    /// <returns>Sem conteúdo</returns>
    /// <response code="204">Veículo deletado com sucesso</response>
    /// <response code="404">Veículo não encontrado</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.RemoverAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { mensagem = "Veículo não encontrado" });
        }
    }
}