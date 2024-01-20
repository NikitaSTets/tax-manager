using Models;

namespace TaxManager.Models;

public static class Seeder
{
    public static void Seed(this TaxContext taxContext)
    {
        if (!taxContext.Cities.Any())
        {
            var taxRules = new List<TaxRule>
            {
                new TaxRule
                {
                    FromDate = new DateTime(2024, 1, 1),
                    ToDate = new DateTime(2024, 12, 31),
                    Tax = 3.3
                },
                new TaxRule
                {
                    FromDate = new DateTime(2024, 6, 1),
                    ToDate = new DateTime(2024, 6, 30),
                    Tax = 5
                },
                new TaxRule
                {
                    FromDate = new DateTime(2024, 7, 1),
                    ToDate = new DateTime(2024, 7, 31),
                    Tax = 4
                },
                new TaxRule
                {
                    FromDate = new DateTime(2024, 8, 1),
                    ToDate = new DateTime(2024, 8, 31),
                    Tax = 6
                },
                new TaxRule
                {
                    FromDate = new DateTime(2024, 2, 9),
                    ToDate = new DateTime(2024, 2, 15),
                    Tax = 2.5
                },
                new TaxRule
                {
                    FromDate = new DateTime(2024, 3, 2),
                    ToDate = new DateTime(2024, 3, 8),
                    Tax = 2.5
                },
                new TaxRule
                {
                    FromDate = new DateTime(2024, 6, 1),
                    ToDate = new DateTime(2024, 6, 1),
                    Tax = 1.5
                },
                new TaxRule
                {
                    FromDate = new DateTime(2024, 10, 23),
                    ToDate = new DateTime(2024, 10, 23),
                    Tax = 1.2
                },
            };

            var cities = new List<City>() {
                new City { Name = "Kaunas", TaxRules  = taxRules}
            };

            taxContext.AddRange(cities);
            taxContext.SaveChanges();
        }
    }
}
