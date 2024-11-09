using ACore.Server.Storages.Attributes;
using ACore.Server.Storages.Definitions.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ACore.Server.Storages.Contexts.EF;

public abstract partial class DbContextBase
{
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    foreach (var entityType in modelBuilder.Model.GetEntityTypes())
    {
      var typee = entityType.ClrType ?? throw new ArgumentNullException(nameof(entityType.ClrType));
      var aa = typee.TableIdAttr();
      if (aa != null)
      {
        var f = (entityType.FindProperties([aa.IdName]) ?? Array.Empty<IMutableProperty>()).FirstOrDefault();
        if (f != null)
         modelBuilder.Entity(typee).Property(f.Name).HasColumnName(aa.IdName);
      }
    }
  }

  protected static void SetDatabaseNames<T>(Dictionary<string, EFDbNames> objectNameMapping, ModelBuilder modelBuilder) where T : class
  {
    if (objectNameMapping.TryGetValue(typeof(T).Name, out var auditColumnEntityObjectNames))
    {
      modelBuilder.Entity<T>().ToTable(auditColumnEntityObjectNames.TableName);
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