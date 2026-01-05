using FluentAssertions;

using User.api.Database;
using User.api.Models;
using User.api.Repositories;
using User.api.Requests;
using User.api.Services;
using User.Tests.Helpers;

using Xunit;

namespace User.Tests.Services;

/// <summary>
/// Unit tests for AirlineUserCreateService using in-memory database
/// </summary>
public class AirlineUserCreateServiceTests : IDisposable
{
    private readonly UserContext _context;
    private readonly AirlineUserRepository _repository;
    private readonly AirlineUserCreateService _service;

    public AirlineUserCreateServiceTests()
    {
        UserContext context = InMemoryDatabaseHelper.CreateInMemoryContext();
        AirlineUserRepository repository = new AirlineUserRepository(context);
        AirlineUserCreateService service = new AirlineUserCreateService(repository);

        _context = context;
        _repository = repository;
        _service = service;
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task CreateAirlineUserAsync_ShouldCreateUserAndReturnId()
    {
        // Arrange
        AirlineUserCreateRequest request = new()
        {
            Email = "novo@example.com",
            Password = "senha123",
            Document = "12345678900",
            Name = "Teste",
            LastName = "Usuario"
        };

        // Act
        int result = await _service.CreateAirlineUserAsync(request);

        // Assert
        result.Should().BeGreaterThan(0);

        AirlineUser? savedUser = await _context.AirlineUsers.FindAsync(result);
        savedUser.Should().NotBeNull();
        savedUser!.Email.Should().Be(request.Email);
        savedUser.Password.Should().Be(request.Password);
        savedUser.Document.Should().Be(request.Document);
        savedUser.Name.Should().Be(request.Name);
        savedUser.LastName.Should().Be(request.LastName);
    }

    [Fact]
    public async Task CreateAirlineUserAsync_ShouldSaveDataCorrectlyInDatabase()
    {
        // Arrange
        AirlineUserCreateRequest request = new()
        {
            Email = "teste@example.com",
            Password = "senhaSegura",
            Document = "98765432100",
            Name = "Maria",
            LastName = "Silva"
        };

        // Act
        int userId = await _service.CreateAirlineUserAsync(request);

        // Assert
        userId.Should().BeGreaterThan(0);

        AirlineUser? savedUser = _context.AirlineUsers.FirstOrDefault(u => u.AirlineUserId == userId);
        savedUser.Should().NotBeNull();
        savedUser!.Email.Should().Be(request.Email);
        savedUser.Password.Should().Be(request.Password);
        savedUser.Document.Should().Be(request.Document);
        savedUser.Name.Should().Be(request.Name);
        savedUser.LastName.Should().Be(request.LastName);
    }

    [Theory]
    [InlineData("user1@example.com", "pass1", "11111111111", "Nome1", "Sobrenome1")]
    [InlineData("user2@example.com", "pass2", "22222222222", "Nome2", "Sobrenome2")]
    [InlineData("user3@example.com", "pass3", "33333333333", "Nome3", "Sobrenome3")]
    public async Task CreateAirlineUserAsync_ShouldAcceptDifferentValues(
        string email, string password, string document, string name, string lastName)
    {
        // Arrange
        AirlineUserCreateRequest request = new()
        {
            Email = email,
            Password = password,
            Document = document,
            Name = name,
            LastName = lastName
        };

        // Act
        int result = await _service.CreateAirlineUserAsync(request);

        // Assert
        result.Should().BeGreaterThan(0);

        AirlineUser? savedUser = await _context.AirlineUsers.FindAsync(result);
        savedUser.Should().NotBeNull();
        savedUser!.Email.Should().Be(email);
        savedUser.Password.Should().Be(password);
        savedUser.Document.Should().Be(document);
        savedUser.Name.Should().Be(name);
        savedUser.LastName.Should().Be(lastName);
    }

    [Fact]
    public async Task CreateAirlineUserAsync_ShouldAllowCreatingMultipleUsers()
    {
        // Arrange
        AirlineUserCreateRequest[] requests =
        [
            new AirlineUserCreateRequest { Email = "user1@test.com", Password = "pass1", Document = "111", Name = "User", LastName = "One" },
            new AirlineUserCreateRequest { Email = "user2@test.com", Password = "pass2", Document = "222", Name = "User", LastName = "Two" },
            new AirlineUserCreateRequest { Email = "user3@test.com", Password = "pass3", Document = "333", Name = "User", LastName = "Three" }
        ];

        // Act
        List<int> userIds = [];
        foreach(AirlineUserCreateRequest request in requests)
        {
            int id = await _service.CreateAirlineUserAsync(request);
            userIds.Add(id);
        }

        // Assert
        userIds.Should().HaveCount(3);
        userIds.Should().OnlyHaveUniqueItems();

        List<AirlineUser> allUsers = _context.AirlineUsers.ToList();
        allUsers.Should().HaveCount(3);
    }
}
