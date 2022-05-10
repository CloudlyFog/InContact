using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_EF.Models
{
    public class UserModel
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string? Name { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public bool Authenticated { get; set; }
        public bool Access { get; set; } = false;
        public bool IncognitoMode { get; set; } = false;


        [NotMapped]
        public MessageModel? MessageModel { get; set; } = new();

        [NotMapped]
        public NoteModel? NoteModel { get; set; } = new();
    }
}
