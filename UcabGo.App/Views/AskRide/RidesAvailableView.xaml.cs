using UcabGo.App.ViewModel;

namespace UcabGo.App.Views;

public partial class RidesAvailableView : ContentPage
{
	public RidesAvailableView(RidesAvailableViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

	protected override void OnAppearing()
	{
        base.OnAppearing();
        (BindingContext as RidesAvailableViewModel)?.OnAppearing();
    }

}