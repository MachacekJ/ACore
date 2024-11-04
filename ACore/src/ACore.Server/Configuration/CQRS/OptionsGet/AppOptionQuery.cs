using ACore.Models.Result;
using MediatR;

namespace ACore.Server.Configuration.CQRS.OptionsGet;

public enum OptionQueryEnum
{
  HashSalt,
}

public class AppOptionQuery<T>(OptionQueryEnum optionQueryEnum) : IRequest<Result<T>>
{
  public OptionQueryEnum OptionQueryEnum => optionQueryEnum;
}