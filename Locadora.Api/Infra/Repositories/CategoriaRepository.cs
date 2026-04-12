using Locadora.Api.Domain.Entities;
using Locadora.Api.Domain.Interfaces;
using Locadora.Api.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Locadora.Api.Infra.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly LocadoraContext _context;

    public CategoriaRepository(LocadoraContext context) => _context = context;

    public async Task<IEnumerable<Categoria>> ObterTodosAsync()
    {
       return await _context.Categorias.ToListAsync();
    }

    public async Task<Categoria> ObterPorIdAsync(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        return categoria ?? throw new KeyNotFoundException($"Categoria com id {id} nao encontrada.");
    }

    public async Task AdicionarAsync(Categoria categoria)
    {
        await _context.Categorias.AddAsync(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Categoria categoria)
    {
        _context.Categorias.Update(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria is null)
            throw new KeyNotFoundException($"Categoria com id {id} nao encontrada.");

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();
    }
}