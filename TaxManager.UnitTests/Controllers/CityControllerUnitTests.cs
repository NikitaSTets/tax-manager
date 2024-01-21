using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using NSubstitute;
using NUnit.Framework;
using Services;
using Services.Interfaces;
using System.Linq.Expressions;
using TaxManager.Contants;
using TaxManager.Controllers;

namespace TaxManager.UnitTests.Controllers;

[TestFixture]
public class CityControllerUnitTests
{
    private CityController _controller = default!;
    private ILogger<CityController> _logger = default!;
    private ICityService _cityService = default!;
    private ITaxRuleService _taxRuleService = default!;

    [SetUp]
    public void BeforeEveryTest()
    {
        _logger = Substitute.For<ILogger<CityController>>();
        _cityService = Substitute.For<ICityService>();
        _taxRuleService = Substitute.For<ITaxRuleService>();

        _controller = new CityController(_logger, _cityService, _taxRuleService);
    }

    [Test]
    public async Task GetCities_Successfully()
    {
        var cities = new List<City>()
        {
            new City() { Id = 1, Name = "AAA"},
            new City() { Id = 2, Name = "BBB"}
        };
        _cityService.GetAllAsync().Returns(cities);

        var result = await _controller.Get();
        var actual = (result.Result as OkObjectResult).Value as IEnumerable<City>;

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);

            Assert.That(actual.Count(), Is.EqualTo(cities.Count));
            Assert.That(actual.First().Id, Is.EqualTo(cities.First().Id));
            Assert.That(actual.First().Name, Is.EqualTo(cities.First().Name));
            Assert.That(actual.Last().Id, Is.EqualTo(cities.Last().Id));
            Assert.That(actual.Last().Name, Is.EqualTo(cities.Last().Name));
        });
    }

    [Test]
    public async Task GetCityTaxes_Successfully([Values] Roles role)
    {
        var cityId = 1;
        var taxRules = new List<TaxRule>()
        {
            new TaxRule() { Id = 2, CityId = cityId,Tax = 1 }
        };
        _cityService.GetByIdAsync(cityId).Returns(new City { Id = cityId });

        _taxRuleService.GetWhereAsync(Arg.Any<Expression<Func<TaxRule, bool>>>()).Returns(taxRules);
        var result = await _controller.GetCityTaxes(cityId, role, default);
        var actual = (result.Result as OkObjectResult).Value as IEnumerable<TaxRule>;
        Assert.That(actual.Count, Is.EqualTo(taxRules.Count));
    }

    [Test]
    public async Task GetCityTaxes_By_Date_Successfully([Values] Roles role)
    {
        var cityId = 1;
        var dateTime = new DateTime(2024, 1, 3);
        var taxRules = new List<TaxRule>()
        {
            new TaxRule() { Id = 2, CityId = cityId,Tax = 1, FromDate = new DateTime(2024, 1 , 1), ToDate = new DateTime(2024, 1 , 5) },
            new TaxRule() { Id = 2, CityId = cityId,Tax = 1, FromDate = new DateTime(2024, 2 , 1), ToDate = new DateTime(2024, 2 , 5) }
        };
        _cityService.GetByIdAsync(cityId).Returns(new City { Id = cityId });
        var taxRuleResult = taxRules.Where(tax => tax.CityId == cityId && tax.ToDate >= dateTime && tax.FromDate <= dateTime).OrderByDescending(c => c.Type);

        _taxRuleService.GetCityTaxRuleByDate(Arg.Is(cityId), Arg.Is(dateTime)).Returns(taxRuleResult);
        var result = await _controller.GetCityTaxes(cityId, role, dateTime);
        var actual = (result.Result as OkObjectResult).Value as TaxRule;

        Assert.That(actual, Is.EqualTo(taxRuleResult));
    }
}