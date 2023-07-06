using UcabGo.App.ViewModel;

namespace UcabGo.App.Views;

public partial class ActiveRiderView : ContentPage
{
    public ActiveRiderView(ActiveRiderViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as ActiveRiderViewModel)?.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        (BindingContext as ActiveRiderViewModel)?.OnDisappearing();
    }
}