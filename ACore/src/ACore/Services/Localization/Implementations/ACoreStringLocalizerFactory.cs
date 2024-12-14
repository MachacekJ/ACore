using ACore.Modules.LocalizationModule.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace ACore.Services.Localization.Implementations;

public class ACoreStringLocalizerFactory(IOptions<LocalizationModuleOptions> localizationOptions) : IStringLocalizerFactory
{
  IStringLocalizer IStringLocalizerFactory.Create(Type resourceSource)
  {
    return new ACoreStringLocalizer(resourceSource, localizationOptions);
  }

  IStringLocalizer IStringLocalizerFactory.Create(string baseName, string location)
  {
    throw new NotImplementedException();
  }
}