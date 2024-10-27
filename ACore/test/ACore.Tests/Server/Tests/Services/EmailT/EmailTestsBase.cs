using ACore.Server.Services.Email;
using ACore.Tests.Base;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.Tests.Services.EmailT;

public class EmailTestsBase : TestsBase
{
    protected IEmailSenderJM EmailSenderJM = null!;

    protected override async Task GetServices(IServiceProvider sp)
    {
        await base.GetServices(sp);

        EmailSenderJM = sp.GetRequiredService<IEmailSenderJM>();
        if (EmailSenderJM == null)
            throw new Exception("EmailBaseT IEmailSenderJM is not implemented.");
    }
}