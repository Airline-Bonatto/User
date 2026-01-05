using FluentAssertions;

using User.api.Models;
using User.api.Repositories;
using User.api.Requests;
using User.api.Services;
using User.Tests.Helpers;

using Xunit;

namespace User.Tests.Integration;

/// <summary>
/// Testes de integração que demonstram o uso completo do banco de dados em memória
/// testando o fluxo completo: Service -> Repository -> Database
/// </summary>
public class AirlineUserIntegrationTests : IDisposable
{
    private readonly api.Database.UserContext _context;
    private readonly AirlineUserRepository _repository;
    private readonly AirlineUserCreateService _service;

    public AirlineUserIntegrationTests()
    {
        // Configura o banco de dados em memória e as dependências
        _context = InMemoryDatabaseHelper.CreateInMemoryContext();
        _repository = new AirlineUserRepository(_context);
        _service = new AirlineUserCreateService(_repository);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task FluxoCompleto_DeveCriarUsuarioERecuperarPorCredenciais()
    {
        // Arrange
        var request = new AirlineUserCreateRequest
        {
            Email = "integracao@example.com",
            Password = "senhaIntegracao123",
            Document = "12345678900",
            Name = "Usuario",
            LastName = "Integracao"
        };

        // Act - Criar usuário através do service
        var userId = await _service.CreateAirlineUserAsync(request);

        // Assert - Verificar se o usuário foi criado
        userId.Should().BeGreaterThan(0);

        // Act - Buscar usuário pelas credenciais
        var authDto = new api.DTOs.AuthenticationDto
        {
            Username = request.Email,
            Password = request.Password
        };
        var user = await _repository.GetByCredentialsAsync(authDto);

        // Assert - Verificar se o usuário foi encontrado com os dados corretos
        user.Should().NotBeNull();
        user!.AirlineUserId.Should().Be(userId);
        user.Email.Should().Be(request.Email);
        user.Name.Should().Be(request.Name);
        user.LastName.Should().Be(request.LastName);
        user.Document.Should().Be(request.Document);
    }

    [Fact]
    public async Task FluxoCompleto_DeveCriarMultiplosUsuariosEBuscarIndividualmente()
    {
        // Arrange
        var requests = new[]
        {
            new AirlineUserCreateRequest
            {
                Email = "user1@example.com",
                Password = "senha1",
                Document = "11111111111",
                Name = "Usuario",
                LastName = "Um"
            },
            new AirlineUserCreateRequest
            {
                Email = "user2@example.com",
                Password = "senha2",
                Document = "22222222222",
                Name = "Usuario",
                LastName = "Dois"
            },
            new AirlineUserCreateRequest
            {
                Email = "user3@example.com",
                Password = "senha3",
                Document = "33333333333",
                Name = "Usuario",
                LastName = "Tres"
            }
        };

        // Act - Criar múltiplos usuários
        var userIds = new List<int>();
        foreach(var request in requests)
        {
            var id = await _service.CreateAirlineUserAsync(request);
            userIds.Add(id);
        }

        // Assert - Verificar que todos os usuários foram criados
        userIds.Should().HaveCount(3);
        userIds.Should().OnlyHaveUniqueItems();

        // Act & Assert - Buscar cada usuário individualmente
        for(int i = 0; i < requests.Length; i++)
        {
            var authDto = new api.DTOs.AuthenticationDto
            {
                Username = requests[i].Email,
                Password = requests[i].Password
            };
            var user = await _repository.GetByCredentialsAsync(authDto);

            user.Should().NotBeNull();
            user!.AirlineUserId.Should().Be(userIds[i]);
            user.Email.Should().Be(requests[i].Email);
        }
    }

    [Fact]
    public void BancoDeDadosEmMemoria_DeveIniciarVazio()
    {
        // Assert
        var users = _context.AirlineUsers.ToList();
        users.Should().BeEmpty();
    }

    [Fact]
    public async Task BancoDeDadosEmMemoria_DevePersistirDadosEntreOperacoes()
    {
        // Arrange & Act - Primeira operação
        var request1 = new AirlineUserCreateRequest
        {
            Email = "persist1@example.com",
            Password = "senha1",
            Document = "11111111111",
            Name = "Primeiro",
            LastName = "Usuario"
        };
        var id1 = await _service.CreateAirlineUserAsync(request1);

        // Act - Segunda operação
        var request2 = new AirlineUserCreateRequest
        {
            Email = "persist2@example.com",
            Password = "senha2",
            Document = "22222222222",
            Name = "Segundo",
            LastName = "Usuario"
        };
        var id2 = await _service.CreateAirlineUserAsync(request2);

        // Assert - Ambos os usuários devem existir
        var allUsers = _context.AirlineUsers.ToList();
        allUsers.Should().HaveCount(2);
        allUsers.Should().Contain(u => u.AirlineUserId == id1);
        allUsers.Should().Contain(u => u.AirlineUserId == id2);
    }

    [Fact]
    public async Task BancoDeDadosEmMemoria_DeveSuportarOperacoesComplexas()
    {
        // Arrange - Criar múltiplos usuários
        var users = new[]
        {
            new AirlineUser { Email = "a@test.com", Password = "pass", Document = "111", Name = "A", LastName = "User" },
            new AirlineUser { Email = "b@test.com", Password = "pass", Document = "222", Name = "B", LastName = "User" },
            new AirlineUser { Email = "c@test.com", Password = "pass", Document = "333", Name = "C", LastName = "User" }
        };

        foreach(var user in users)
        {
            await _repository.AddAsync(user);
        }

        // Act - Buscar usuário específico
        var authDto = new api.DTOs.AuthenticationDto
        {
            Username = "b@test.com",
            Password = "pass"
        };
        var foundUser = await _repository.GetByCredentialsAsync(authDto);

        // Assert
        foundUser.Should().NotBeNull();
        foundUser!.Email.Should().Be("b@test.com");
        foundUser.Name.Should().Be("B");

        // Verificar que todos os usuários ainda existem
        var allUsers = _context.AirlineUsers.ToList();
        allUsers.Should().HaveCount(3);
    }
}
