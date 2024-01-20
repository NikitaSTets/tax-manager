using EFCore.TaxDb.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Reflection.Metadata;

namespace TaxManager.Models;

public class TaxContext : DbContext
{
    public TaxContext(DbContextOptions<TaxContext> options) : base(options)
    {
    }

    public DbSet<City>? Cities { get; set; }

    public DbSet<TaxRule>? TaxRules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CityEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TaxRuleTypeConfiguration());
    }
}