using ACore.Models.Cache;
using ACore.Server.Services.ServerCache.Models;
using MediatR;

namespace ACore.Server.Services.ServerCache.CQRS.Notification;

/// <summary>
/// Notification about getting cache value.
/// </summary>
/// <param name="CacheKey"></param>
/// <param name="CacheType"></param>
/// <param name="Result">True - success, not null</param>
public record ServerCacheGetItemNotification(CacheKey CacheKey, ServerCacheTypeEnum CacheType, bool Result) : INotification;