using Microsoft.EntityFrameworkCore;

namespace TaxManager.Models;

public class TaxContext : DbContext
{
    public TaxContext(DbContextOptions<TaxContext> options) : base(options)
    {
    }
}