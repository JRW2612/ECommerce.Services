namespace Orders.Service.Entities
{
    public class OutBoxMessage : EntityBase
    {
        public string? Type { get; set; }
        public string? Content { get; set; }
        public string CorrelationId { get; set; }
        public string? Error { get; set; }
        public DateTime? OccurredOn { get; set; }
        public DateTime? ProcessedOn { get; set; }
        public bool IsProcessed => ProcessedOn.HasValue;
    }
}
