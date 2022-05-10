namespace Backend_EF.Models
{
    public class GroupsNodeOwner
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid OwnerID { get; set; } = new();
        public Guid GroupID { get; set; } = new();
    }
}
