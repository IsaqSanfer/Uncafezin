using FluentAssertions;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Customers.Entities;
using Xunit;

namespace Sales.Domain.Tests.Customers.Entities;

public class AddressTests
{
    private static Address CreateValidAddress()
    {
        return new Address(
            postalCode: "12345678",
            street: "123 Main St",
            number: "123",
            district: "Downtown",
            city: "Anytown",
            state: "CA",
            country: "USA"
        //complement: "Apt 4B"
        );
    }

    [Fact(DisplayName = "Deve criar um endereço válido")]
    public void Create_ValidAddress_ShouldSucceed()
    {
        // Arrange & Act
        var address = CreateValidAddress();

        // Assert
        address.PostalCode.Should().Be("12345678");
        address.Street.Should().Be("123 Main St");
        address.Number.Should().Be("123");
        address.District.Should().Be("Downtown");
        address.City.Should().Be("Anytown");
        address.State.Should().Be("CA");
        address.Country.Should().Be("USA");
        address.Complement.Should().BeEmpty();
    }

    //[Fact(DisplayName = "Deve lançar exceção quando o CEP for inválido")]
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_InvalidPostalCode_ShouldThrowException(string? invalidPostalCode)
    {
        // Arrange & Act
        Action act = () => new Address(
            postalCode: invalidPostalCode!,
            street: "123 Main St",
            number: "123",
            district: "Downtown",
            city: "Anytown",
            state: "CA",
            country: "USA"
        );

        // Assert
        act.Should().Throw<DomainException>().WithMessage("O CEP é obrigatório.");
    }

    [Fact(DisplayName = "Deve lançar exceção quando o CEP não tiver 8 dígitos")]
    public void Create_InvalidPostalCodeLength_ShouldThrowException()
    {
        // Arrange & Act
        Action act = () => new Address(
            postalCode: "1234567",
            street: "123 Main St",
            number: "123",
            district: "Downtown",
            city: "Anytown",
            state: "CA",
            country: "USA"
        );

        // Assert
        act.Should().Throw<DomainException>().WithMessage("CEP inválido.");
    }

    //[Fact(DisplayName = "Deve lançar exceção quando um campo obrigatório for inválido")]
    [Theory]
    [InlineData(null, "123", "Downtown", "Anytown", "CA", "USA")]
    [InlineData("123 Main St", null, "Downtown", "Anytown", "CA", "USA")]
    [InlineData("123 Main St", "123", null, "Anytown", "CA", "USA")]
    [InlineData("123 Main St", "123", "Downtown", null, "CA", "USA")]
    [InlineData("123 Main St", "123", "Downtown", "Anytown", null, "USA")]
    [InlineData("123 Main St", "123", "Downtown", "Anytown", "CA", null)]
    public void Create_MissingRequiredField_ShouldThrowException(
        string? street,
        string? number,
        string? district,
        string? city,
        string? state,
        string? country)
    {
        // Arrange & Act
        Action act = () => new Address(
            postalCode: "12345678",
            street: street!,
            number: number!,
            district: district!,
            city: city!,
            state: state!,
            country: country!
        );

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact(DisplayName = "Deve atualizar um endereço válido")]
    public void Update_ValidAddress_ShouldSucceed()
    {
        // Arrange
        var address = CreateValidAddress();

        // Act
        address.Update(
            postalCode: "87654321",
            street: "456 Elm St",
            number: "456",
            district: "Uptown",
            city: "Othertown",
            state: "NY",
            country: "USA",
            complement: "Suite 5C"
        );

        // Assert
        address.PostalCode.Should().Be("87654321");
        address.Street.Should().Be("456 Elm St");
        address.Number.Should().Be("456");
        address.District.Should().Be("Uptown");
        address.City.Should().Be("Othertown");
        address.State.Should().Be("NY");
        address.Country.Should().Be("USA");
        address.Complement.Should().Be("Suite 5C");
    }

    [Fact(DisplayName = "Deve lançar exceção ao atualizar com um CEP inválido")]
    public void Update_InvalidPostalCode_ShouldThrowException()
    {
        // Arrange
        var address = CreateValidAddress();

        // Act
        Action act = () => address.Update(
            postalCode: "12345",
            street: "456 Elm St",
            number: "456",
            district: "Uptown",
            city: "Othertown",
            state: "NY",
            country: "USA",
            complement: "Suite 5C"
        );

        // Assert
        act.Should().Throw<DomainException>().WithMessage("CEP inválido.");
    }

    [Fact(DisplayName = "Deve lançar exceção ao atualizar com um campo obrigatório inválido")]
    public void Update_MissingRequiredField_ShouldThrowException()
    {
        // Arrange
        var address = CreateValidAddress();
        // Act
        Action act = () => address.Update(
            postalCode: "87654321",
            street: null!,
            number: "456",
            district: "Uptown",
            city: "Othertown",
            state: "NY",
            country: "USA",
            complement: "Suite 5C"
        );

        // Assert
        act.Should().Throw<DomainException>();
    }
}