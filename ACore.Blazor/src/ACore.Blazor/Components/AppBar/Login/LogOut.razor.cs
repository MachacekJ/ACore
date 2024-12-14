using ACore.Blazor.Services.App.Models;

namespace ACore.Blazor.Components.AppBar.Login
{
    public partial class LogOut : ACoreComponentBase, IDisposable
    {
        protected override bool LocalizationEnabled => true;
        
        private string _mobileIconCss = string.Empty;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            AppManager.OnResponsiveChange += AppManagerOnOnResponsiveChange;
        }

        private Task AppManagerOnOnResponsiveChange(ResponsiveTypeEnum type)
        {
            _mobileIconCss = type== ResponsiveTypeEnum.Desktop
                ? string.Empty
                : "jm-mobile-icon";
            StateHasChanged();
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            base.Dispose();
            AppManager.OnResponsiveChange -= AppManagerOnOnResponsiveChange;
        }
    }
}