namespace UcabGo.App.Views;

public partial class MapView : ContentPage
{
	public MapView()
	{
		InitializeComponent();

		myMap.UiSettings.ZoomControlsEnabled = false;
		myMap.UiSettings.MyLocationButtonEnabled = false;
		myMap.UiSettings.CompassEnabled = false;
		myMap.UiSettings.TiltGesturesEnabled = true;
	}
}