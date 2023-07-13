using UcabGo.App.ViewModel;

namespace UcabGo.App.Views;

public partial class RateUserView : ContentPage
{
	public RateUserView(RateUserViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        (BindingContext as RateUserViewModel)?.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        (BindingContext as RateUserViewModel)?.OnDisappearing();
    }
}