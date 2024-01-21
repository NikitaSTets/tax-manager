using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using TaxManager.Contants;

namespace TaxManager.Controllers;

[ApiController]
[Route("[controller]")]
public class TaxRuleController : ControllerBase
{
    private readonly ILogger<TaxRuleController> _logger;
    private readonly IAuthenticationService _authenticationService;
    private readonly ITaxRuleService _taxRuleService;


    public TaxRuleController(
        ILogger<TaxRuleController> logger,
        ITaxRuleService taxRuleService,
        IAuthenticationService authenticationService)
    {
        _logger = logger;
        _taxRuleService = taxRuleService;
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

            var taxRules = await _taxRuleService.GetAllAsync();

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
            await _taxRuleService.AddAsync(taxRule);

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

            await _taxRuleService.UpdateAsync(taxRule);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Create Rule is failed");

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

            await _taxRuleService.DeleteAsync(id);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Delete Rule is failed");

            return BadRequest();
        }
    }
}
