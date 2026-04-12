using Locadora.Api.Domain.Entities;
using Locadora.Api.Domain.Interfaces;
using Locadora.Api.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Locadora.Api.Infra.Repositories;

public class FabricanteRepository : IFabricanteRepository
{
    private readonly LocadoraContext _context;

    public FabricanteRepository(LocadoraContext context) => _context = context;

    public async Task<IEnumerable<Fabricante>> ObterTodosAsync()
    {
        return await _context.Fabricantes.ToListAsync();
    }

    public async Task<Fabricante> ObterPorIdAsync(int id)
    {
        var fabricante = await _context.Fabricantes.FindAsync(id);
        return fabricante ?? throw new KeyNotFoundException($"Fabricante com id {id} nao encontrado.");
    }

    public async Task AdicionarAsync(Fabricante fabricante)
    {
        await _context.Fabricantes.AddAsync(fabricante);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Fabricante fabricante)
    {
        _context.Fabricantes.Update(fabricante);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(int id)
    {
        var fabricante = await _context.Fabricantes.FindAsync(id);
        if (fabricante is null)
            throw new KeyNotFoundException($"Fabricante com id {id} nao encontrado.");

        _context.Fabricantes.Remove(fabricante);
        await _context.SaveChangesAsync();
    }
}