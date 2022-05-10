

namespace Backend_EF.Models
{
    public class GroupsMembers
    {
        public Guid ID { get; set; } = new();
        public Guid UserID { get; set; } = new();
        public Guid GroupID { get; set; } = new();
    }
}
