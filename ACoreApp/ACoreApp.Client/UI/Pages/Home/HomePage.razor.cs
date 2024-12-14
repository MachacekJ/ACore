using ACoreApp.Client.Configuration.Pages;
using Microsoft.AspNetCore.Components;

namespace ACoreApp.Client.UI.Pages.Home;

public partial class HomePage(NavigationManager navMan) : ACore.Blazor.ACorePageBase
{
  protected override void OnInitialized()
  {
    base.OnInitialized();
  }

  protected override Task OnInitializedAsync()
  {
    return base.OnInitializedAsync();
  }

  private void GoToAboutPage()
  {
    navMan.NavigateTo(AppPagesDefinitions.About.PageUrl);
  }

  private void GetTestData()
  {
    
  }
}