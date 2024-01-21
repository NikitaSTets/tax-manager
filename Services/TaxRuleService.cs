using Models;
using Services.Interfaces;
using UnitOfWork.Interfaces;

namespace Services;

public class TaxRuleService : EntityService<TaxRule>, ITaxRuleService
{
    private readonly ITaxUnitOfWork _taxUnitOfWork;


    public TaxRuleService(ITaxUnitOfWork taxUnitOfWork)
        : base(taxUnitOfWork)
    {
        _taxUnitOfWork = taxUnitOfWork;
    }

    public async Task<IEnumerable<TaxRule>> GetCityTaxRuleByDate(int cityId, DateTime date)
    {
        var taxRuleRepository = _taxUnitOfWork.GetRepository<TaxRule>();
        var taxRules = await taxRuleRepository.FindByCondition(tax => tax.CityId == cityId && tax.ToDate >= date && tax.FromDate <= date);

        //Return all taxes to the same level, as an example we have 2 'day' taxes 
        var dateTaxRules = taxRules.GroupBy(p => p.Type)
                  .OrderByDescending(p => p.Min(q => q.Type))
                  .First();

        return dateTaxRules;
    }


    public async Task UpdateAsync(TaxRule taxRule)
    {
        var taxRuleRepository = _taxUnitOfWork.GetRepository<TaxRule>();
        taxRuleRepository.Update(taxRule);
        await _taxUnitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var taxRuleRepository = _taxUnitOfWork.GetRepository<TaxRule>();
        var taxRule = await taxRuleRepository.GetByIdAsync(id);
        if (taxRule is null)
        {
            return;
        }
        taxRuleRepository.Delete(id);

        await _taxUnitOfWork.SaveAsync();
    }
}
