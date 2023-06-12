using UcabGo.App.ViewModel;

namespace UcabGo.App.Views;

public partial class VehiclesAddView : ContentPage
{
    public VehiclesAddView(VehiclesAddViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as VehiclesAddViewModel)?.OnAppearing();
    }
}