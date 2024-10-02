using System.ComponentModel.DataAnnotations;

namespace UncafezinWeb.Models;

public class ClientViewModel
{
    public int Code { get; set; }


    [Required(ErrorMessage = "Informe o Nome do Cliente!")]
    public string Name { get; set; }


    [Required(ErrorMessage = "Informe o CNPJ/CPF do Cliente!")]
    public string CNPJ_CPF { get; set; }


    [Required(ErrorMessage = "Informe o Email do Cliente!")]
    public string Email { get; set; }


    [Required(ErrorMessage = "Informe o celular do Cliente!")]
    public string Cellphone { get; set; }
}
