using Locadora.Api.Domain.Entities;
using Locadora.Api.Domain.Interfaces;
using Locadora.Api.Infra.Data;
using Microsoft.EntityFrameworkCore;


namespace Locadora.Api.Infra.Repositories;

public class VeiculoRepository : IVeiculoRepository
{
    private readonly LocadoraContext _context;

    public VeiculoRepository(LocadoraContext context) => _context = context;

    public async Task<IEnumerable<Veiculo>> ObterTodosAsync()
    {
        return await _context.Veiculos.ToListAsync();
    }

    public async Task<Veiculo> ObterPorIdAsync(int id)
    {
        var veiculo = await _context.Veiculos.FindAsync(id);
        return veiculo ?? throw new KeyNotFoundException($"Veiculo com id {id} nao encontrado.");
    }

    public async Task AdicionarAsync(Veiculo veiculo)
    {
        await _context.Veiculos.AddAsync(veiculo);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Veiculo veiculo)
    {
        _context.Veiculos.Update(veiculo);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(int id)
    {
        var veiculo = await _context.Veiculos.FindAsync(id);
        if (veiculo is null)
            throw new KeyNotFoundException($"Veiculo com id {id} nao encontrado.");

        _context.Veiculos.Remove(veiculo);
        await _context.SaveChangesAsync();
    }
}