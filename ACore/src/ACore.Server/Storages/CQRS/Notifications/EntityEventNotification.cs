using ACore.Server.Storages.Models.EntityEvent;
using MediatR;

namespace ACore.Server.Storages.CQRS.Notifications;

/// <summary>
/// This event is fired when some operation for storage was executed.
/// </summary>
public record EntityEventNotification(EntityEventItem EntityEvent) : INotification;