using ACore.Blazor.Modules.Analytics.Models;
using ACore.Blazor.Modules.LocalStorageModule.CQRS.LocalStorageGet;
using ACore.Blazor.Modules.LocalStorageModule.CQRS.LocalStorageSave;
using ACore.Blazor.Modules.LocalStorageModule.CQRS.Models;
using MediatR;

namespace ACore.Blazor.Modules.Analytics;

public class WriteAnalyticsHandler(IMediator mediator) : IRequestHandler<WriteAnalyticsCommand>
{
  public async Task Handle(WriteAnalyticsCommand request, CancellationToken cancellationToken)
  {
    var analyticsName = Enum.GetName(typeof(AnalyticsTypeEnum), request.AnalyticsData.AnalyticsTypeEnum) ?? throw new NullReferenceException();
    var savedAnalyticsValues = await mediator.Send(new LocalStorageGetQuery(LocalStorageCategoryEnum.Analytics, analyticsName), cancellationToken);
    var analytics = new List<AnalyticsData>();
    if (savedAnalyticsValues.IsValue)
    {
      analytics = savedAnalyticsValues.GetValue<List<AnalyticsData>>() ?? [];
    }

    analytics.Add(request.AnalyticsData);

    if (analytics.Count > 10)
    {
      // TODO send to server
      analytics.Clear();
    }

    await mediator.Send(new LocalStorageSaveCommand(LocalStorageCategoryEnum.Analytics, analyticsName, analytics, analytics.GetType()), cancellationToken);
  }
}