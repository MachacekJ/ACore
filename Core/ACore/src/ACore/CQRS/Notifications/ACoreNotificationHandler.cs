using MediatR;
using Microsoft.Extensions.Logging;

namespace ACore.CQRS.Notifications;

public abstract class ACoreNotificationHandler<TNotification>(ILogger logger) : ACoreNotificationHandler, INotificationHandler<TNotification>
  where TNotification : INotification
{
  protected abstract Task HandleMethod(TNotification notification, CancellationToken cancellationToken);

  public async Task Handle(TNotification notification, CancellationToken cancellationToken)
  {
    try
    {
      if (!InBackground)
        await HandleMethod(notification, cancellationToken);
      else
      {
        _ = Task.Run(async () => await HandleMethod(notification, cancellationToken), cancellationToken).ConfigureAwait(false);
      }
    }
    catch (Exception e)
    {
      logger.LogError(e, e.Message);
      if (ThrowException)
        throw;
    }
  }
}

public abstract class ACoreNotificationHandler
{
  /// <summary>
  /// Throw an exception if the other side also throws an exception.
  /// For example, some metrics cannot be written (the server is unavailable), but the application continues. Only the log is written.
  /// </summary>
  public abstract bool ThrowException { get; }

  /// <summary>
  /// Receive notification and run handle in background.
  /// </summary>
  public abstract bool InBackground { get; }
}