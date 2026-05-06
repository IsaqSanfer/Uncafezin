using Sales.Domain.Orders.Events;

namespace Sales.Domain.Common.Base;

public abstract class Entity
{
    // Protected set para que apenas a própria classe ou classes derivadas possam definir o Id, imutabilidade garantida
    public Guid Id { get; protected set; }
    public DateTime CreateDate { get; protected set; }
    public DateTime? UpdateDate { get; protected set; }

    // Construtor protegido para garantir que as entidades sejam criadas via Construtor ou Factory, e não "apenas" instanciadas diretamente
    protected Entity()
    {
        Id = Guid.NewGuid();          // Gera um novo Id único para cada entidade criada
        CreateDate = DateTime.UtcNow; // Define a data de criação como o momento atual em UTC
    }
    protected void SetUpdateDate()
    {
        UpdateDate = DateTime.UtcNow; // Define a data de atualização como o momento atual em UTC
    }
    protected Entity(Guid id)
    {
        // Validar id   // Guard.Against.NullOrEmpty(id, nameof(id));

        Id = id;
        //CreateDate = DateTime.UtcNow; // Define a data de criação como o momento atual em UTC
    }
    protected Entity(Guid id, DateTime createDate, DateTime? updateDate)
    {
        Id = id;
        CreateDate = createDate;
        UpdateDate = updateDate;
    }

    // Sobrescreve o método Equals para comparar entidades com base no Id
    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        // Duas entidades são consideradas iguais se tiverem o mesmo Id
        return Id.Equals(other.Id);
    }    
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    public static bool operator ==(Entity left, Entity right)
    {
        if (ReferenceEquals(left, null)) 
            return ReferenceEquals(right, null);

        return left.Equals(right);
    }
    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }

    // Eventos de domínio
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}
