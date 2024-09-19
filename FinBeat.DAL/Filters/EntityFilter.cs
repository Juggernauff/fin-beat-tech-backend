﻿namespace FinBeat.DAL.Filters
{
    /// <summary>
    /// Represents a filter for querying entities from the database based on specified criteria such as ID range, code range, or value.
    /// </summary>
    public class EntityFilter
    {
        public int? MinId { get; set; }
        public int? MaxId { get; set; }
        public int? MinCode { get; set; }
        public int? MaxCode { get; set; }
        public string Value { get; set; }
    }
}
