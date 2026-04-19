using Locadora.Api.Domain.Entities;
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
        ValidarAluguel(aluguel);
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        await _service.AdicionarAsync(aluguel);
        return CreatedAtAction(nameof(GetById), new { id = aluguel.Id }, aluguel);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Aluguel aluguel)
    {
        if (id != aluguel.Id)
            return BadRequest(new { mensagem = "O id da rota deve ser igual ao id do corpo." });

        ValidarAluguel(aluguel);
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        await _service.ObterPorIdAsync(id);
        await _service.AtualizarAsync(aluguel);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.RemoverAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Filtro 1: Obter aluguéis ativos (não devolvidos) - Demonstra INNER JOIN
    /// </summary>
    [HttpGet("ativo/filtrar")]
    public async Task<IActionResult> ObterAlugueisAtivos()
    {
        var alugueisAtivos = await _service.ObterAlugueisAtivosAsync();
        return Ok(alugueisAtivos);
    }

    /// <summary>
    /// Filtro 2: Obter aluguéis por cliente - Demonstra INNER JOIN com tabela Cliente
    /// </summary>
    [HttpGet("cliente/{clienteId:int}")]
    public async Task<IActionResult> ObterAluguelsPorCliente(int clienteId)
    {
        var alugueis = await _service.ObterAluguelsPorClienteAsync(clienteId);
        return Ok(alugueis);
    }

    /// <summary>
    /// Filtro 3: Obter aluguéis por período - Demonstra filtro de data
    /// </summary>
    [HttpGet("periodo")]
    public async Task<IActionResult> ObterAluguelsPorPeriodo(
        [FromQuery] DateTime dataInicio,
        [FromQuery] DateTime dataFim)
    {
        if (dataInicio > dataFim)
            return BadRequest(new { mensagem = "Data de início não pode ser maior que data de fim." });

        var alugueis = await _service.ObterAluguelsPorPeriodoAsync(dataInicio, dataFim);
        return Ok(alugueis);
    }

    /// <summary>
    /// Filtro 4: Obter aluguéis com histórico de devolução - Demonstra LEFT JOIN
    /// </summary>
    [HttpGet("devolvidos/filtrar")]
    public async Task<IActionResult> ObterAlugueisDevolvidos()
    {
        var alugueisDevolvidos = await _service.ObterAlugueisDevolvosAsync();
        return Ok(alugueisDevolvidos);
    }

    /// <summary>
    /// Filtro 5: Obter aluguéis por veículo com detalhes - Demonstra INNER JOIN com Veiculo, Categoria e Fabricante
    /// </summary>
    [HttpGet("veiculo/{veiculoId:int}")]
    public async Task<IActionResult> ObterAluguelsPorVeiculo(int veiculoId)
    {
        var alugueis = await _service.ObterAluguelsPorVeiculoAsync(veiculoId);
        return Ok(alugueis);
    }

    private void ValidarAluguel(Aluguel aluguel)
    {
        if (aluguel.ClienteId <= 0)
            ModelState.AddModelError(nameof(aluguel.ClienteId), "ClienteId deve ser maior que zero.");

        if (aluguel.VeiculoId <= 0)
            ModelState.AddModelError(nameof(aluguel.VeiculoId), "VeiculoId deve ser maior que zero.");

        if (aluguel.DataInicio == default)
            ModelState.AddModelError(nameof(aluguel.DataInicio), "Data de início é obrigatória.");

        if (aluguel.DataFimPrevista == default)
            ModelState.AddModelError(nameof(aluguel.DataFimPrevista), "Data de fim prevista é obrigatória.");

        if (aluguel.DataInicio >= aluguel.DataFimPrevista)
            ModelState.AddModelError(nameof(aluguel.DataInicio), "Data de início deve ser anterior à data de fim.");

        if (aluguel.KmInicial < 0)
            ModelState.AddModelError(nameof(aluguel.KmInicial), "Quilometragem inicial não pode ser negativa.");

        if (aluguel.ValorDiaria <= 0)
            ModelState.AddModelError(nameof(aluguel.ValorDiaria), "Valor da diária deve ser maior que zero.");
    }
}

