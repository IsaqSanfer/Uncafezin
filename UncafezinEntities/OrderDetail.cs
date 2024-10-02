namespace UncafezinEntities;

public class OrderDetail
{
    public int CodeOrder{ get; set; }
    public int CodeProduct { get; set; }
    public double Quantity { get; set; }
    public decimal UnitValue { get; set; }
    public decimal TotalValue { get; set; }

    public Product Product { get; set; }
    public Order Order { get; set; }
}
