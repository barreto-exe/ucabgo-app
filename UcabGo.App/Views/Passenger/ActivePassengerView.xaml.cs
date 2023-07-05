using UcabGo.App.ViewModel;

namespace UcabGo.App.Views;

public partial class ActivePassengerView : ContentPage
{
	public ActivePassengerView(ActivePassengerViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

	protected override void OnAppearing()
	{
        base.OnAppearing();

        (BindingContext as ActivePassengerViewModel)?.OnAppearing();
    }
}