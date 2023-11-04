using System.ComponentModel.DataAnnotations;

namespace WebToApp2.Entities
{
    public class AppFile
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string Extension { get; set; } = null!;
        
        public virtual ICollection<OperationFile> OperationFiles { get; set; }

    }
}
