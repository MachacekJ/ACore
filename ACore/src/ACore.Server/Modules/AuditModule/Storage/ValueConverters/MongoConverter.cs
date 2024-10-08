// using System.Globalization;
// using ACore.Server.Modules.AuditModule.Storage.SQL.Models;
// using ACore.Server.Storages.Definitions.EF.MongoStorage;
// using MongoDB.Bson;
//
// namespace ACore.Server.Modules.AuditModule.Storage.ValueConverters;
//
// public class MongoValueConverter : MongoStorageDefinition, IAuditConverterDefinition
// {
//   public TAudit? ToAuditValue<TAudit, TCSharp>(object? value)
//   {
//     if (value == null)
//       return default;
//
//     return value switch
//     {
//       byte or short or int or long or bool or Guid or ObjectId => value.ToString(),
//       TimeSpan ts => ts.Ticks.ToString(),
//       DateTime dateTime => dateTime.Ticks.ToString(),
//       decimal dec => dec.ToString(CultureInfo.InvariantCulture),
//       _ => SqlConvertedItem.ToValueString(logger, value)
//     };
//   }
//
//   public TCSharp ToCSharpValue<TCSharp, TAudit>(TAudit value)
//   {
//     throw new NotImplementedException();
//   }
// }