using ACore.Blazor.Modules.Analytics.Models;
using MediatR;

namespace ACore.Blazor.Modules.Analytics;

public class WriteAnalyticsCommand(AnalyticsData analyticsData) : IRequest
{
    public AnalyticsData AnalyticsData { get; } = analyticsData;
}