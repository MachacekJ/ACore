using BlazorApp.Modules.CustomerModule.Repository.Mongo.Models;
using Mapster;

namespace BlazorApp.Modules.CustomerModule.Models;

public class CustomerAddressItem
{
  public required string Street { get; set; }
  public required string City { get; set; }
  public required string Country { get; set; }
}

internal static class CustomerAddressItemExtensions
{
  internal static CustomerAddressItem ToItem(this CustomerAddressEntity customerAddressEntity) 
    => customerAddressEntity.Adapt<CustomerAddressItem>();

  internal static CustomerAddressEntity ToEntity(this CustomerAddressItem customerAddressItem) 
    => customerAddressItem.Adapt<CustomerAddressEntity>();
}