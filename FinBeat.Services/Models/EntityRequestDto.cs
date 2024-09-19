namespace FinBeat.Services.Models
{
    /// <summary>
    /// The data transfer object for the entity from the request.
    /// </summary>
    public class EntityRequestDto
    {
        public int Code { get; set; }
        public string Value { get; set; }
    }
}
