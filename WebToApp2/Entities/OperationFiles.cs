using System.ComponentModel.DataAnnotations;

namespace WebToApp2.Entities;

public class OperationFile
{
    [Key]
    public int Id { get; set; }

    public bool IsSigned { get; set; } = false;
    public int OperationId { get; set; }
    public virtual Operation Operation { get; set; }
    public int AppFileId { get; set; }
    public virtual AppFile AppFile { get; set; }
}