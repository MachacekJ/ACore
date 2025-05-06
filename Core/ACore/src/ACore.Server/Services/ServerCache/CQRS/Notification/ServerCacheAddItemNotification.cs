using ACore.Models.Cache;
using ACore.Server.Services.ServerCache.Models;
using MediatR;

namespace ACore.Server.Services.ServerCache.CQRS.Notification;

public record ServerCacheAddItemNotification(CacheKey CacheKey, object? CacheValue, ServerCacheTypeEnum CacheType, TimeSpan Expiration) : INotification;