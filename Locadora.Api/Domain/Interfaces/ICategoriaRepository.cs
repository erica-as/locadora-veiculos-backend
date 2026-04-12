using Locadora.Api.Domain.Entities;

namespace Locadora.Api.Domain.Interfaces;

public interface ICategoriaRepository
{
    Task<IEnumerable<Categoria>> ObterTodosAsync();
    Task<Categoria> ObterPorIdAsync(int id);
    Task AdicionarAsync(Categoria categoria);
    Task AtualizarAsync(Categoria categoria);
    Task RemoverAsync(int id);
}