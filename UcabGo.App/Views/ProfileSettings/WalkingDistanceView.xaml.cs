using UcabGo.App.ViewModel;

namespace UcabGo.App.Views;

public partial class WalkingDistanceView : ContentPage
{
	public WalkingDistanceView(WalkingDistanceViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override void OnAppearing()
	{
        base.OnAppearing();
        (BindingContext as WalkingDistanceViewModel)?.OnAppearing();
    }
}