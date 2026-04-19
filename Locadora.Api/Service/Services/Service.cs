using Locadora.Api.Domain.Interfaces;
using Locadora.Api.Service.Interfaces;

namespace Locadora.Api.Service.Services;

public abstract class Service<T, TRepository> : IService<T>
    where T : class
    where TRepository : IRepository<T>
{
    protected readonly TRepository Repository;

    protected Service(TRepository repository)
    {
        Repository = repository;
    }

    public virtual Task<IEnumerable<T>> ObterTodosAsync() => Repository.ObterTodosAsync();

    public virtual Task<T> ObterPorIdAsync(int id) => Repository.ObterPorIdAsync(id);

    public virtual Task AdicionarAsync(T entidade) => Repository.AdicionarAsync(entidade);

    public virtual Task AtualizarAsync(T entidade) => Repository.AtualizarAsync(entidade);

    public virtual Task RemoverAsync(int id) => Repository.RemoverAsync(id);
}

