﻿using UcabGo.App.Services;
using UcabGo.App.Views;

namespace UcabGo.App.Shells;

public partial class AppShell : Shell
{
    private readonly INavigationService navigationService;
    public AppShell(INavigationService navigationService)
    {
        InitializeComponent();
        this.navigationService = navigationService;

        lblVersion.Text = $"Versión {VersionTracking.CurrentVersion}";

        Routing.RegisterRoute(nameof(PasswordView), typeof(PasswordView));
        Routing.RegisterRoute(nameof(PhoneView), typeof(PhoneView));
        Routing.RegisterRoute(nameof(SosContactsView), typeof(SosContactsView));
        Routing.RegisterRoute(nameof(SosContactAddView), typeof(SosContactAddView));
        Routing.RegisterRoute(nameof(VehiclesView), typeof(VehiclesView));
        Routing.RegisterRoute(nameof(VehiclesAddView), typeof(VehiclesAddView));
        Routing.RegisterRoute(nameof(WalkingDistanceView), typeof(WalkingDistanceView));
        Routing.RegisterRoute(nameof(MapView), typeof(MapView));
        Routing.RegisterRoute(nameof(DestinationsListView), typeof(DestinationsListView));
        Routing.RegisterRoute(nameof(DestinationAddView), typeof(DestinationAddView));
        Routing.RegisterRoute(nameof(ActiveRiderView), typeof(ActiveRiderView));
        Routing.RegisterRoute(nameof(SelectDestinationView), typeof(SelectDestinationView));
        Routing.RegisterRoute(nameof(RidesAvailableView), typeof(RidesAvailableView));
        Routing.RegisterRoute(nameof(ActivePassengerView), typeof(ActivePassengerView));
        Routing.RegisterRoute(nameof(ChatView), typeof(ChatView));
        Routing.RegisterRoute(nameof(RateUserView), typeof(RateUserView));
    }

    protected override async void OnParentSet()
    {
        base.OnParentSet();

        //if(Parent is not null)
        //{
        //    await navigationService.InitializeAsync();
        //}
    }
}
