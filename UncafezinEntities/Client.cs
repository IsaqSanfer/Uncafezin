using System.ComponentModel.DataAnnotations;

namespace UncafezinEntities;

public class Client
{
    [Key]
    public int Code { get; set; }
    public string Name { get; set; }
    public string CNPJ_CPF { get; set; }
    public string Email { get; set; }
    public string Cellphone { get; set; }

    public ICollection<Order> Orders { get; set; }
}
