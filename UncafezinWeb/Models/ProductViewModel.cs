using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace UncafezinWeb.Models;

public class ProductViewModel
{
    public int Code { get; set; }

    //[Required(ErrorMessage = "Informe o Nome do Produto")]
    //public string Name { get; set; }

    [Required(ErrorMessage = "Informe a Descrição do Produto")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Informe a Quantidade do Produto")]
    public double Quantity { get; set; }

    [Required(ErrorMessage = "Informe o Valor Unitário do Produto")]
    [Range(0.1, Double.MaxValue)]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Informe a Categoria do Produto")]
    public int? CodeCategory { get; set; }

    public IEnumerable<SelectListItem> CategoryList { get; set; }
}
