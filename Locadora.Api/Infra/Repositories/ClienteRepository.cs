using Locadora.Api.Domain.Entities;
using Locadora.Api.Domain.Interfaces;
using Locadora.Api.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Locadora.Api.Infra.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly LocadoraContext _context;

    public ClienteRepository(LocadoraContext context) => _context = context;

    public async Task<IEnumerable<Cliente>> ObterTodosAsync()
    {
        return await _context.Clientes.ToListAsync();
    }

    public async Task<Cliente> ObterPorIdAsync(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        return cliente ?? throw new KeyNotFoundException($"Cliente com id {id} nao encontrado.");
    }

    public async Task AdicionarAsync(Cliente cliente)
    {
        await _context.Clientes.AddAsync(cliente);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Cliente cliente)
    {
        _context.Clientes.Update(cliente);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente is null)
            throw new KeyNotFoundException($"Cliente com id {id} nao encontrado.");

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();
    }
}