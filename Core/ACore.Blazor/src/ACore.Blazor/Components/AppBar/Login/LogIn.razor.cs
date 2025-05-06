using ACore.Blazor.Services.App.Models;
using Microsoft.AspNetCore.Components;

namespace ACore.Blazor.Components.AppBar.Login
{
    public partial class LogIn(NavigationManager navigation) : ACoreComponentBase, IDisposable
    {
        private string _mobileIconCss = string.Empty;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            AppManager.OnResponsiveChange += AppManagerOnOnResponsiveChange;
        }

        private Task AppManagerOnOnResponsiveChange(ResponsiveTypeEnum type)
        {
            _mobileIconCss = type == ResponsiveTypeEnum.Desktop
                ? string.Empty
                : "jm-mobile-icon";
            StateHasChanged();
            return Task.CompletedTask;
        }
        
        private void LoginClick()
        {
            navigation.NavigateTo("api/Account/login", true);
        }
        
        public override void Dispose()
        {
            base.Dispose();
            AppManager.OnResponsiveChange -= AppManagerOnOnResponsiveChange;
        }
    }
}