using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
