using UcabGo.App.ViewModel;

namespace UcabGo.App.Views;

public partial class ProfileView : ContentPage
{
    public ProfileView(ProfileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as ProfileViewModel)?.OnAppearing();

        scrollview.ScrollToAsync(0, 0, false);
    }   
}