using Microsoft.EntityFrameworkCore;
using JobService.Models;

namespace JobService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Job> Jobs => Set<Job>();
}