using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BazarBoutique.Vistas.BarraNavegacion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuLateralVistaFlyout : ContentPage
    {
        public ListView ListView;

        public MenuLateralVistaFlyout()
        {
            InitializeComponent();

            BindingContext = new MenuLateralVistaFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        private class MenuLateralVistaFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MenuLateralVistaFlyoutMenuItem> MenuItems { get; set; }

            public MenuLateralVistaFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<MenuLateralVistaFlyoutMenuItem>(new[]
                {
                    new MenuLateralVistaFlyoutMenuItem { Id = 0, Title = "Page 1" },
                    new MenuLateralVistaFlyoutMenuItem { Id = 1, Title = "Page 2" },
                    new MenuLateralVistaFlyoutMenuItem { Id = 2, Title = "Page 3" },
                    new MenuLateralVistaFlyoutMenuItem { Id = 3, Title = "Page 4" },
                    new MenuLateralVistaFlyoutMenuItem { Id = 4, Title = "Page 5" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}