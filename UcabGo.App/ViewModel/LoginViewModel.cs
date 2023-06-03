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

    [ObservableProperty]
    bool isEnabled;

    public LoginViewModel()
    {
        this.authService = new AuthService();
        isEnabled = true;
    }

    [RelayCommand]
    async void Login()
    {
        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password)) return;

        Token = "Cargando...";
        IsEnabled = false;

        var response = await authService.LoginAsync(Email, Password);

        Token = response.Token;
        IsEnabled = true;
    }

    //readonly Func<Action, Task> UpdateUI = async (action) =>
    //{
    //    action.Invoke();
    //    await Task.Delay(1);
    //};
}
