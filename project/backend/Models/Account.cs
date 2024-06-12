using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Account
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("Name")]
    public string Name { get; set; }

    [BsonElement("AccountNumber")]
    public string AccountNumber { get; set; }

    [BsonElement("Balance")]
    public decimal Balance { get; set; }

    [BsonElement("Transactions")]
    public List<Transaction> Transactions { get; set; } = new List<Transaction>();
}

public class Transaction
{
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
}
