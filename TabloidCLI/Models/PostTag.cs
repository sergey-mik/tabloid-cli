namespace TabloidCLI.Models
{
    public class PostTag
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public Tag Tag { get; set; }
    }
}
