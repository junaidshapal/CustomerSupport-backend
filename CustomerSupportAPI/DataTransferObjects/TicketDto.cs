﻿namespace CustomerSupportAPI.DataTransferObjects
{
    public class TicketDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        public string AssignedTo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
