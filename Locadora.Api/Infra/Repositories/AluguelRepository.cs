using Locadora.Api.Domain.Entities;
using Locadora.Api.Domain.Interfaces;
using Locadora.Api.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Locadora.Api.Infra.Repositories;

public class AluguelRepository : Repository<Aluguel>, IAluguelRepository
{
    public AluguelRepository(LocadoraContext context) : base(context)
    {
    }

    // Sobrescrever ObterTodosAsync para incluir relacionamentos
    public override async Task<IEnumerable<Aluguel>> ObterTodosAsync()
    {
        return await DbSet.Include(a => a.Cliente).Include(a => a.Veiculo).ToListAsync();
    }

    // ...existing code...

    /// <summary>
    /// Filtro 1: Obter aluguéis ativos (não devolvidos)
    /// Demonstra INNER JOIN entre Aluguel e Cliente
    /// </summary>
    public async Task<IEnumerable<Aluguel>> ObterAlugueisAtivosAsync()
    {
        return await DbSet
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

        return await DbSet
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

        return await DbSet
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
        return await DbSet
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

        return await DbSet
            .Where(a => a.VeiculoId == veiculoId)
            .Include(a => a.Cliente)
            .Include(a => a.Veiculo)
                .ThenInclude(v => v.Fabricante)
            .Include(a => a.Veiculo)
                .ThenInclude(v => v.Categoria)
            .ToListAsync();
    }
}