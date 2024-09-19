namespace FinBeat.DAL.Models
{
    /// <summary>
    /// Represents an entity in the database with a unique identifier, code, and value.
    /// </summary>
    public class Entity
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Value { get; set; }
    }
}
