using Locadora.Api.Domain.Entities;
namespace Locadora.Api.Domain.Interfaces;

public interface IAluguelRepository
{
    Task<IEnumerable<Aluguel>> ObterTodosAsync();
    Task<Aluguel> ObterPorIdAsync(int id);
    Task AdicionarAsync(Aluguel aluguel);
    Task AtualizarAsync(Aluguel aluguel);
    Task RemoverAsync(int id);
}