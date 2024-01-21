using Models;

namespace Services.Interfaces;

public interface ITaxRuleService : IEntityService<TaxRule>
{
    Task<IEnumerable<TaxRule>> GetCityTaxRuleByDate(int cityId, DateTime date);

    Task UpdateAsync(TaxRule taxRule);

    Task DeleteAsync(int id);
}
