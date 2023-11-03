namespace WebToApp2.Entities;

public interface ISoftDeletedEntity
{
    public bool Deleted { get; set; }
}