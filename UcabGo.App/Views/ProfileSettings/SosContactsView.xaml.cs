using UcabGo.App.ViewModel;

namespace UcabGo.App.Views;

public partial class SosContactsView : ContentPage
{
    public SosContactsView(SosContactsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as SosContactsViewModel)?.OnAppearing();
    }
}