using Locadora.Api.Domain.Entities;
namespace Locadora.Api.Domain.Interfaces;

public interface IAluguelRepository : IRepository<Aluguel>
{
    // Filtros - Requisito 2.5
    Task<IEnumerable<Aluguel>> ObterAlugueisAtivosAsync();
    Task<IEnumerable<Aluguel>> ObterAluguelsPorClienteAsync(int clienteId);
    Task<IEnumerable<Aluguel>> ObterAluguelsPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<IEnumerable<Aluguel>> ObterAlugueisDevolvosAsync();
    Task<IEnumerable<Aluguel>> ObterAluguelsPorVeiculoAsync(int veiculoId);
}