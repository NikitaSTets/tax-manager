using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Linq.Expressions;
using TaxManager.Contants;
using TaxManager.Controllers;
using UnitOfWork.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaxManager.UnitTests.Controllers;

[TestFixture]
public class CityControllerUnitTests
{
    private CityController _controller = default!;
    private ILogger<CityController> _logger = default!;
    private ITaxUnitOfWork _taxUnitOfWork = default!;
    private DbContext _dbContext = default!;

    [SetUp]
    public void BeforeEveryTest()
    {
        _taxUnitOfWork = Substitute.For<ITaxUnitOfWork>();
        _logger = Substitute.For<ILogger<CityController>>();
        _dbContext = Substitute.For<DbContext>();

        _controller = new CityController(_logger, _taxUnitOfWork);
    }

    [Test]
    public async Task GetCities_Successfully()
    {
        var cities = new List<City>()
        {
            new City() { Id = 1, Name = "AAA"},
            new City() { Id = 2, Name = "BBB"}
        };
        var repository = Substitute.For<IRepository<City>>();
        repository.GetAllAsync().Returns(cities);
        _taxUnitOfWork.GetRepository<City>().Returns(repository);

        var result = await _controller.Get();

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);

            Assert.That(result.Count(), Is.EqualTo(cities.Count));
            Assert.That(result.First().Id, Is.EqualTo(cities.First().Id));
            Assert.That(result.First().Name, Is.EqualTo(cities.First().Name));
            Assert.That(result.Last().Id, Is.EqualTo(cities.Last().Id));
            Assert.That(result.Last().Name, Is.EqualTo(cities.Last().Name));
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

        var repository = Substitute.For<IRepository<TaxRule>>();
        _taxUnitOfWork.GetRepository<TaxRule>().Returns(repository);
        repository.FindByCondition(Arg.Any<Expression<Func<TaxRule, bool>>>()).Returns(taxRules);
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

        var repository = Substitute.For<IRepository<TaxRule>>();
        _taxUnitOfWork.GetRepository<TaxRule>().Returns(repository);
        repository.FindByCondition(Arg.Any<Expression<Func<TaxRule, bool>>>()).Returns(taxRules.Where(tax => tax.CityId == cityId && tax.ToDate >= dateTime && tax.FromDate <= dateTime));
        var result = await _controller.GetCityTaxes(cityId, role, dateTime);
        var actual = (result.Result as OkObjectResult).Value as IEnumerable<TaxRule>;

        Assert.That(actual.Count, Is.EqualTo(1));
        Assert.That(actual.First(), Is.EqualTo(taxRules.First()));
    }
}