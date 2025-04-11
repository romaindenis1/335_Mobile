using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5ListView.ViewModels
{
    public sealed partial class CrudViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<string> wishes = new() { "décrocher la lune", "voler dans les airs" };

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddWishCommand))]
        private string wishEntry = "";

        [RelayCommand(CanExecute = nameof(AddWishCanExecute))]
        private void AddWish(string wish)
        {
            Wishes.Add(wish);

            WishEntry = "";
        }

        private bool AddWishCanExecute()
        {
            return !string.IsNullOrEmpty(WishEntry);
        }
    }
}