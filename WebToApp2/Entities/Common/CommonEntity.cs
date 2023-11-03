using WebToApp2.Helpers;

namespace WebToApp2.Entities;

public abstract class CommonEntity : BaseEntity, IAuditedEntity
{
    public DateTime CreatedDate { get; set; } = DateTimeUtility.BakuLocalTime();
    public int CreatedById { get; set; }

    public DateTime? UpdatedDate { get; set; } = DateTimeUtility.BakuLocalTime();
    public int? UpdatedById { get; set; }
}