using ACore.Models.Result;
using MediatR;
using Microsoft.Extensions.Options;

namespace ACore.Server.Configuration.CQRS.OptionsGet;

public class AppOptionHandler<T>(IOptions<ACoreServerOptions> options) : IRequestHandler<AppOptionQuery<T>, Result<T>>
{
  public Task<Result<T>> Handle(AppOptionQuery<T> request, CancellationToken cancellationToken)
  {
    switch (request.OptionQueryEnum)
    {
      case OptionQueryEnum.HashSalt:
        return Task.FromResult(Result.Success((T)Convert.ChangeType(options.Value.SaltForHash, typeof(T))));
    }

    throw new NotImplementedException();
  }
}