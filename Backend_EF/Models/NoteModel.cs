namespace Backend_EF.Models
{
    public class NoteModel
    {
        public Guid IdNote { get; set; } = Guid.NewGuid(); //note`s id
        public Guid ID { get; set; }//user`s id
        public string? Title { get; set; }
        public string? Body { get; set; }
    }
}
