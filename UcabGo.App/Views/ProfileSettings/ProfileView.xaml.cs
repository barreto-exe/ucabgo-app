using UcabGo.App.ViewModel;

namespace UcabGo.App.Views;

public partial class ProfileView : ContentPage
{
    public ProfileView(ProfileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}