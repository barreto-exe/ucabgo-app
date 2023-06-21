using UcabGo.App.ViewModel;

namespace UcabGo.App.Views;

public partial class DestinationAddView : ContentPage
{
    public DestinationAddView(DestinationAddViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        viewModel.Map = myMap;
        viewModel.SearchBar = searchBar;

        myMap.UiSettings.ZoomControlsEnabled = false;
        myMap.UiSettings.MyLocationButtonEnabled = false;
        myMap.UiSettings.CompassEnabled = false;
        myMap.UiSettings.TiltGesturesEnabled = true;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as DestinationAddViewModel)?.OnAppearing();
    }
}