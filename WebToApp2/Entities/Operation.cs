using System.ComponentModel.DataAnnotations;
using WebToApp2.Enum;

namespace WebToApp2.Entities;

public class Operation
{
    public Operation()
    {
        OperationFiles = new HashSet<OperationFile>();
    }
    [Key]
    public int Id { get; set; }

    public string QrSignature { get; set; }
    public string QrOperationId { get; set; }

    public GetFileResponseTypeEnum GetFileResponseType { get; set; }
    
    public virtual ICollection<OperationFile> OperationFiles { get; set; }
}