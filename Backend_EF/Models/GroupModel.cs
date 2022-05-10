using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_EF.Models
{
    public class GroupModel
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid OwnerID { get; set; } = new();
        public Guid UserID { get; set; } = new();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [NotMapped]
        public NewsModel? NewsModel { get; set; }

    }
}
