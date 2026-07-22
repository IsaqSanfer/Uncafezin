using Sales.Domain.Catalog.Enums;
using Sales.Domain.Catalog.Events;
using Sales.Domain.Catalog.ValueObjects;
using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Catalog.Entities;

public sealed class Product : AggregateRoot
{
    public ProductName Name { get; private set; }
    public ProductCode Code { get; private set; }
    public ProductPrice Price { get; private set; }
    public string? Description { get; private set; }
    public Guid CategoryId { get; private set; }
    public ProductStatus Status { get; private set; }
    public int Stock { get; private set; }

    private readonly List<ProductImage> _images = new();
    public IReadOnlyCollection<ProductImage> Images => _images.AsReadOnly();

    public Product(ProductName name, ProductCode code, ProductPrice price, Guid categoryId, int initialStock = 0, string? description = null)
    {
        Guard.AgainstNull(name, nameof(name));
        Guard.AgainstNull(code, nameof(code));
        Guard.AgainstNull(price, nameof(price));
        Guard.AgainstEmptyGuid(categoryId, nameof(categoryId));
        Guard.Against<DomainException>(initialStock < 0, "O estoque inicial não pode ser negativo.");

        Name = name;
        Code = code;
        Price = price;
        CategoryId = categoryId;
        Description = description?.Trim();
        Stock = initialStock;
        Status = ProductStatus.Active;
    }

    public void AlterName(ProductName newName)
    {
        Guard.AgainstNull(newName, nameof(newName));

        Name = newName;
        SetUpdateDate();
    }

    public void AlterPrice(ProductPrice newPrice)
    {
        Guard.AgainstNull(newPrice, nameof(newPrice));

        var oldPriceValue = Price.Value;
        var newPriceValue = newPrice.Value;

        Price = newPrice;
        SetUpdateDate();

        AddDomainEvent(new ProductPriceAlteredEvent(Id, oldPriceValue, newPriceValue));
    }

    public void AlterCategory(Guid newCategoryId)
    {
        Guard.AgainstEmptyGuid(newCategoryId, nameof(newCategoryId));

        CategoryId = newCategoryId;
        SetUpdateDate();
    }

    public void AlterDescription(string? newDescription)
    {
        Description = newDescription?.Trim();
        SetUpdateDate();
    }

    public void AdjustStock(int quantity, string reason)
    {
        Guard.AgainstNullOrWhiteSpace(reason, nameof(reason));
        Guard.Against<DomainException>(Stock + quantity < 0, "O estoque não pode ser negativo.");

        Stock += quantity;
        SetUpdateDate();

        AddDomainEvent(new ProductStockAdjustedEvent(Id, quantity, reason));
    }

    public void AddImage(ProductImage image)
    {
        Guard.AgainstNull(image, nameof(image));
        Guard.Against<DomainException>(_images.Any(i => i.Position == image.Position), "Já existe uma imagem com a mesma posição.");

        _images.Add(image);
        SetUpdateDate();

        AddDomainEvent(new ProductImageAddedEvent(Id, image.Url, image.Position));
    }

    public void Activate()
    {
        Guard.Against<DomainException>(Status == ProductStatus.Active, "O produto já está ativo.");

        Status = ProductStatus.Active;
        SetUpdateDate();

        AddDomainEvent(new ProductActivatedEvent(Id));
    }

    public void Inactivate()
    {
        Guard.Against<DomainException>(Status == ProductStatus.Inactive, "O produto já está inativo.");

        Status = ProductStatus.Inactive;
        SetUpdateDate();

        AddDomainEvent(new ProductInactivatedEvent(Id));
    }
}
