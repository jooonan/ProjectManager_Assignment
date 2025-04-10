using Infrastructure.Context;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ClientRepository(DataContext context)
{
    private readonly DataContext _context = context;

    public async Task AddClientAsync(ClientEntity client)
    {
        try
        {
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to add client: {ex.Message}", ex);
        }
    }

    public async Task<ClientEntity?> GetClientByNameAsync(string clientName)
    {
        try
        {
            return await _context.Clients.FirstOrDefaultAsync(c => c.ClientName == clientName);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get client by name: {ex.Message}", ex);
        }
    }
}