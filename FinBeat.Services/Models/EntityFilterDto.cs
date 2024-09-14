namespace FinBeat.Services.Models
{
    public class EntityFilterDto
    {
        public int? MinId { get; set; }
        public int? MaxId { get; set; }
        public int? MinCode { get; set; }
        public int? MaxCode { get; set; }
        public string? Value { get; set; }
    }
}
