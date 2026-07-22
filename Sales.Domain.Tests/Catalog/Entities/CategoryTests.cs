using FluentAssertions;
using Sales.Domain.Catalog.Entities;
using Sales.Domain.Common.Exceptions;
using Xunit;

namespace Sales.Domain.Tests;

public class CategoryTests
{
    [Fact]
    public void CreateCategory_ShouldBeCreatedWithValidData()
    {
        // Arrange
        var name = "Teste";

        // Act
        var category = new Category(name);

        // Assert
        category.Name.Should().Be(name);
        category.Description.Should().BeNull();
        category.Active.Should().BeTrue();
        category.CreateDate.Should().NotBe(default);
        category.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void CreateCategory_ShouldThrowException_WhenInvalidName()
    {
        // Arrange
        string name = "ab";

        // Act
        Action act = () => new Category(name);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Nome deve ter no mínimo 3 caracteres.");
    }

    [Fact]
    public void AlterName_ShouldUpdateName_WhenValidData()
    {
        // Arrange
        var category = new Category("Souveniers");
        var newName = "Novo Nome";

        // Act
        category.AlterName(newName);

        // Assert
        category.Name.Should().Be(newName);
        category.UpdateDate.Should().NotBeNull();
    }
}
