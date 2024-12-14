using System.Text.Json.Serialization;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace ACoreApp.Client.UI.Pages.Home;

/// <summary>
/// https://app.quicktype.io/
/// </summary>
public class HomePageLoc //: ILocXJsonConfig
{
  //public string LocXPath => $"{nameof(UI)}/{nameof(Pages)}/{nameof(HomePage)}";
  
  [JsonPropertyName("title")]
  public string Title { get; set; }
}
