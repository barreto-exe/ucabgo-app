using CommunityToolkit.Mvvm.ComponentModel;
using UcabGo.App.Services;

namespace UcabGo.App.ViewModel
{
    public abstract class ViewModelBase : ObservableObject
    {
        protected readonly ISettingsService settings;
        protected readonly INavigationService navigation;
        protected ViewModelBase(ISettingsService settingsService, INavigationService navigation)
        {
            settings = settingsService;
            this.navigation = navigation;
        }
    }
}
