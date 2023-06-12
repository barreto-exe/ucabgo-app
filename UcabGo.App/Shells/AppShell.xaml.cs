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

        Routing.RegisterRoute(nameof(PasswordView), typeof(PasswordView));
        Routing.RegisterRoute(nameof(PhoneView), typeof(PhoneView));
        Routing.RegisterRoute(nameof(SosContactsView), typeof(SosContactsView));
        Routing.RegisterRoute(nameof(SosContactAddView), typeof(SosContactAddView));
        Routing.RegisterRoute(nameof(VehiclesView), typeof(VehiclesView));
        Routing.RegisterRoute(nameof(VehiclesAddView), typeof(VehiclesAddView));
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
