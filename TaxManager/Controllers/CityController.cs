using Microsoft.AspNetCore.Mvc;
using Models;
using TaxManager.Contants;
using UnitOfWork.Interfaces;

namespace TaxManager.Controllers;

[ApiController]
[Route("api/city")]
public class CityController : ControllerBase
{
    private readonly ITaxUnitOfWork _taxUnitOfWork;


    public CityController(ILogger<CityController> logger, ITaxUnitOfWork taxUnitOfWork)
    {
        _taxUnitOfWork = taxUnitOfWork;
    }

    [HttpGet("", Name = "GetCities")]
    public async Task<IEnumerable<City>> Get()
    {
        var cityRepository = _taxUnitOfWork.GetRepository<City>();
        var cities = await cityRepository.GetAllAsync();

        return cities;
    }

    [HttpGet("{id}/taxes", Name = "Get City Taxes")]
    public async Task<ActionResult<IEnumerable<TaxRule>>> GetCityTaxes(
        int id,
        [FromHeader(Name = Headers.RoleHeaderName)] Roles role,
        [FromQuery] DateTime? date)
    {
        var taxRulesRepository = _taxUnitOfWork.GetRepository<TaxRule>();
        var cityTaxRules = date.HasValue
            ? await taxRulesRepository.FindByCondition(tax => tax.CityId == id && tax.ToDate >= date && tax.FromDate <= date)
            : await taxRulesRepository.FindByCondition(tax => tax.CityId == id);

        return Ok(cityTaxRules);
    }
}