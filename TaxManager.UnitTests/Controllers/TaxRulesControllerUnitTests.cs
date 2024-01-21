using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using TaxManager.Contants;
using TaxManager.Controllers;
using UnitOfWork.Interfaces;

namespace TaxManager.UnitTests.Controllers;

[TestFixture]
public class TaxRulesControllerUnitTests
{
    private TaxRuleController _controller = default!;
    private ILogger<TaxRuleController> _logger = default!;
    private ITaxUnitOfWork _taxUnitOfWork = default!;
    private IAuthenticationService _authenticationService = default!;


    [SetUp]
    public void BeforeEveryTest()
    {
        _taxUnitOfWork = Substitute.For<ITaxUnitOfWork>();
        _logger = Substitute.For<ILogger<TaxRuleController>>();
        _authenticationService = new AuthenticationService();

        _controller = new TaxRuleController(_logger, _taxUnitOfWork, _authenticationService);
    }

    #region GetTaxRules
    [Test]
    public async Task GetTaxRules_Successfully()
    {
        var taxRules = new List<TaxRule>()
        {
            new TaxRule() { Id = 1, CityId = 1},
            new TaxRule() { Id = 2, CityId = 1}
        };
        var repository = Substitute.For<IRepository<TaxRule>>();
        repository.GetAllAsync().Returns(taxRules);
        _taxUnitOfWork.GetRepository<TaxRule>().Returns(repository);

        var result = await _controller.Get(Roles.Admin);

        Assert.That(result, Is.Not.Null);
        var actual = (result.Result as OkObjectResult).Value as IEnumerable<TaxRule>;

        Assert.Multiple(() =>
        {
            Assert.That(actual.Count(), Is.EqualTo(taxRules.Count));
            Assert.That(actual.First().Id, Is.EqualTo(taxRules.First().Id));
            Assert.That(actual.First().Tax, Is.EqualTo(taxRules.First().Tax));
            Assert.That(actual.Last().Id, Is.EqualTo(taxRules.Last().Id));
            Assert.That(actual.Last().Tax, Is.EqualTo(taxRules.Last().Tax));
        });
    }

    [Test]
    public async Task GetTaxRules_For_User_Failed()
    {
        var taxRules = new List<TaxRule>()
        {
            new TaxRule() { Id = 1, CityId = 1},
            new TaxRule() { Id = 2, CityId = 1}
        };
        var repository = Substitute.For<IRepository<TaxRule>>();
        repository.GetAllAsync().Returns(taxRules);
        _taxUnitOfWork.GetRepository<TaxRule>().Returns(repository);

        var result = await _controller.Get(Roles.User);

        var actual = result.Result as UnauthorizedObjectResult;
        Assert.That(actual, Is.Null);
    }

    [Test]
    public async Task GetTaxRules_Failed()
    {
        var repository = Substitute.For<IRepository<TaxRule>>();
        _taxUnitOfWork.GetRepository<TaxRule>().Returns(repository);
        repository.GetAllAsync().ThrowsAsync(new Exception());

        var result = await _controller.Get(Roles.Admin);

        var actual = result.Result as BadRequestResult;
        Assert.That(actual, Is.Not.Null);
    }
    #endregion

    #region CreateTaxRule

    [Test]
    public async Task CreateTaxRule_Successfully()
    {
        var taxRule = new TaxRule() { Id = 2, CityId = 1 };

        var repository = Substitute.For<IRepository<TaxRule>>();
        _taxUnitOfWork.GetRepository<TaxRule>().Returns(repository);

        var result = await _controller.Create(Roles.Admin, taxRule);
        var actual = result as OkResult;

        Assert.That(actual, Is.Not.Null);
    }

    [Test]
    public async Task CreateTaxRule_Failed_For_User()
    {
        var taxRule = new TaxRule() { Id = 2, CityId = 1 };

        var repository = Substitute.For<IRepository<TaxRule>>();
        _taxUnitOfWork.GetRepository<TaxRule>().Returns(repository);

        var result = await _controller.Create(Roles.User, taxRule);
        var actual = result as UnauthorizedResult;

        Assert.That(actual, Is.Not.Null);
    }

    [Test]
    public async Task CreateTaxRule_Failed()
    {
        var taxRule = new TaxRule() { Id = 2, CityId = 1 };

        var repository = Substitute.For<IRepository<TaxRule>>();
        _taxUnitOfWork.GetRepository<TaxRule>().Throws(new Exception());

        var result = await _controller.Create(Roles.Admin, taxRule);
        var actual = result as BadRequestResult;

        Assert.That(actual, Is.Not.Null);
    }

    #endregion


    #region UpdateTaxRule

    [Test]
    public async Task UpdateTaxRule_Successfully()
    {
        var taxRule = new TaxRule() { Id = 2, CityId = 1 };

        var repository = Substitute.For<IRepository<TaxRule>>();
        _taxUnitOfWork.GetRepository<TaxRule>().Returns(repository);
        repository.GetByIdAsync(taxRule.Id).Returns(taxRule);

        var result = await _controller.Update(Roles.Admin, taxRule);
        var actual = result as OkResult;

        Assert.That(actual, Is.Not.Null);
    }

    [Test]
    public async Task UpdateTaxRule_Failed_For_User()
    {
        var taxRule = new TaxRule() { Id = 2, CityId = 1 };

        var repository = Substitute.For<IRepository<TaxRule>>();
        _taxUnitOfWork.GetRepository<TaxRule>().Returns(repository);

        var result = await _controller.Update(Roles.User, taxRule);
        var actual = result as UnauthorizedResult;

        Assert.That(actual, Is.Not.Null);
    }

    [Test]
    public async Task UpdateTaxRule_Failed()
    {
        var taxRule = new TaxRule() { Id = 2, CityId = 1 };

        _taxUnitOfWork.GetRepository<TaxRule>().Throws(new Exception());

        var result = await _controller.Update(Roles.Admin, taxRule);
        var actual = result as BadRequestResult;

        Assert.That(actual, Is.Not.Null);
    }

    [Test]
    public async Task UpdateTaxRule_If_Not_Found_Failed()
    {
        var taxRule = new TaxRule() { Id = 2, CityId = 1 };

        var repository = Substitute.For<IRepository<TaxRule>>();
        _taxUnitOfWork.GetRepository<TaxRule>().Returns(repository);
        var result = await _controller.Update(Roles.Admin, taxRule);
        var actual = result as BadRequestResult;

        Assert.That(actual, Is.Not.Null);
    }
    #endregion

    #region DeleteTaxRule

    [Test]
    public async Task DeleteTaxRule_If_Not_Found_Successfully()
    {
        var repository = Substitute.For<IRepository<TaxRule>>();
        _taxUnitOfWork.GetRepository<TaxRule>().Returns(repository);

        var result = await _controller.Delete(Roles.Admin, 2);
        var actual = result as OkResult;

        Assert.That(actual, Is.Not.Null);
    }

    [Test]
    public async Task DeleteTaxRule_If_Found_Successfully()
    {
        var taxRule = new TaxRule() { Id = 2, CityId = 1 };

        var repository = Substitute.For<IRepository<TaxRule>>();
        _taxUnitOfWork.GetRepository<TaxRule>().Returns(repository);
        repository.GetByIdAsync(taxRule.Id).Returns(taxRule);

        var result = await _controller.Delete(Roles.Admin, 2);
        var actual = result as OkResult;

        Assert.That(actual, Is.Not.Null);
    }

    [Test]
    public async Task DeleteTaxRule_Failed_For_User()
    {
        var taxRule = new TaxRule() { Id = 2, CityId = 1 };

        var repository = Substitute.For<IRepository<TaxRule>>();
        _taxUnitOfWork.GetRepository<TaxRule>().Returns(repository);

        var result = await _controller.Delete(Roles.User, taxRule.Id);
        var actual = result as UnauthorizedResult;

        Assert.That(actual, Is.Not.Null);
    }

    [Test]
    public async Task DeleteTaxRule_Failed()
    {
        var taxRule = new TaxRule() { Id = 2, CityId = 1 };

        _taxUnitOfWork.GetRepository<TaxRule>().Throws(new Exception());

        var result = await _controller.Delete(Roles.Admin, taxRule.Id);
        var actual = result as BadRequestResult;

        Assert.That(actual, Is.Not.Null);
    }

    #endregion
}