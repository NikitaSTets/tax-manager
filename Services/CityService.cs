using Models;
using Services.Interfaces;
using UnitOfWork.Interfaces;

namespace Services;

public class CityService : EntityService<City>, ICityService
{
    private readonly ITaxUnitOfWork _taxUnitOfWork;


    public CityService(ITaxUnitOfWork taxUnitOfWork)
        : base(taxUnitOfWork)
    {
        _taxUnitOfWork = taxUnitOfWork;
    }

    public async Task<City> GetByIdAsync(int id)
    {
        var cityRepository = _taxUnitOfWork.GetRepository<City>();
        var city = await cityRepository.GetByIdAsync(id);

        return city;
    }
}