using Locadora.Api.Domain.Interfaces;
using Locadora.Api.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Locadora.Api.Infra.Repositories;

public abstract class Repository<T> : IRepository<T> where T : class
{
    protected readonly LocadoraContext Context;
    protected readonly DbSet<T> DbSet;

    protected Repository(LocadoraContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> ObterTodosAsync()
    {
        return await DbSet.ToListAsync();
    }

    public virtual async Task<T> ObterPorIdAsync(int id)
    {
        var entidade = await DbSet.FindAsync(id);
        return entidade ?? throw new KeyNotFoundException($"{typeof(T).Name} com id {id} não encontrado.");
    }

    public virtual async Task AdicionarAsync(T entidade)
    {
        await DbSet.AddAsync(entidade);
        await Context.SaveChangesAsync();
    }

    public virtual async Task AtualizarAsync(T entidade)
    {
        DbSet.Update(entidade);
        await Context.SaveChangesAsync();
    }

    public virtual async Task RemoverAsync(int id)
    {
        var entidade = await DbSet.FindAsync(id);
        if (entidade is null)
            throw new KeyNotFoundException($"{typeof(T).Name} com id {id} não encontrado.");

        DbSet.Remove(entidade);
        await Context.SaveChangesAsync();
    }
}

