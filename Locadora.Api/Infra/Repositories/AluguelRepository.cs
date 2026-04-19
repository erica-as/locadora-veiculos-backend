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

    
    // Filtros - Requisito 2.5
    
    /// <summary>
    /// Filtro 1: Obter aluguéis ativos (não devolvidos)
    /// Demonstra INNER JOIN entre Aluguel e Cliente
    /// </summary>
    public async Task<IEnumerable<Aluguel>> ObterAlugueisAtivosAsync()
    {
        return await _context.Alugueis
            .Where(a => a.DataDevolucao == null)
            .Include(a => a.Cliente)
            .Include(a => a.Veiculo)
            .ToListAsync();
    }

    /// <summary>
    /// Filtro 2: Obter aluguéis por cliente
    /// Demonstra INNER JOIN entre Aluguel, Cliente e Veiculo
    /// </summary>
    public async Task<IEnumerable<Aluguel>> ObterAluguelsPorClienteAsync(int clienteId)
    {
        if (clienteId <= 0)
            throw new ArgumentException("ClienteId deve ser maior que zero.", nameof(clienteId));

        return await _context.Alugueis
            .Where(a => a.ClienteId == clienteId)
            .Include(a => a.Cliente)
            .Include(a => a.Veiculo)
                .ThenInclude(v => v.Fabricante)
            .Include(a => a.Veiculo)
                .ThenInclude(v => v.Categoria)
            .ToListAsync();
    }

    /// <summary>
    /// Filtro 3: Obter aluguéis por período
    /// Demonstra filtro de data com múltiplas condições
    /// </summary>
    public async Task<IEnumerable<Aluguel>> ObterAluguelsPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        if (dataInicio > dataFim)
            throw new ArgumentException("Data de início não pode ser maior que data de fim.");

        return await _context.Alugueis
            .Where(a => a.DataInicio >= dataInicio && a.DataFimPrevista <= dataFim)
            .Include(a => a.Cliente)
            .Include(a => a.Veiculo)
            .ToListAsync();
    }

    /// <summary>
    /// Filtro 4: Obter aluguéis com devolução (devolvidos)
    /// Demonstra LEFT JOIN implícito com verificação de Nullable
    /// </summary>
    public async Task<IEnumerable<Aluguel>> ObterAlugueisDevolvosAsync()
    {
        return await _context.Alugueis
            .Where(a => a.DataDevolucao != null && a.KmFinal != null)
            .Include(a => a.Cliente)
            .Include(a => a.Veiculo)
            .ToListAsync();
    }

    /// <summary>
    /// Filtro 5: Obter aluguéis por veículo com detalhes
    /// Demonstra INNER JOIN múltiplo entre Aluguel, Veiculo, Categoria e Fabricante
    /// </summary>
    public async Task<IEnumerable<Aluguel>> ObterAluguelsPorVeiculoAsync(int veiculoId)
    {
        if (veiculoId <= 0)
            throw new ArgumentException("VeiculoId deve ser maior que zero.", nameof(veiculoId));

        return await _context.Alugueis
            .Where(a => a.VeiculoId == veiculoId)
            .Include(a => a.Cliente)
            .Include(a => a.Veiculo)
                .ThenInclude(v => v.Fabricante)
            .Include(a => a.Veiculo)
                .ThenInclude(v => v.Categoria)
            .ToListAsync();
    }
}