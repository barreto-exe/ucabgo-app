using UcabGo.App.ViewModel;

namespace UcabGo.App.Views;

public partial class DestinationsListView : ContentPage
{
	public DestinationsListView(DestinationsListViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

	protected override void OnAppearing()
	{
        base.OnAppearing();
        (BindingContext as DestinationsListViewModel)?.OnAppearing();
    }
}