using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UcabGo.App.ApiAccess.Interfaces;
using UcabGo.App.ApiAccess.Services;

namespace UcabGo.App.ViewModel;

public partial class LoginViewModel : ObservableObject
{
    readonly IAuthService authService;

    [ObservableProperty]
    string email;

    [ObservableProperty]
    string password;

    [ObservableProperty]
    string token;

    public LoginViewModel()
    {
        this.authService = new AuthService();
    }

    [RelayCommand]
    async void Login()
    {
        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password)) return;

        var response = await authService.LoginAsync(Email, Password);
        Token = response.Token;
    }
}
