using Locadora.Api.Domain.Entities;
using Locadora.Api.Domain.Exceptions;
using Locadora.Api.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AluguelController : ControllerBase
{
    private readonly IAluguelService _service;

    public AluguelController(IAluguelService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var alugueis = await _service.ObterTodosAsync();
        return Ok(alugueis);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var aluguel = await _service.ObterPorIdAsync(id);
        return Ok(aluguel);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Aluguel aluguel)
    {
        try
        {
            await _service.AdicionarAsync(aluguel);
            return CreatedAtAction(nameof(GetById), new { id = aluguel.Id }, aluguel);
        }
        catch (ValidacaoException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Aluguel aluguel)
    {
        if (id != aluguel.Id)
            return BadRequest(new { mensagem = "O id da rota deve ser igual ao id do corpo." });

        try
        {
            await _service.ObterPorIdAsync(id);
            await _service.AtualizarAsync(aluguel);
            return NoContent();
        }
        catch (ValidacaoException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.RemoverAsync(id);
        return NoContent();
    }
    
    [HttpGet("ativo/filtrar")]
    public async Task<IActionResult> ObterAlugueisAtivos()
    {
        var alugueisAtivos = await _service.ObterAlugueisAtivosAsync();
        return Ok(alugueisAtivos);
    }
    
    [HttpGet("cliente/{clienteId:int}")]
    public async Task<IActionResult> ObterAluguelsPorCliente(int clienteId)
    {
        var alugueis = await _service.ObterAluguelsPorClienteAsync(clienteId);
        return Ok(alugueis);
    }

    [HttpGet("periodo")]
    public async Task<IActionResult> ObterAluguelsPorPeriodo(
        [FromQuery] DateTime dataInicio,
        [FromQuery] DateTime dataFim)
    {
        try
        {
            if (dataInicio > dataFim)
                throw new ValidacaoException("Data de início não pode ser maior que data de fim.");

            var alugueis = await _service.ObterAluguelsPorPeriodoAsync(dataInicio, dataFim);
            return Ok(alugueis);
        }
        catch (ValidacaoException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpGet("devolvidos/filtrar")]
    public async Task<IActionResult> ObterAlugueisDevolvidos()
    {
        var alugueisDevolvidos = await _service.ObterAlugueisDevolvosAsync();
        return Ok(alugueisDevolvidos);
    }
    
    [HttpGet("veiculo/{veiculoId:int}")]
    public async Task<IActionResult> ObterAluguelsPorVeiculo(int veiculoId)
    {
        var alugueis = await _service.ObterAluguelsPorVeiculoAsync(veiculoId);
        return Ok(alugueis);
    }
}

