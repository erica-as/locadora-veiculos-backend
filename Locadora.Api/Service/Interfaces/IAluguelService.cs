using Locadora.Api.Domain.Entities;

namespace Locadora.Api.Service.Interfaces;

public interface IAluguelService : IService<Aluguel>
{
    Task<IEnumerable<Aluguel>> ObterAlugueisAtivosAsync();
    Task<IEnumerable<Aluguel>> ObterAluguelsPorClienteAsync(int clienteId);
    Task<IEnumerable<Aluguel>> ObterAluguelsPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<IEnumerable<Aluguel>> ObterAlugueisDevolvosAsync();
    Task<IEnumerable<Aluguel>> ObterAluguelsPorVeiculoAsync(int veiculoId);
}

