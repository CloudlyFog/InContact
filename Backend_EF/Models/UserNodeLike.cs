using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_EF.Models
{
    public class UserNodeLike
    {
        public int ID { get; set; }
        public Guid PostNewsID { get; set; } = Guid.NewGuid();
        public Guid UserID { get; set; } = Guid.NewGuid();

        [NotMapped]
        public UserModel UserModel { get; set; } = new UserModel();

    }
}
