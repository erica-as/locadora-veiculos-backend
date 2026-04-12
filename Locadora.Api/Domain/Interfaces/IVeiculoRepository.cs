using Locadora.Api.Domain.Entities;

namespace Locadora.Api.Domain.Interfaces;

public interface IVeiculoRepository
{
    Task<IEnumerable<Veiculo>> ObterTodosAsync();
    Task<Veiculo> ObterPorIdAsync(int id);
    Task AdicionarAsync(Veiculo veiculo);
    Task AtualizarAsync(Veiculo veiculo);
    Task RemoverAsync(int id);
}