namespace Nom1Done.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TransactionLog
    {
        [Key]
        public long Id { get; set; }
        public string TransactionId { get; set; }
        public int StatusId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
