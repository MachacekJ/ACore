using ACore.Server.Repository.Contexts.Mongo;
using ACore.Server.Repository.Contexts.Mongo.Models;
using ACore.Server.Services;
using ACoreApp.Modules.CustomerModule.Configuration;
using ACoreApp.Modules.CustomerModule.Repository.Mongo.Models;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ACoreApp.Modules.CustomerModule.Repository.Mongo;

internal class CustomerMongoRepositoryImp : MongoContextBase, ICustomerRepository
{
  public const string CustomerCollectionName = "customer";
  
  protected override string ModuleName => nameof(ICustomerRepository);
  protected override IEnumerable<MongoVersionScriptsBase> AllUpdateVersions => Scripts.EFScriptRegistrations.AllVersions;
 
  private readonly IMongoCollection<CustomerEntity> _customerCollection;
  
  public CustomerMongoRepositoryImp(IACoreServerCurrentScope serverCurrentScope, IOptions<CustomerModuleOptions> options, IMediator mediator, ILogger<CustomerMongoRepositoryImp> logger)
    : base(serverCurrentScope, options.Value.MongoDb ?? throw new ArgumentNullException(nameof(options.Value.MongoDb)), mediator, logger)
  {
    _customerCollection = MongoDatabase.GetCollection<CustomerEntity>(CustomerCollectionName);
  }

}