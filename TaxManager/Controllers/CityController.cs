using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using TaxManager.Contants;

namespace TaxManager.Controllers;

[ApiController]
[Route("api/city")]
public class CityController : ControllerBase
{
    private readonly ILogger<CityController> _logger;
    private readonly ICityService _cityService;
    private readonly ITaxRuleService _taxRuleService;


    public CityController(ILogger<CityController> logger, ICityService cityService, ITaxRuleService taxRuleService)
    {
        _logger = logger;
        _cityService = cityService;
        _taxRuleService = taxRuleService;
    }


    [HttpGet("", Name = "GetCities")]
    public async Task<ActionResult<IEnumerable<City>>> Get()
    {
        try
        {
            var cities = await _cityService.GetAllAsync();

            return Ok(cities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get Cities");

            return BadRequest();
        }
    }

    [HttpGet("{id}/taxes", Name = "Get City Taxes")]
    public async Task<ActionResult<IEnumerable<TaxRule>>> GetCityTaxes(
        int id,
        [FromHeader(Name = Headers.RoleHeaderName)] Roles role,
        [FromQuery] DateTime? date)
    {
        try
        {
            var city = await _cityService.GetByIdAsync(id);
            if (city == null)
            {
                return BadRequest();
            }

            if (date.HasValue)
            {
                var taxRule = await _taxRuleService.GetCityTaxRuleByDate(id, date.Value);

                return Ok(taxRule);
            }

            var cityTaxRules = await _taxRuleService.GetWhereAsync(tax => tax.CityId == id);

            return Ok(cityTaxRules);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get Cities");

            return BadRequest();
        }
    }
}