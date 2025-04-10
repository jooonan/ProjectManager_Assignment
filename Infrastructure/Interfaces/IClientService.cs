using Infrastructure.Dtos;

namespace Infrastructure.Interfaces;

public interface IClientService
{
    Task<ClientDto> AddClientAsync(string clientName);
    Task<ClientDto?> GetClientByNameAsync(string clientName);
}