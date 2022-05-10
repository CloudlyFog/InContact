namespace Backend_EF.Models
{
    public class MessageModel
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid UserID { get; set; }
        public string? Email { get; set; } = string.Empty;
        public string? ToEmail { get; set; } = "maximkirichenk0.06@gmail.com";
        public string? Message { get; set; } = string.Empty;
    }
}
