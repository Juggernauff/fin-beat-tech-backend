namespace FinBeat.Services.Models
{
    /// <summary>
    /// The data transfer object for the entity from the response.
    /// </summary>
    public class EntityResponseDto
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Value { get; set; }
    }
}
