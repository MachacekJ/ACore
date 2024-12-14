using ACore.Server.Repository.Contexts.Mongo.Models.PK;
using ACoreApp.Modules.CustomerModule.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace ACoreApp.Modules.CustomerModule.Repository.Mongo.Models;

internal class CustomerEntity : PKMongoEntity
{
  [BsonElement("name")]
  public required string Name { get; set; }
  
  [BsonElement("addresses")]
  public CustomerAddressEntity[]? Addresses { get; set; }
  
  [BsonElement("contacts")]
  public CustomerContactEntity[]? Contacts { get; set; }
}

internal class CustomerAddressEntity
{
  [BsonElement("street")]
  public required string Street { get; set; }
  [BsonElement("city")]
  public required string City { get; set; }
  [BsonElement("country")]
  public required string Country { get; set; }
}

internal class CustomerContactEntity
{
  [BsonElement("type")]
  public CustomerContactTypeEnum Type { get; set; }
  [BsonElement("value")]
  public required string Value { get; set; }
}