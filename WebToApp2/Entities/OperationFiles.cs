using System.ComponentModel.DataAnnotations;

namespace WebToApp2.Entities;

public class OperationFile
{
    [Key]
    public int Id { get; set; }
    public int OperationId { get; set; }
    public int FileId { get; set; }
    
    public virtual Operation Operation { get; set; } = null!;
    public virtual AppFile AppFile { get; set; } = null!;
}