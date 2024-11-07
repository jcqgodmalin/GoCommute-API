using GoCommute.Models;
using Microsoft.EntityFrameworkCore;

namespace GoCommute.Data;

public class AppDBContext : DbContext
{

    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options){ }

    public DbSet<User> Users { get; set; }
    public DbSet<App> Apps { get; set; }
    public DbSet<Route> Routes { get; set; }
    public DbSet<Marker> Markers { get; set; }
    public DbSet<RoutePoint> RoutePoints { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
        .HasMany(u => u.Apps)
        .WithOne(a => a.User)
        .HasForeignKey(a => a.UserId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Route>()
        .HasMany(r => r.Markers)
        .WithOne(m => m.Route)
        .HasForeignKey(m => m.RouteId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Route>()
        .HasMany(r => r.RoutePoints)
        .WithOne(rp => rp.Route)
        .HasForeignKey(rp => rp.RouteId)
        .OnDelete(DeleteBehavior.Cascade);
    }

}
