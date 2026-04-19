namespace Locadora.Api.Service.Interfaces;

public interface IService<T> where T : class
{
    Task<IEnumerable<T>> ObterTodosAsync();
    Task<T> ObterPorIdAsync(int id);
    Task AdicionarAsync(T entidade);
    Task AtualizarAsync(T entidade);
    Task RemoverAsync(int id);
}

