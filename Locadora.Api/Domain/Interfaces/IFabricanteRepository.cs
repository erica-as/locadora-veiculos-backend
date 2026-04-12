using Locadora.Api.Domain.Entities;

namespace Locadora.Api.Domain.Interfaces;

public interface IFabricanteRepository
{
    Task<IEnumerable<Fabricante>> ObterTodosAsync();
    Task<Fabricante> ObterPorIdAsync(int id);
    Task AdicionarAsync(Fabricante fabricante);
    Task AtualizarAsync(Fabricante fabricante);
    Task RemoverAsync(int id);
}