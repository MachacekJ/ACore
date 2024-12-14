namespace ACore.Blazor.Services.App.Manager;

public interface IAppEnvironment
{
  Task ChangeLanguage(int lcid);
  Task SetStartLanguage(int defaultLanguage);
}