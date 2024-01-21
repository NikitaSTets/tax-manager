using Microsoft.EntityFrameworkCore;
using TaxManager.Models;
using UnitOfWork.Interfaces;
using UnitOfWork.UoW;

namespace TaxManager.Extensions;

public static class DBExtensions
{
    public static void AddDBExtensions(this IServiceCollection services, string taxDbConnectionString)
    {
        services.AddPooledDbContextFactory<TaxContext>(item => item.UseSqlServer(taxDbConnectionString));
        services.AddTransient<ITaxUnitOfWork, TaxUnitOfWork>();
    }
}
