namespace CustomerSupportAPI.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string JsonData { get; set; } = string.Empty;

    }
}
