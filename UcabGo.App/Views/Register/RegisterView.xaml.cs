using UcabGo.App.ViewModel;

namespace UcabGo.App.Views;

public partial class RegisterView : ContentPage
{
	public RegisterView(RegisterViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

	protected override void OnAppearing()
	{
        base.OnAppearing();
		(BindingContext as RegisterViewModel)?.OnAppearing();
	}

}