using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UncafezinEntities;

public class Order
{
    [Key]
    public int Code { get; set; }
    public DateTime Date { get; set; }

    [ForeignKey("Client")]
    public int CodeClient { get; set; }
    public Client Client { get; set; } 
    public decimal Total { get; set; }

    public ICollection<OrderDetail> Products { get; set; }
}
