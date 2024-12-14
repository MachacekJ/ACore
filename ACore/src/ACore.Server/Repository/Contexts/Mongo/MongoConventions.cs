using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace ACore.Server.Repository.Contexts.Mongo;

public static class MongoConventions
{
  public static void RegisterConventions()
  {
    var packs = ConventionRegistry.Lookup(typeof(Guid));
    if (packs != null && packs.Conventions.Any(c => c is GuidAsStringRepresentationConvention))
      return;

    var pack = new ConventionPack { new GuidAsStringRepresentationConvention() };
    ConventionRegistry.Register(
      "GUIDs as strings Conventions",
      pack,
      _ => true // For all entity types
    );
  }

  private class GuidAsStringRepresentationConvention : ConventionBase, IMemberMapConvention
  {
    public void Apply(BsonMemberMap memberMap)
    {
      if (memberMap.MemberType == typeof(Guid))
      {
        memberMap.SetSerializer(
          new GuidSerializer(BsonType.String));
      }
      else if (memberMap.MemberType == typeof(Guid?))
      {
        memberMap.SetSerializer(
          new NullableSerializer<Guid>(new GuidSerializer(BsonType.String)));
      }
    }
  }
}