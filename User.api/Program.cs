using System.Data;

using Microsoft.EntityFrameworkCore;

using Npgsql;

using User.api.Database;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<UserContext>(options =>
    options.UseNpgsql(builder.Configuration["ConnectionStrings:Default"]));


builder.Services.AddTransient<IDbConnection>(sp =>
{
    var cs = builder.Configuration["ConnectionStrings:Default"];
    return new NpgsqlConnection(cs);
});



WebApplication app = builder.Build();

app.MapControllers();
app.Run();
