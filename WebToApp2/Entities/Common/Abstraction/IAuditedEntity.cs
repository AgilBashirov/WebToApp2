namespace WebToApp2.Entities;

public interface IAuditedEntity : ICreatedDateEntity, ICreatedByEntity, IUpdatedByEntity, IUpdatedDateEntity
{
}