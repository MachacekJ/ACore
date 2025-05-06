using ACore.Server.Repository.Models.EntityEvent;
using MediatR;

namespace ACore.Server.Repository.CQRS.Notifications;

/// <summary>
/// This event is fired when some operation for repository was executed.
/// </summary>
public record RepositorySaveEventNotification(EntityEventItem EntityEvent) : INotification;