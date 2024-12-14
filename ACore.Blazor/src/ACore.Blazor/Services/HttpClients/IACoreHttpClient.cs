namespace ACore.Blazor.Services.HttpClients;

public interface IACoreHttpClient
{
  Task<HttpClient> CreateLocXClientAsync();
}