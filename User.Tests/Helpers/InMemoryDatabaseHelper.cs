using Microsoft.EntityFrameworkCore;

using User.api.Database;

namespace User.Tests.Helpers;

public static class InMemoryDatabaseHelper
{

    public static UserContext CreateInMemoryContext(string? databaseName = null)
    {
        var dbName = databaseName ?? Guid.NewGuid().ToString();

        var options = new DbContextOptionsBuilder<UserContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .EnableSensitiveDataLogging()
            .Options;

        UserContext context = new(options);

        context.Database.EnsureCreated();

        return context;
    }


    public static UserContext CreateInMemoryContextWithData(
        Action<UserContext> seedAction,
        string? databaseName = null)
    {
        UserContext context = CreateInMemoryContext(databaseName);

        seedAction(context);
        context.SaveChanges();

        return context;
    }


    public static void ClearDatabase(UserContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
}
