using UcabGo.App.ViewModel;

namespace UcabGo.App.Views;

public partial class RoleSelectionView : ContentPage
{
	public RoleSelectionView(RoleSelectionViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}