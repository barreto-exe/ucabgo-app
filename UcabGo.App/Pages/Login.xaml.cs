﻿using UcabGo.App.ApiAccess;
using UcabGo.App.ApiAccess.Services;
using UcabGo.App.ViewModel;

namespace UcabGo.App.Pages;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
		BindingContext = new LoginViewModel();
	}
}
