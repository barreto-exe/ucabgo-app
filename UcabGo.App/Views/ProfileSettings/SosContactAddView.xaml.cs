using UcabGo.App.ViewModel;

namespace UcabGo.App.Views;

public partial class SosContactAddView : ContentPage
{
    public SosContactAddView(SosContactsAddViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as SosContactsAddViewModel)?.OnAppearing();
    }
}