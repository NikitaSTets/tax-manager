using UnitOfWork.Interfaces;
using UnitOfWork.UoW;

namespace TaxManager.Extensions;

public static class DBExtensions
{
    public static void AddDBExtensions(this IServiceCollection services)
    {
        services.AddTransient<ITaxUnitOfWork, TaxUnitOfWork>();
    }
}
