using Microsoft.EntityFrameworkCore;
using Test.Api.Entities;
// using Microsoft.Extensions.Configuration;

namespace Test.Api.Data;

public class CatalogDbContext : DbContext
{
    // protected readonly IConfiguration Configuration;
    //
    // public CatalogDbContext(IConfiguration configuration)
    // {
    //     Configuration = configuration;
    // }
    // 
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseNpgsql(Configuration.GetConnectionString("MusicCatalog"));
    // }
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
        : base(options)
    {
    }

    public DbSet<AlbumEntity> Albums => Set<AlbumEntity>();

    public DbSet<GenreEntity> Genres => Set<GenreEntity>();

    public DbSet<BandEntity> Bands => Set<BandEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<GenreEntity>().HasData(
            new {Id = 1, Name = "Black Metal"},
            new {Id = 2, Name = "Death Metal"},
            new {Id = 3, Name = "Progressive Rock"}
        );

        modelBuilder.Entity<BandEntity>().HasData(
            new {Id = 1, Name = "Bethlehem"},
            new {Id = 2, Name = "Devourment"},
            new {Id = 3, Name = "Pink Floyd"}
        );
    }

}

