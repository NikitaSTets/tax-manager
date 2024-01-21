using Models;
namespace Services.Interfaces;

public interface ICityService : IEntityService<City>
{
    Task<City> GetByIdAsync(int id);
}
