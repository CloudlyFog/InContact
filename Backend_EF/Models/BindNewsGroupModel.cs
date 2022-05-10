namespace Backend_EF.Models
{
    public class BindNewsGroupModel
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid PostID { get; set; } = new();
        public Guid GroupID { get; set; } = new();
    }
}
