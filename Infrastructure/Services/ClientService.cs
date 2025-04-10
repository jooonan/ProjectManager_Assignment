using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class ClientService(ClientRepository clientRepository) : IClientService
{
    private readonly ClientRepository _clientRepository = clientRepository;

    public async Task<ClientDto> AddClientAsync(string clientName)
    {
        try
        {
            var existingClient = await _clientRepository.GetClientByNameAsync(clientName);
            if (existingClient != null)
            {
                return MapToClientDto(existingClient);
            }

            var newClient = new ClientEntity
            {
                ClientName = clientName
            };

            await _clientRepository.AddClientAsync(newClient);

            return MapToClientDto(newClient);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to add client: {ex.Message}", ex);
        }
    }

    public async Task<ClientDto?> GetClientByNameAsync(string clientName)
    {
        try
        {
            var client = await _clientRepository.GetClientByNameAsync(clientName);
            if (client == null) return null;

            return MapToClientDto(client);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get client by name: {ex.Message}", ex);
        }
    }

    private static ClientDto MapToClientDto(ClientEntity client)
    {
        return new()
        {
            Id = client.Id,
            ClientName = client.ClientName
        };
    }
}