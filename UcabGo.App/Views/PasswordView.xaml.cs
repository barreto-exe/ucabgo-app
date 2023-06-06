using UcabGo.App.ViewModel;

namespace UcabGo.App.Views;

public partial class PasswordView : ContentPage
{
	public PasswordView(PasswordViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override void OnAppearing()
	{
        base.OnAppearing();

		var viewModel = BindingContext as PasswordViewModel;
        viewModel?.OnAppearing();
    }
}