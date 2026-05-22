using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;
using Sales.Domain.Customers.Enums;
using Sales.Domain.Customers.Events;
using Sales.Domain.Customers.ValueObjects;

namespace Sales.Domain.Customers.Entities;

public sealed class Customer : AggregateRoot
{
    public CompleteName Name { get; private set; }
    public CPF Cpf { get; private set; }
    public Email Email { get; private set; }
    public Phone Phone { get; private set; }
    public CustomerStatus Status { get; private set; }
    public Sex Sex { get; private set; }
    public MaritalStatus MaritalStatus { get; private set; }

    public Guid PrimaryAddressId { get; private set; }
    private readonly List<Address> _addresses = new();
    public IReadOnlyCollection<Address> Addresses => _addresses.AsReadOnly();

    public Customer(CompleteName name, CPF cpf, Email email, Phone phone, Address primaryAddress, Sex sex = Sex.NotSpecified, MaritalStatus maritalStatus = MaritalStatus.None)
    {
        Validate(name, cpf, email, phone, primaryAddress);

        Name = name;
        Cpf = cpf;
        Email = email;
        Phone = phone;
        Status = CustomerStatus.Active;

        Sex = sex;
        MaritalStatus = maritalStatus;

        _addresses.Add(primaryAddress);
        PrimaryAddressId = primaryAddress.Id;

        AddDomainEvent(new CustomerRegisteredEvent(CustomerId: Id, Name: Name.FullName, Cpf: Cpf.Number, Email: Email.Address));
    }

    public void AddAddress(Address address)
    {
        Guard.AgainstNull(address, nameof(address), "Endereço inválido.");
        _addresses.Add(address);
        SetUpdateDate();
    }

    public void RemoveAddress(Guid addressId)
    {
        var address = _addresses.FirstOrDefault(a => a.Id == addressId);

        Guard.AgainstNull(address, nameof(addressId), "Endereço não encontrado.");
        Guard.Against<DomainException>(_addresses.Count == 1, "O cliente deve ter pelo menos um endereço.");

        _addresses.Remove(address!);

        // Se o endereço removido for o principal, definir outro endereço automaticamente
        if (addressId == PrimaryAddressId)
        {
            PrimaryAddressId = _addresses.First().Id;

            AddDomainEvent(new PrimaryAddressChangedEvent(CustomerId: Id, NewAddressId: PrimaryAddressId));  
        }

        SetUpdateDate();
    }

    public void ChangeAddress(Guid addressId, string postalCode, string street, string number, string district, string city, string state, string country, string complement = "")
    {
        var address = _addresses.FirstOrDefault(a => a.Id == addressId);

        Guard.AgainstNull(address, nameof(address), "Endereço não encontrado.");

        address!.Update(postalCode, street, number, district, city, state, country, complement);

        SetUpdateDate();
    }

    public void SetPrimaryAddress(Guid addressId)
    {
        var address = _addresses.FirstOrDefault(a => a.Id == addressId);

        Guard.AgainstNull(address, nameof(address), "Endereço não encontrado.");

        PrimaryAddressId = address!.Id;

        AddDomainEvent(new PrimaryAddressChangedEvent(CustomerId: Id, NewAddressId: PrimaryAddressId));

        SetUpdateDate();
    }

    public Address GetPrimaryAddress()
    {
        return _addresses.First(a => a.Id == PrimaryAddressId);
    }

    public void UpdateProfile(CompleteName name, Email email, Phone phone, Sex sex, MaritalStatus maritalStatus)
    {
        Guard.Against<DomainException>(Status == CustomerStatus.Blocked, "Não é possível atualizar o perfil de um cliente bloqueado.");

        Guard.AgainstNull(name, nameof(name));
        Guard.AgainstNull(email, nameof(email));
        Guard.AgainstNull(phone, nameof(phone));

        Name = name;
        Email = email;
        Phone = phone;

        Sex = sex;
        MaritalStatus = maritalStatus;

        SetUpdateDate();
    }

    public void Block()
    {
        if (Status == CustomerStatus.Blocked)
            return;

        // Guard.Against<DomainException>(Status == CustomerStatus.Blocked, "O cliente já está bloqueado.");
        Status = CustomerStatus.Blocked;

        AddDomainEvent(new CustomerBlockedEvent(CustomerId: Id, Cpf: Cpf.Number));
        SetUpdateDate();
    }

    public void Activate()
    {
        Status = CustomerStatus.Active;
        SetUpdateDate();
    }

    private static void Validate(CompleteName name, CPF cpf, Email email, Phone phone, Address primaryAddress)
    {
        Guard.AgainstNull(name, nameof(name));
        Guard.AgainstNull(cpf, nameof(cpf));
        Guard.AgainstNull(email, nameof(email));
        Guard.AgainstNull(phone, nameof(phone));
        Guard.AgainstNull(primaryAddress, nameof(primaryAddress), "O cliente deve ter um endereço principal.");
    }
}
