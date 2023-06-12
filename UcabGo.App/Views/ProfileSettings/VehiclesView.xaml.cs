using UcabGo.App.ViewModel;

namespace UcabGo.App.Views;

public partial class VehiclesView : ContentPage
{
	public VehiclesView(VehiclesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override void OnAppearing()
	{
        base.OnAppearing();
        (BindingContext as VehiclesViewModel)?.OnAppearing();
    }
}