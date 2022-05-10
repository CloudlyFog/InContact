namespace Backend_EF.Models
{
    public class NewsModel
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid PostID { get; set; } = Guid.NewGuid();
        public Guid UserID { get; set; } = new();
        public string GroupName { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public int Likes { get; set; } = 0;
        public Guid BindID { get; set; } = new();
    }
}
