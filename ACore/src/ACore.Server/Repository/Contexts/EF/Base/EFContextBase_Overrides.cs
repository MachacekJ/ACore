using ACore.Server.Repository.Contexts.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace ACore.Server.Repository.Contexts.EF.Base;

public abstract partial class EFContextBase
{
  protected static void SetDatabaseNames<T>(Dictionary<string, EFInternalName> objectNameMapping, ModelBuilder modelBuilder) where T : class
  {
    if (objectNameMapping.TryGetValue(typeof(T).Name, out var auditColumnEntityObjectNames))
    {
      modelBuilder.Entity<T>().ToTable(auditColumnEntityObjectNames.TableName, auditColumnEntityObjectNames.SchemaName);
      foreach (var expression in auditColumnEntityObjectNames.GetColumns<T>())
      {
        modelBuilder.Entity<T>().Property(expression.Key).HasColumnName(expression.Value);
      }
    }
    else
    {
      throw new Exception($"Missing database name definition for entity: {typeof(T).Name}");
    }
  }
}