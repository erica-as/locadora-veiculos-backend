using Locadora.Api.Domain.Entities;

namespace Locadora.Api.Service.Interfaces;

public interface IAluguelService
{
    Task<IEnumerable<Aluguel>> ObterTodosAsync();
    Task<Aluguel> ObterPorIdAsync(int id);
    Task AdicionarAsync(Aluguel aluguel);
    Task AtualizarAsync(Aluguel aluguel);
    Task RemoverAsync(int id);
}

