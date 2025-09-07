using System.Text.Json.Serialization;

namespace AsyncProgramming.Models
{
    public record DailyPrice
    (
    [property: JsonPropertyName("date")] DateTime Date,
    [property: JsonPropertyName("open")] decimal Open,
    [property: JsonPropertyName("close")] decimal Close,
    [property: JsonPropertyName("high")] decimal High,
    [property: JsonPropertyName("low")] decimal Low,
    [property: JsonPropertyName("volume")] long Volume,
    [property: JsonPropertyName("adj_close")] decimal AdjClose
    );

    /* WHY RECORD VS CLASS?
     * Value equality: Two DailyPrice with same values are equal
     * Thread safety: Immutable objects are inherently thread-safe
     * Concise syntax: Less boilerplate code.
     * Built-in ToString(): Great for debugging.
     */
}
