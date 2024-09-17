using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UncafezinEntities;

public class Product
{
    [Key]
    public int Code { get; set; }
    //public string Name { get; set; }
    public string Description { get; set; }
    public double Quantity { get; set; }
    public decimal Price { get; set; }

    [ForeignKey("Category")]
    public int CodeCategory { get; set; }
    public Category Category { get; set; }

    public ICollection<OrderDetail> Orders { get; set; }
}
