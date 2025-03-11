using dt191g_projekt.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SanitationApp.Models;

namespace dt191g_projekt.Data;

public class ApplicationDbContext : IdentityDbContext
{
    //Konstruktor
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    //Definierar DbSet-egenskaper för entiteter (tabeller)
    public DbSet<SanitationModel>? Sanitations { get; set; }
    public DbSet<CustomerModel> Customers { get; set; }
    public DbSet<WorkerModel> Workers { get; set; }


    //Relationer
       protected override void OnModelCreating(ModelBuilder modelBuilder)       //Metod som konfigurerar relationer mellan entiteter
        {
            base.OnModelCreating(modelBuilder);

            //Relation mellan Sanitation och Customer (en till många)
            modelBuilder.Entity<SanitationModel>()
                .HasOne(s => s.Customer)            //En sanering har en kund
                .WithMany(c => c.Sanitations)       //En kund har många saneringar
                .HasForeignKey(s => s.CustomerId)   //Sanitation refererar till Customer via CustomerId
                .OnDelete(DeleteBehavior.Cascade);  //Vid radering av Customer raderas relaterade Sanitations

            //Relation mellan Sanitation och Worker (en till många)
            modelBuilder.Entity<SanitationModel>()
                .HasOne(s => s.Worker)              //En sanering har en arbetare
                .WithMany(w => w.Sanitations)       //En arbetare har många saneringar
                .HasForeignKey(s => s.WorkerId)     //Sanitation refererar till Worker via WorkerId
                .OnDelete(DeleteBehavior.Cascade);  //Vid radering Worker raderas relaterade Sanitations
        }
    }

