using System.Globalization;
using ACore.Server.Modules.AuditModule.Repositories.Helpers;
using ACore.Server.Modules.AuditModule.Repositories.SQL.Models;
using ACore.Server.Storages.Models.EntityEvent;

namespace ACore.Server.Modules.AuditModule.Repositories.Extensions;

internal static class SaveInfoColumnItemExtensions
{
  internal static AuditValueEntity ToAuditSqlValue(this EntityEventColumnItem infoColumnItem, Dictionary<string, int> columnIds)
  {
    var valueEntity = new AuditValueEntity
    {
      IsChanged = infoColumnItem.IsChanged,
      AuditColumnId = columnIds.First(e => e.Key == infoColumnItem.PropName).Value
    };

    if (infoColumnItem.OldValue != null)
    {
      switch (infoColumnItem.OldValue)
      {
        case byte b:
          valueEntity.OldValueInt = b;
          break;
        case short s:
          valueEntity.OldValueInt = s;
          break;
        case int i:
          valueEntity.OldValueInt = i;
          break;
        case long l:
          valueEntity.OldValueLong = l;
          break;
        case bool bl:
          valueEntity.OldValueBool = bl;
          break;
        case Guid g:
          valueEntity.OldValueGuid = g;
          break;
        case DateTime dt:
          valueEntity.OldValueLong = dt.Ticks;
          break;
        case TimeSpan span:
          valueEntity.OldValueLong = span.Ticks;
          break;
        case string st:
          valueEntity.OldValueString = st;
          break;
        case decimal dec:
          valueEntity.OldValueString = dec.ToString(CultureInfo.InvariantCulture);
          break;
        default:
          valueEntity.OldValueString = infoColumnItem.OldValue.ToAuditValue();
          break;
      }
    }

    if (!infoColumnItem.IsChanged || infoColumnItem.NewValue == null)
      return valueEntity;

    switch (infoColumnItem.NewValue)
    {
      case byte b:
        valueEntity.NewValueInt = b;
        break;
      case short s:
        valueEntity.NewValueInt = s;
        break;
      case int i:
        valueEntity.NewValueInt = i;
        break;
      case long l:
        valueEntity.NewValueLong = l;
        break;
      case bool bl:
        valueEntity.NewValueBool = bl;
        break;
      case Guid g:
        valueEntity.NewValueGuid = g;
        break;
      case DateTime dt:
        valueEntity.NewValueLong = dt.Ticks;
        break;
      case TimeSpan span:
        valueEntity.NewValueLong = span.Ticks;
        break;
      case string st:
        valueEntity.NewValueString = st;
        break;
      case decimal dec:
        valueEntity.NewValueString = dec.ToString(CultureInfo.InvariantCulture);
        break;
      default:
        valueEntity.NewValueString = infoColumnItem.NewValue.ToAuditValue();
        break;
    }

    return valueEntity;
  }
}