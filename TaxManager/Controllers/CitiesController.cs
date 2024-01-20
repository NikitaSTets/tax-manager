using Microsoft.AspNetCore.Mvc;
using Models;
using UnitOfWork.Interfaces;

namespace TaxManager.Controllers;

[ApiController]
[Route("[controller]")]
public class CitiesController : ControllerBase
{
    private readonly IRepository<City> _cityRepository;

    public CitiesController(ILogger<CitiesController> logger, ITaxUnitOfWork taxUnitOfWork)
    {
        _cityRepository = taxUnitOfWork.GetRepository<City>();
    }

    [HttpGet(Name = "GetCities")]
    public async Task<IEnumerable<City>> Get()
    {
        var cities = await _cityRepository.GetAllAsync();

        return cities;
    }
}