namespace Models;

public class TaxRule
{
    public int Id { get; set; }
    
    public int CityId { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public double Tax { get; set; }
}
