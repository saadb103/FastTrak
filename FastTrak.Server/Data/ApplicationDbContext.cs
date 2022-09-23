using Microsoft.EntityFrameworkCore;

using FastTrak.Shared.Models;

namespace FastTrak.Server.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { }

    public DbSet<User> Users { get; set; }
}