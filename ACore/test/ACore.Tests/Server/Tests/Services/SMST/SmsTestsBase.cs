using ACore.Server.Services.SMS;
using ACore.Tests.Base;
using ACore.Tests.Server.TestInfrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.Tests.Services.SMST;

public class SmsTestsBase : TestsBase
{
    protected ISMSSenderJM SMSSender = null!;

    protected override async Task GetServices(IServiceProvider sp)
    {
        await base.GetServices(sp);
        SMSSender = sp.GetService<ISMSSenderJM>() ?? throw new ArgumentException($"{nameof(ISMSSenderJM)} is null.");
    }
}