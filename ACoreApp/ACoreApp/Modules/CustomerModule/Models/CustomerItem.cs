using ACoreApp.Modules.CustomerModule.Repository.Mongo.Models;
using Mapster;

namespace ACoreApp.Modules.CustomerModule.Models;

public class CustomerItem
{
  public required string Id { get; set; }
  public required string Name { get; set; }
  public CustomerAddressItem[]? Addresses { get; set; }
  public CustomerContactItem[]? Contacts { get; set; }
}

internal static class CustomerItemExtensions
{
  internal static CustomerItem ToItem(this CustomerEntity customerEntity)
  {
    var customerItem = customerEntity.Adapt<CustomerItem>();
    if (customerEntity.Addresses != null)
      customerItem.Addresses = customerEntity.Addresses.Select(customerAddressEntity => customerAddressEntity.ToItem()).ToArray();

    if (customerEntity.Contacts != null)
      customerItem.Contacts = customerEntity.Contacts.Select(customerContactEntity => customerContactEntity.ToItem()).ToArray();

    return customerItem;
  }

  internal static CustomerEntity ToEntity(this CustomerItem customerItem)
  {
    var customerEntity = customerItem.Adapt<CustomerEntity>();
    if (customerItem.Addresses != null)
      customerEntity.Addresses = customerItem.Addresses.Select(customerAddressItem => customerAddressItem.ToEntity()).ToArray();
    if (customerItem.Contacts != null)
      customerEntity.Contacts = customerItem.Contacts.Select(customerContactItem => customerContactItem.ToEntity()).ToArray();
    return customerEntity;
  }
}