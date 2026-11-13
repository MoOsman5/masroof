using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Masroof.Api.Models;

public class Expense
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string TenantId { get; set; } = string.Empty;

    public required string Title { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Amount { get; set; }

    public string Currency { get; set; } = "EGP";

    public string Category { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}

