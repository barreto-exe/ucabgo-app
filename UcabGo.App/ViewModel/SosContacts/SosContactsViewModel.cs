using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UcabGo.App.Models;
using UcabGo.App.Services;

namespace UcabGo.App.ViewModel
{
    public partial class SosContactsViewModel : ViewModelBase
    {
        [ObservableProperty]
        ObservableCollection<SosContact> contacts;

        public SosContactsViewModel(ISettingsService settingsService, INavigationService navigation) : base(settingsService, navigation)
        {
            contacts = new ObservableCollection<SosContact>
            {
                //Test data
                new SosContact { Id = 1, Name = "Juan", Phone = "0414-1234567" },
                new SosContact { Id = 2, Name = "Pedro", Phone = "0414-7654321" },
                new SosContact { Id = 3, Name = "Maria", Phone = "0414-1234567" }
            };
        }
    }
}
