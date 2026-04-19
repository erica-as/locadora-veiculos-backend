using Locadora.Api.Domain.Entities;
using Locadora.Api.Domain.Exceptions;
using Locadora.Api.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.Api.Controllers;

/// <summary>
/// Controller para gerenciar operações CRUD de Aluguéis e aplicar filtros
/// Implementa 5 filtros diferentes com JOINs variados conforme requisito 2.5
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AluguelController : ControllerBase
{
    private readonly IAluguelService _service;

    public AluguelController(IAluguelService service) => _service = service;

    /// <summary>
    /// Obtém todos os aluguéis cadastrados
    /// </summary>
    /// <returns>Lista de aluguéis</returns>
    /// <response code="200">Retorna a lista de aluguéis</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Aluguel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var alugueis = await _service.ObterTodosAsync();
        return Ok(alugueis);
    }

    /// <summary>
    /// Obtém um aluguel específico pelo ID
    /// </summary>
    /// <param name="id">ID do aluguel</param>
    /// <returns>Dados do aluguel</returns>
    /// <response code="200">Retorna o aluguel encontrado</response>
    /// <response code="404">Aluguel não encontrado</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Aluguel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var aluguel = await _service.ObterPorIdAsync(id);
            return Ok(aluguel);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { mensagem = "Aluguel não encontrado" });
        }
    }

    /// <summary>
    /// Cria um novo aluguel
    /// </summary>
    /// <param name="aluguel">Dados do aluguel a ser criado</param>
    /// <returns>Aluguel criado</returns>
    /// <response code="201">Aluguel criado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost]
    [ProducesResponseType(typeof(Aluguel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Atualiza um aluguel existente
    /// </summary>
    /// <param name="id">ID do aluguel</param>
    /// <param name="aluguel">Dados atualizados do aluguel</param>
    /// <returns>Sem conteúdo</returns>
    /// <response code="204">Aluguel atualizado com sucesso</response>
    /// <response code="400">Dados inválidos ou ID não corresponde</response>
    /// <response code="404">Aluguel não encontrado</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        catch (KeyNotFoundException)
        {
            return NotFound(new { mensagem = "Aluguel não encontrado" });
        }
    }

    /// <summary>
    /// Deleta um aluguel existente
    /// </summary>
    /// <param name="id">ID do aluguel a ser deletado</param>
    /// <returns>Sem conteúdo</returns>
    /// <response code="204">Aluguel deletado com sucesso</response>
    /// <response code="404">Aluguel não encontrado</response>
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
            return NotFound(new { mensagem = "Aluguel não encontrado" });
        }
    }
    
    /// <summary>
    /// Obtém aluguéis ativos (não devolvidos)
    /// Demonstra INNER JOIN entre Aluguel e Cliente
    /// Retorna todos os aluguéis onde DataDevolucao é nula
    /// </summary>
    /// <returns>Lista de aluguéis ativos</returns>
    /// <response code="200">Retorna os aluguéis ativos</response>
    [HttpGet("ativo/filtrar")]
    [ProducesResponseType(typeof(IEnumerable<Aluguel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterAlugueisAtivos()
    {
        var alugueisAtivos = await _service.ObterAlugueisAtivosAsync();
        return Ok(alugueisAtivos);
    }
    
    /// <summary>
    /// Obtém aluguéis por cliente
    /// Demonstra INNER JOIN múltiplo entre Aluguel, Cliente, Veiculo, Fabricante e Categoria
    /// Retorna todos os aluguéis de um cliente específico
    /// </summary>
    /// <param name="clienteId">ID do cliente para filtrar</param>
    /// <returns>Lista de aluguéis do cliente</returns>
    /// <response code="200">Retorna os aluguéis do cliente</response>
    /// <response code="400">ClienteId inválido</response>
    [HttpGet("cliente/{clienteId:int}")]
    [ProducesResponseType(typeof(IEnumerable<Aluguel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ObterAluguelsPorCliente(int clienteId)
    {
        try
        {
            var alugueis = await _service.ObterAluguelsPorClienteAsync(clienteId);
            return Ok(alugueis);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }
    
    /// <summary>
    /// Obtém aluguéis por período
    /// Demonstra filtro de intervalo de datas com validação
    /// Retorna aluguéis que ocorrem dentro do período especificado
    /// Parâmetros via QueryString: ?dataInicio=2025-01-01&dataFim=2025-12-31
    /// </summary>
    /// <param name="dataInicio">Data inicial do período</param>
    /// <param name="dataFim">Data final do período</param>
    /// <returns>Lista de aluguéis no período</returns>
    /// <response code="200">Retorna os aluguéis no período</response>
    /// <response code="400">Datas inválidas</response>
    [HttpGet("periodo")]
    [ProducesResponseType(typeof(IEnumerable<Aluguel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    
    /// <summary>
    /// Obtém aluguéis com histórico de devolução (devolvidos)
    /// Demonstra verificação de Nullable (LEFT JOIN implícito)
    /// Retorna aluguéis onde DataDevolucao e KmFinal não são nulas
    /// </summary>
    /// <returns>Lista de aluguéis devolvidos</returns>
    /// <response code="200">Retorna os aluguéis devolvidos</response>
    [HttpGet("devolvidos/filtrar")]
    [ProducesResponseType(typeof(IEnumerable<Aluguel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterAlugueisDevolvidos()
    {
        var alugueisDevolvidos = await _service.ObterAlugueisDevolvosAsync();
        return Ok(alugueisDevolvidos);
    }
    
    /// <summary>
    /// Obtém aluguéis por veículo com detalhes completos
    /// Demonstra INNER JOIN múltiplo entre Aluguel, Veiculo, Categoria e Fabricante
    /// Retorna todo o histórico de aluguéis de um veículo específico
    /// </summary>
    /// <param name="veiculoId">ID do veículo para filtrar</param>
    /// <returns>Lista de aluguéis do veículo</returns>
    /// <response code="200">Retorna os aluguéis do veículo</response>
    /// <response code="400">VeiculoId inválido</response>
    [HttpGet("veiculo/{veiculoId:int}")]
    [ProducesResponseType(typeof(IEnumerable<Aluguel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ObterAluguelsPorVeiculo(int veiculoId)
    {
        try
        {
            var alugueis = await _service.ObterAluguelsPorVeiculoAsync(veiculoId);
            return Ok(alugueis);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }
}
