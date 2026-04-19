using Locadora.Api.Domain.Entities;

namespace Locadora.Api.Service.Interfaces;

public interface IAluguelService
{
    Task<IEnumerable<Aluguel>> ObterTodosAsync();
    Task<Aluguel> ObterPorIdAsync(int id);
    Task AdicionarAsync(Aluguel aluguel);
    Task AtualizarAsync(Aluguel aluguel);
    Task RemoverAsync(int id);
    
    // Filtros - Requisito 2.5
    Task<IEnumerable<Aluguel>> ObterAlugueisAtivosAsync();
    Task<IEnumerable<Aluguel>> ObterAluguelsPorClienteAsync(int clienteId);
    Task<IEnumerable<Aluguel>> ObterAluguelsPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<IEnumerable<Aluguel>> ObterAlugueisDevolvosAsync();
    Task<IEnumerable<Aluguel>> ObterAluguelsPorVeiculoAsync(int veiculoId);
}

