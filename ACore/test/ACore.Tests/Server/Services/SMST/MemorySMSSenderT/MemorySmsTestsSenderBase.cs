﻿using ACore.Server.Services.SMS;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.Services.SMST.MemorySMSSenderT;

public class MemorySmsTestsSenderBase : SmsTestsBase
{
    protected override void RegisterServices(ServiceCollection sc)
    {
        base.RegisterServices(sc);
        sc.AddSingleton<ISMSSenderJM, MemorySMSSender>();
    }
}