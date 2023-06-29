using UcabGo.App.ViewModel;

namespace UcabGo.App.Views;

public partial class RoleSelectionView : ContentPage
{
    public RoleSelectionView(RoleSelectionViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as RoleSelectionViewModel)?.OnAppearing();
    }
}