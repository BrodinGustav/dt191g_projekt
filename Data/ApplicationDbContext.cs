using dt191g_projekt.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SanitationApp.Models;

namespace dt191g_projekt.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<SanitationModel>? Sanitations { get; set; }
    public DbSet<CustomerModel> Customers { get; set; }
    public DbSet<WorkerModel> Workers { get; set; }
}
