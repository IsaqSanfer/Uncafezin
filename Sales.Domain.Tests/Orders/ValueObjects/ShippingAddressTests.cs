using FluentAssertions;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Orders.ValueObjects;
using Xunit;

namespace Sales.Domain.Tests.Orders.ValueObjects;

public class ShippingAddressTests
{
    [Fact(DisplayName = "Criar endereço de entrega (ShippingAddress) com sucesso onde todos os dados são válidos")]
    public void CreateShippingAddress_ShouldReturnShippingAddress()
    {
        // Arrange
        var postalCode = "01007-040";
        var street = "Rua Santa Ifigênia";
        var number = "100";
        var complement = "Apt 4B";
        var district = "Santa Ifigênia";
        var city = "São Paulo";
        var state = "SP";
        var country = "Brasil";

        // Act
        var shippingAddress = ShippingAddress.Create(postalCode, street, number, complement, district, state, city, country);

        // Assert
        shippingAddress.Should().NotBeNull();
        shippingAddress.PostalCode.Should().Be(postalCode);
        shippingAddress.Street.Should().Be(street);
        shippingAddress.Number.Should().Be(number);
        shippingAddress.Complement.Should().Be(complement);
        shippingAddress.District.Should().Be(district);
        shippingAddress.City.Should().Be(city);
        shippingAddress.State.Should().Be(state);
        shippingAddress.Country.Should().Be(country);
        shippingAddress.FullAddress().Should().Be($"{street}, {number}, {complement} - {district}, {city}/{state}, {country} - CEP: {postalCode}");
    }

    [Xunit.Theory(DisplayName = "Criar endereço de entrega (ShippingAddress) com falha quando o CEP é inválido")]
    [InlineData("", "CEP não pode ser nulo ou vazio.")]
    [InlineData(null, "CEP não pode ser nulo ou vazio.")]
    [InlineData("1234", "CEP inválido. O formato deve ser 00000-000 ou 00000000.")]
    [InlineData("12-3456", "CEP inválido. O formato deve ser 00000-000 ou 00000000.")]
    public void CreateShippingAddress_ShouldThrowDomainException_WhenPostalCodeIsInvalid(string postalCode, string message)
    {
        // Arrange
        var street = "Rua Santa Ifigênia";
        var number = "100";
        var complement = "Apt 4B";
        var district = "Santa Ifigênia";
        var city = "São Paulo";
        var state = "SP";
        var country = "Brasil";

        // Act
        Action act = () => ShippingAddress.Create(postalCode, street, number, complement, district, state, city, country);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(message);
    }

    [Fact(DisplayName = "Dois endereços de entrega (ShippingAddress) com os mesmos dados devem ser iguais")]
    public void ShippingAddress_ShouldBeEqual_WhenSameData()
    {
        // Arrange
        var shippingAddress1 = ShippingAddress.Create("01007-040", "Rua Santa Ifigênia", "100", "Apt 4B", "Santa Ifigênia", "SP", "São Paulo", "Brasil");
        var shippingAddress2 = ShippingAddress.Create("01007-040", "Rua Santa Ifigênia", "100", "Apt 4B", "Santa Ifigênia", "SP", "São Paulo", "Brasil");

        // Assert
        shippingAddress1.Should().Be(shippingAddress2);
        (shippingAddress1 == shippingAddress2).Should().BeTrue();
    }

    [Fact(DisplayName = "Dois endereços de entrega (ShippingAddress) com dados diferentes não devem ser iguais")]
    public void ShippingAddress_ShouldNotBeEqual_WhenDifferentData()
    {
        // Arrange
        var shippingAddress1 = ShippingAddress.Create("01007-040", "Rua Santa Ifigênia", "100", "Apt 4B", "Santa Ifigênia", "SP", "São Paulo", "Brasil");
        var shippingAddress2 = ShippingAddress.Create("01007-050", "Rua Santa Ifigênia", "200", "Suite 5C", "Santa Ifigênia", "SP", "São Paulo", "Brasil");

        // Assert
        shippingAddress1.Should().NotBe(shippingAddress2);
        (shippingAddress1 != shippingAddress2).Should().BeTrue();
    }

    [Fact(DisplayName = "Endereço de Entrega (ShippingAddress) deve ser imutável após a criação")]
    public void ShippingAddress_ShouldBeImmutable()
    {
        // Arrange
        var shippingAddress = ShippingAddress.Create("01007-040", "Rua Santa Ifigênia", "100", "Apt 4B", "Santa Ifigênia", "SP", "São Paulo", "Brasil");

        // Act 
        Action act = () => {
            // Tentar modificar uma propriedade do endereço de entrega
            //shippingAddress.PostalCode = "54321";
        };

        // Assert
        shippingAddress.GetType().GetProperties().All(p => p.SetMethod == null || p.SetMethod.IsPrivate).Should().BeTrue("As propriedades do endereço de entrega devem ser imutáveis após a criação");
    }

    [Xunit.Theory(DisplayName = "Criar endereço de entrega (ShippingAddress) com falha quando um campo obrigatório é nulo ou vazio")]
    [InlineData(null, "Rua Santa Ifigênia", "123", "Apt 4B", "Santa Ifigênia", "SP", "São Paulo", "Brasil", "CEP não pode ser nulo ou vazio.")]
    [InlineData("01007-040", null, "123", "Apt 4B", "Santa Ifigênia", "SP", "São Paulo", "Brasil", "Rua não pode ser nulo ou vazio.")]
    [InlineData("01007-040", "Rua Santa Ifigênia", null, "Apt 4B", "Santa Ifigênia", "SP", "São Paulo", "Brasil", "Número não pode ser nulo ou vazio.")]
    [InlineData("01007-040", "Rua Santa Ifigênia", "123", "Apt 4B", null, "SP", "São Paulo", "Brasil", "Bairro não pode ser nulo ou vazio.")]
    [InlineData("01007-040", "Rua Santa Ifigênia", "123", "Apt 4B", "Santa Ifigênia", null, "São Paulo", "Brasil", "Estado não pode ser nulo ou vazio.")]
    [InlineData("01007-040", "Rua Santa Ifigênia", "123", "Apt 4B", "Santa Ifigênia", "SP", null, "Brasil", "Cidade não pode ser nulo ou vazio.")]
    [InlineData("01007-040", "Rua Santa Ifigênia", "123", "Apt 4B", "Santa Ifigênia", "SP", "São Paulo", null, "País não pode ser nulo ou vazio.")]
    public void CreateShippingAddress_ShouldThrowDomainException_WhenRequiredDataIsNullOrEmpty(string postalCode, string street, string number, string complement, string district, string state, string city, string country, string message)
    {
        // Act
        Action act = () => ShippingAddress.Create(postalCode, street, number, complement, district, state, city, country);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(message);
    }

    [Fact(DisplayName = "Criar endereço de entrega (ShippingAddress) com complemento nulo deve substituir por vazio")]
    public void CreateShippingAddress_ShouldAllowNullComplement()
    {
        // Act
        var shippingAddress = ShippingAddress.Create("01007-040", "Rua Santa Ifigênia", "100", null, "Santa Ifigênia", "SP", "São Paulo", "Brasil");

        // Assert
        shippingAddress.Complement.Should().BeEmpty();
    }

    [Fact(DisplayName = "Verificar se o método FullAddress retorna o endereço completo corretamente")]
    public void FullAddress_ShouldReturnCompleteAddress()
    {
        // Arrange
        var postalCode = "01007-040";
        var street = "Rua Santa Ifigênia";
        var number = "100";
        var complement = "Apt 4B";
        var district = "Santa Ifigênia";
        var city = "São Paulo";
        var state = "SP";
        var country = "Brasil";
        var shippingAddress = ShippingAddress.Create(postalCode, street, number, complement, district, state, city, country);

        // Act
        var fullAddress = shippingAddress.FullAddress();

        // Assert
        fullAddress.Should().Be($"{street}, {number}, {complement} - {district}, {city}/{state}, {country} - CEP: {postalCode}");
    }
}
