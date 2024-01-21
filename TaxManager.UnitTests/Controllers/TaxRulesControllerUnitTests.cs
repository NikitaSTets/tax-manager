using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Services.Interfaces;
using TaxManager.Contants;
using TaxManager.Controllers;

namespace TaxManager.UnitTests.Controllers;

[TestFixture]
public class TaxRulesControllerUnitTests
{
    private TaxRuleController _controller = default!;
    private ILogger<TaxRuleController> _logger = default!;
    private ITaxRuleService _taxRuleService = default!;
    private IAuthenticationService _authenticationService = default!;


    [SetUp]
    public void BeforeEveryTest()
    {
        _logger = Substitute.For<ILogger<TaxRuleController>>();
        _authenticationService = new AuthenticationService();
        _taxRuleService = Substitute.For<ITaxRuleService>();

        _controller = new TaxRuleController(_logger, _taxRuleService, _authenticationService);
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

        _taxRuleService.GetAllAsync().Returns(taxRules);

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
        _taxRuleService.GetAllAsync().Returns(taxRules);

        var result = await _controller.Get(Roles.User);

        var actual = result.Result as UnauthorizedObjectResult;
        Assert.That(actual, Is.Null);
    }

    [Test]
    public async Task GetTaxRules_Failed()
    {
        _taxRuleService.GetAllAsync().ThrowsAsync(new Exception());

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

        var result = await _controller.Create(Roles.Admin, taxRule);
        var actual = result as OkResult;

        await _taxRuleService.Received(1).AddAsync(taxRule);
        Assert.That(actual, Is.Not.Null);
    }

    [Test]
    public async Task CreateTaxRule_Failed_For_User()
    {
        var taxRule = new TaxRule() { Id = 2, CityId = 1 };

        var result = await _controller.Create(Roles.User, taxRule);
        var actual = result as UnauthorizedResult;
        await _taxRuleService.Received(0).AddAsync(taxRule);

        Assert.That(actual, Is.Not.Null);
    }

    [Test]
    public async Task CreateTaxRule_Failed()
    {
        var taxRule = new TaxRule() { Id = 2, CityId = 1 };
        _taxRuleService.AddAsync(taxRule).Throws(new Exception());

        var result = await _controller.Create(Roles.Admin, taxRule);
        var actual = result as BadRequestResult;
        await _taxRuleService.Received(1).AddAsync(taxRule);

        Assert.That(actual, Is.Not.Null);
    }

    #endregion


    #region UpdateTaxRule

    [Test]
    public async Task UpdateTaxRule_Successfully()
    {
        var taxRule = new TaxRule() { Id = 2, CityId = 1 };

        var result = await _controller.Update(Roles.Admin, taxRule);
        var actual = result as OkResult;
        await _taxRuleService.Received(1).UpdateAsync(taxRule);

        Assert.That(actual, Is.Not.Null);
    }

    [Test]
    public async Task UpdateTaxRule_Failed_For_User()
    {
        var taxRule = new TaxRule() { Id = 2, CityId = 1 };

        var result = await _controller.Update(Roles.User, taxRule);
        var actual = result as UnauthorizedResult;

        await _taxRuleService.Received(0).UpdateAsync(taxRule);

        Assert.That(actual, Is.Not.Null);
    }

    [Test]
    public async Task UpdateTaxRule_Failed()
    {
        var taxRule = new TaxRule() { Id = 2, CityId = 1 };

        _taxRuleService.UpdateAsync(taxRule).Throws(new Exception());

        var result = await _controller.Update(Roles.Admin, taxRule);
        var actual = result as BadRequestResult;
        await _taxRuleService.Received(1).UpdateAsync(taxRule);

        Assert.That(actual, Is.Not.Null);
    }

    #endregion

    #region DeleteTaxRule

    [Test]
    public async Task DeleteTaxRule_If_Not_Found_Successfully()
    {
        var id = 2;
        var result = await _controller.Delete(Roles.Admin, id);
        var actual = result as OkResult;
        await _taxRuleService.Received(1).DeleteAsync(id);

        Assert.That(actual, Is.Not.Null);
    }

    [Test]
    public async Task DeleteTaxRule_If_Found_Successfully()
    {
        var id = 2;
        var result = await _controller.Delete(Roles.Admin, id);
        var actual = result as OkResult;
        await _taxRuleService.Received(1).DeleteAsync(id);

        Assert.That(actual, Is.Not.Null);
    }

    [Test]
    public async Task DeleteTaxRule_Failed_For_User()
    {
        var taxRule = new TaxRule() { Id = 2, CityId = 1 };

        var result = await _controller.Delete(Roles.User, taxRule.Id);
        var actual = result as UnauthorizedResult;
        await _taxRuleService.Received(0).DeleteAsync(taxRule.Id);

        Assert.That(actual, Is.Not.Null);
    }

    [Test]
    public async Task DeleteTaxRule_Failed()
    {
        var taxRule = new TaxRule() { Id = 2, CityId = 1 };

        _taxRuleService.DeleteAsync(taxRule.Id).Throws(new Exception());

        var result = await _controller.Delete(Roles.Admin, taxRule.Id);
        var actual = result as BadRequestResult;
        await _taxRuleService.Received(1).DeleteAsync(taxRule.Id);

        Assert.That(actual, Is.Not.Null);
    }

    #endregion
}