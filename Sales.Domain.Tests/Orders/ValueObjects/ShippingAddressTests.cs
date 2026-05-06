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
        var postalCode = "12345-678";
        var street = "123 Main St";
        var complement = "Apt 4B";
        var district = "Downtown";
        var city = "Anytown";
        var state = "CA";
        var country = "USA";

        // Act
        var shippingAddress = ShippingAddress.Create(postalCode, street, complement, district, state, city, country);

        // Assert
        shippingAddress.Should().NotBeNull();
        shippingAddress.PostalCode.Should().Be(postalCode);
        shippingAddress.Street.Should().Be(street);
        shippingAddress.Complement.Should().Be(complement);
        shippingAddress.District.Should().Be(district);
        shippingAddress.City.Should().Be(city);
        shippingAddress.State.Should().Be(state);
        shippingAddress.Country.Should().Be(country);
        shippingAddress.FullAddress().Should().Be($"{street}, {complement} - {district}, {city}/{state}, {country} - CEP: {postalCode}");
    }

    [Xunit.Theory(DisplayName = "Criar endereço de entrega (ShippingAddress) com falha quando o CEP é inválido")]
    [InlineData("", "PostalCode não pode ser nulo ou vazio.")]
    [InlineData(null, "PostalCode não pode ser nulo ou vazio.")]
    [InlineData("1234", "CEP inválido. O formato deve ser 00000-000 ou 00000000.")]
    [InlineData("12-3456", "CEP inválido. O formato deve ser 00000-000 ou 00000000.")]
    public void CreateShippingAddress_ShouldThrowDomainException_WhenPostalCodeIsInvalid(string postalCode, string message)
    {
        // Arrange
        var street = "123 Main St";
        var complement = "Apt 4B";
        var district = "Downtown";
        var city = "Anytown";
        var state = "CA";
        var country = "USA";

        // Act
        Action act = () => ShippingAddress.Create(postalCode, street, complement, district, state, city, country);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(message);
    }

    [Fact(DisplayName = "Dois endereços de entrega (ShippingAddress) com os mesmos dados devem ser iguais")]
    public void ShippingAddress_ShouldBeEqual_WhenSameData()
    {
        // Arrange
        var shippingAddress1 = ShippingAddress.Create("12345-678", "123 Main St", "Apt 4B", "Downtown", "CA", "Anytown", "USA");
        var shippingAddress2 = ShippingAddress.Create("12345-678", "123 Main St", "Apt 4B", "Downtown", "CA", "Anytown", "USA");

        // Assert
        shippingAddress1.Should().Be(shippingAddress2);
        (shippingAddress1 == shippingAddress2).Should().BeTrue();
    }

    [Fact(DisplayName = "Dois endereços de entrega (ShippingAddress) com dados diferentes não devem ser iguais")]
    public void ShippingAddress_ShouldNotBeEqual_WhenDifferentData()
    {
        // Arrange
        var shippingAddress1 = ShippingAddress.Create("12345-678", "123 Main St", "Apt 4B", "Downtown", "CA", "Anytown", "USA");
        var shippingAddress2 = ShippingAddress.Create("54321-876", "456 Elm St", "Suite 5C", "Uptown", "NY", "Othertown", "USA");

        // Assert
        shippingAddress1.Should().NotBe(shippingAddress2);
        (shippingAddress1 != shippingAddress2).Should().BeTrue();
    }

    [Fact(DisplayName = "Endereço de Entrega (ShippingAddress) deve ser imutável após a criação")]
    public void ShippingAddress_ShouldBeImmutable()
    {
        // Arrange
        var shippingAddress = ShippingAddress.Create("12345-678", "123 Main St", "Apt 4B", "Downtown", "CA", "Anytown", "USA");

        // Act 
        Action act = () => {
            // Tentar modificar uma propriedade do endereço de entrega
            //shippingAddress.PostalCode = "54321";
        };

        // Assert
        shippingAddress.GetType().GetProperties().All(p => p.SetMethod == null || p.SetMethod.IsPrivate).Should().BeTrue("As propriedades do endereço de entrega devem ser imutáveis após a criação");
    }

    [Xunit.Theory(DisplayName = "Criar endereço de entrega (ShippingAddress) com falha quando um campo obrigatório é nulo ou vazio")]
    [InlineData(null, "123 Main St", "Apt 4B", "Downtown", "CA", "Anytown", "USA", "PostalCode não pode ser nulo ou vazio.")]
    [InlineData("12345-678", null, "Apt 4B", "Downtown", "CA", "Anytown", "USA", "Street não pode ser nulo ou vazio.")]
    [InlineData("12345-678", "123 Main St", "Apt 4B", null, "CA", "Anytown", "USA", "District não pode ser nulo ou vazio.")]
    [InlineData("12345-678", "123 Main St", "Apt 4B", "Downtown", null, "Anytown", "USA", "State não pode ser nulo ou vazio.")]
    [InlineData("12345-678", "123 Main St", "Apt 4B", "Downtown", "CA", null, "USA", "City não pode ser nulo ou vazio.")]
    [InlineData("12345-678", "123 Main St", "Apt 4B", "Downtown", "CA", "Anytown", null, "Country não pode ser nulo ou vazio.")]
    public void CreateShippingAddress_ShouldThrowDomainException_WhenRequiredDataIsNullOrEmpty(string postalCode, string street, string complement, string district, string state, string city, string country, string message)
    {
        // Act
        Action act = () => ShippingAddress.Create(postalCode, street, complement, district, state, city, country);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(message);
    }

    [Fact(DisplayName = "Criar endereço de entrega (ShippingAddress) com complemento nulo deve substituir por vazio")]
    public void CreateShippingAddress_ShouldAllowNullComplement()
    {
        // Act
        var shippingAddress = ShippingAddress.Create("12345-678", "123 Main St", null, "Downtown", "CA", "Anytown", "USA");

        // Assert
        shippingAddress.Complement.Should().BeEmpty();
    }

    [Fact(DisplayName = "Verificar se o método FullAddress retorna o endereço completo corretamente")]
    public void FullAddress_ShouldReturnCompleteAddress()
    {
        // Arrange
        var postalCode = "12345-678";
        var street = "123 Main St";
        var complement = "Apt 4B";
        var district = "Downtown";
        var city = "Anytown";
        var state = "CA";
        var country = "USA";
        var shippingAddress = ShippingAddress.Create(postalCode, street, complement, district, state, city, country);

        // Act
        var fullAddress = shippingAddress.FullAddress();

        // Assert
        fullAddress.Should().Be($"{street}, {complement} - {district}, {city}/{state}, {country} - CEP: {postalCode}");
    }
}
