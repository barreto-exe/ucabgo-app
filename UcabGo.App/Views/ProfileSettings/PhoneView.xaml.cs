using UcabGo.App.ViewModel;

namespace UcabGo.App.Views;

public partial class PhoneView : ContentPage
{
	public PhoneView(PhoneViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var viewModel = BindingContext as ViewModelBase;
        viewModel?.OnAppearing();
    }
}