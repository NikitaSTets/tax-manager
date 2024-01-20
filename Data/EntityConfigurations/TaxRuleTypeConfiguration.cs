using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models;

namespace EFCore.TaxDb.EntityConfigurations
{
    internal class TaxRuleTypeConfiguration : IEntityTypeConfiguration<TaxRule>
    {
        public void Configure(EntityTypeBuilder<TaxRule> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
