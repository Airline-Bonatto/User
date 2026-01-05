using FluentAssertions;

using User.api.Database;
using User.api.DTOs;
using User.api.Models;
using User.api.Repositories;
using User.api.Requests;
using User.Tests.Helpers;

using Xunit;

namespace User.Tests.Repositories;

public class AirlineUserRepositoryTests : IDisposable
{
    private readonly UserContext _context;
    private readonly AirlineUserRepository _repository;

    public AirlineUserRepositoryTests()
    {
        UserContext context = InMemoryDatabaseHelper.CreateInMemoryContext();
        AirlineUserRepository repository = new(context);

        _context = context;
        _repository = repository;
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task AddAsync_ShouldAddUserAndReturnId()
    {
        // Arrange
        AirlineUserCreateRequest request = new()
        {
            Email = "teste@example.com",
            Password = "senha123",
            Document = "12345678900",
            Name = "João",
            LastName = "Silva"
        };
        AirlineUser airlineUser = new(request);

        // Act
        int userId = await _repository.AddAsync(airlineUser);

        // Assert
        userId.Should().BeGreaterThan(0);
        airlineUser.AirlineUserId.Should().Be(userId);

        AirlineUser? savedUser = await _context.AirlineUsers.FindAsync(userId);
        savedUser.Should().NotBeNull();
        savedUser!.Email.Should().Be(request.Email);
        savedUser.Name.Should().Be(request.Name);
        savedUser.LastName.Should().Be(request.LastName);
        savedUser.Document.Should().Be(request.Document);
    }

    [Fact]
    public async Task GetByCredentialsAsync_ShouldReturnUserWhenCredentialsAreCorrect()
    {
        // Arrange
        AirlineUser user = new()
        {
            Email = "usuario@example.com",
            Password = "senhaSegura123",
            Document = "98765432100",
            Name = "Maria",
            LastName = "Santos"
        };
        await _repository.AddAsync(user);

        AuthenticationDto authDto = new()
        {
            Username = "usuario@example.com",
            Password = "senhaSegura123"
        };

        // Act
        AirlineUser? result = await _repository.GetByCredentialsAsync(authDto);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be(authDto.Username);
        result.Password.Should().Be(authDto.Password);
        result.Name.Should().Be("Maria");
        result.LastName.Should().Be("Santos");
    }

    [Fact]
    public async Task GetByCredentialsAsync_ShouldReturnNullWhenEmailIsIncorrect()
    {
        // Arrange
        AirlineUser user = new()
        {
            Email = "correto@example.com",
            Password = "senha123",
            Document = "11111111111",
            Name = "Pedro",
            LastName = "Costa"
        };
        await _repository.AddAsync(user);

        AuthenticationDto authDto = new()
        {
            Username = "incorreto@example.com",
            Password = "senha123"
        };

        // Act
        AirlineUser? result = await _repository.GetByCredentialsAsync(authDto);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByCredentialsAsync_ShouldReturnNullWhenPasswordIsIncorrect()
    {
        // Arrange
        AirlineUser user = new()
        {
            Email = "teste@example.com",
            Password = "senhaCorreta",
            Document = "22222222222",
            Name = "Ana",
            LastName = "Lima"
        };
        await _repository.AddAsync(user);

        AuthenticationDto authDto = new()
        {
            Username = "teste@example.com",
            Password = "senhaErrada"
        };

        // Act
        AirlineUser? result = await _repository.GetByCredentialsAsync(authDto);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_ShouldAllowAddingMultipleUsers()
    {
        // Arrange
        AirlineUser user1 = new()
        {
            Email = "usuario1@example.com",
            Password = "senha1",
            Document = "11111111111",
            Name = "Usuario",
            LastName = "Um"
        };

        AirlineUser user2 = new()
        {
            Email = "usuario2@example.com",
            Password = "senha2",
            Document = "22222222222",
            Name = "Usuario",
            LastName = "Dois"
        };

        // Act
        int id1 = await _repository.AddAsync(user1);
        int id2 = await _repository.AddAsync(user2);

        // Assert
        id1.Should().NotBe(id2);
        id1.Should().BeGreaterThan(0);
        id2.Should().BeGreaterThan(0);

        List<AirlineUser> allUsers = _context.AirlineUsers.ToList();
        allUsers.Should().HaveCount(2);
    }
}
