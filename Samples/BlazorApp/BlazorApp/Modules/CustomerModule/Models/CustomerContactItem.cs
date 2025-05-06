using BlazorApp.Modules.CustomerModule.Repository.Mongo.Models;
using Mapster;

namespace BlazorApp.Modules.CustomerModule.Models;

public class CustomerContactItem
{
  public CustomerContactTypeEnum Type { get; set; }
  public required string Value { get; set; }
}

internal static class CustomerContactItemExtensions
{
  internal static CustomerContactItem ToItem(this CustomerContactEntity customerContactEntity) 
    => customerContactEntity.Adapt<CustomerContactItem>();

  internal static CustomerContactEntity ToEntity(this CustomerContactItem customerContactItem) 
    => customerContactItem.Adapt<CustomerContactEntity>();
}