using Locadora.Api.Domain.Entities;
using Locadora.Api.Domain.Interfaces;
using Locadora.Api.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Locadora.Api.Infra.Repositories;

public class AluguelRepository : IAluguelRepository
{
    private readonly LocadoraContext _context;

    public AluguelRepository(LocadoraContext context) => _context = context;

    public async Task<IEnumerable<Aluguel>> ObterTodosAsync() => 
        await _context.Alugueis.Include(a => a.Cliente).Include(a => a.Veiculo).ToListAsync();

    public async Task<Aluguel> ObterPorIdAsync(int id)
    {
        var aluguel = await _context.Alugueis.FindAsync(id);
        return aluguel ?? throw new KeyNotFoundException($"Aluguel com id {id} nao encontrado.");
    }
    
    public async Task AdicionarAsync(Aluguel aluguel)
    {
        await _context.Alugueis.AddAsync(aluguel);
        await _context.SaveChangesAsync();
    }

    public Task AtualizarAsync(Aluguel aluguel)
    {
        _context.Alugueis.Update(aluguel);
        return _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(int id)
    {
        var aluguel = await _context.Alugueis.FindAsync(id);
        if (aluguel is null)
            throw new KeyNotFoundException($"Aluguel com id {id} nao encontrado.");

        _context.Alugueis.Remove(aluguel);
        await _context.SaveChangesAsync();
    }
}