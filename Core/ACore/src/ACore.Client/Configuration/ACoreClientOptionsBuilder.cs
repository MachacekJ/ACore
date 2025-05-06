namespace ACore.Client.Configuration;

public class ACoreClientOptionsBuilder
{
  public static ACoreClientOptionsBuilder Empty() => new();

  public ACoreClientOptions Build()
  {
    return new ACoreClientOptions();
  }
}