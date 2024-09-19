namespace FinBeat.Services.Models
{
    /// <summary>
    /// The data transfer object to filter by entities.
    /// </summary>
    public class EntityFilterDto
    {
        public int? MinId { get; set; }
        public int? MaxId { get; set; }
        public int? MinCode { get; set; }
        public int? MaxCode { get; set; }
        public string? Value { get; set; }
    }
}
