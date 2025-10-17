using Microsoft.EntityFrameworkCore;

namespace User.api.Database;

public class UserContext(DbContextOptions<UserContext> options) : DbContext(options)
{

}