using UcabGo.App.ViewModel;

namespace UcabGo.App.Views;

public partial class MapView : ContentPage
{
	public MapView(MapViewModel mapViewModel)
	{
		InitializeComponent();
		BindingContext = mapViewModel;

		mapViewModel.Map = myMap;
		mapViewModel.SearchBar = searchBar;

		myMap.UiSettings.ZoomControlsEnabled = false;
		myMap.UiSettings.MyLocationButtonEnabled = false;
		myMap.UiSettings.CompassEnabled = false;
		myMap.UiSettings.TiltGesturesEnabled = true;
	}

	protected override void OnAppearing()
	{
        base.OnAppearing();
        (BindingContext as MapViewModel)?.OnAppearing();
    }
}