using System.ComponentModel.DataAnnotations;

namespace UncafezinEntities;

public class Category
{
    [Key]
    public int Code { get; set; }
    public string Description { get; set; }
    public ICollection<Product> Products { get; set; }
}
