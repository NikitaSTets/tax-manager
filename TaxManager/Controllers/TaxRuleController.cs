using Microsoft.AspNetCore.Mvc;
using Models;
using TaxManager.Contants;
using UnitOfWork.Interfaces;

namespace TaxManager.Controllers;

[ApiController]
[Route("[controller]")]
public class TaxRuleController : ControllerBase
{
    private readonly ILogger<TaxRuleController> _logger;
    private readonly ITaxUnitOfWork _taxUnitOfWork;
    private readonly IAuthenticationService _authenticationService;


    public TaxRuleController(
        ILogger<TaxRuleController> logger,
        ITaxUnitOfWork taxUnitOfWork,
        IAuthenticationService authenticationService)
    {
        _logger = logger;
        _taxUnitOfWork = taxUnitOfWork;
        _authenticationService = authenticationService;
    }


    [HttpGet(Name = "Get TaxRules")]
    public async Task<ActionResult<IEnumerable<TaxRule>>> Get([FromHeader(Name = Headers.RoleHeaderName)] Roles role)
    {
        try
        {
            var isAllowed = _authenticationService.CheckIfActionAllowed(Roles.Admin, role);
            if (!isAllowed)
            {
                return Unauthorized();
            }

            var taxRuleRepository = _taxUnitOfWork.GetRepository<TaxRule>();
            var taxRules = await taxRuleRepository.GetAllAsync();

            return Ok(taxRules);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Get Tax Rules is failed");

            return BadRequest();
        }
    }

    [HttpPost(Name = "Create TaxRule")]
    public async Task<ActionResult> Create(
        [FromHeader(Name = Headers.RoleHeaderName)] Roles role,
        [FromBody] TaxRule taxRule)
    {
        try
        {
            var isAllowed = _authenticationService.CheckIfActionAllowed(Roles.Admin, role);
            if (!isAllowed)
            {
                return Unauthorized();
            }

            var taxRuleRepository = _taxUnitOfWork.GetRepository<TaxRule>();
            taxRuleRepository.Add(taxRule);
            await _taxUnitOfWork.SaveAsync();

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Create Rules is failed");

            return BadRequest();
        }
    }

    [HttpPut(Name = "Update TaxRule")]
    public async Task<ActionResult> Update(
        [FromHeader(Name = Headers.RoleHeaderName)] Roles role,
        [FromBody] TaxRule taxRule)
    {
        try
        {
            var isAllowed = _authenticationService.CheckIfActionAllowed(Roles.Admin, role);
            if (!isAllowed)
            {
                return Unauthorized();
            }
            var taxRuleRepository = _taxUnitOfWork.GetRepository<TaxRule>();
            var taxRuleDb = await taxRuleRepository.GetByIdAsync(taxRule.Id);
            if (taxRuleDb is null)
            {
                return BadRequest();
            }

            taxRuleRepository.Update(taxRule);
            await _taxUnitOfWork.SaveAsync();

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Create Rules is failed");

            return BadRequest();
        }
    }

    [HttpDelete(Name = "Delete TaxRule")]
    public async Task<ActionResult> Delete(
        [FromHeader(Name = Headers.RoleHeaderName)] Roles role,
        int id)
    {
        try
        {
            var isAllowed = _authenticationService.CheckIfActionAllowed(Roles.Admin, role);
            if (!isAllowed)
            {
                return Unauthorized();
            }

            var taxRuleRepository = _taxUnitOfWork.GetRepository<TaxRule>();
            var taxRule = await taxRuleRepository.GetByIdAsync(id);
            if (taxRule is null)
            {
                return Ok();
            }
            taxRuleRepository.Delete(id);

            await _taxUnitOfWork.SaveAsync();

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Delete Rule is failed");

            return BadRequest();
        }
    }
}
