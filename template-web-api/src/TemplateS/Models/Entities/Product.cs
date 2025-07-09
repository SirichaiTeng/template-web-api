namespace template_web_api.Models.Entities;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Balance { get; set; }
    public decimal? Fee { get; set; }
    public decimal? Gross { get; set; }
    public decimal? GrossAmount { get; set; }
    public string? Category { get; set; }
    public int Stock { get; set; }
}
