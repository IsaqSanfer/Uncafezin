using System.ComponentModel.DataAnnotations;

namespace UncafezinWeb.Models;

public class CategoryViewModel
{
    public int Code { get; set; }

    [Required(ErrorMessage="Informe a Descrição da Categoria!")]
    public string Description { get; set; }
}
