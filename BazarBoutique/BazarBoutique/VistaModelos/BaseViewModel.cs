using BazarBoutique.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace BazarBoutique.VistaModelos
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        private bool _estaLogueado;
        public bool EstaLogueado
        {
            get => _estaLogueado;
            set => SetProperty(ref _estaLogueado, value);
        }

        private int contadoCarrito;
        public int ContadoCarrito
        {
            get => contadoCarrito;
            set => SetProperty(ref contadoCarrito, value);
        }

        public void ContandoProductoEnCarrito()
        {
            int cantidad = 0;
            foreach (var elementos in CarroServices.Carritos)
            {
                cantidad ++;
            }
            ContadoCarrito = cantidad;
        }

        //Metodo DesordenaListas
        public static void Shuffle<T>(IList<T> values)
        {
            var n = values.Count;
            var rnd = new Random();
            for (int i = n - 1; i > 0; i--)
            {
                var j = rnd.Next(0, i);
                var temp = values[i];
                values[i] = values[j];
                values[j] = temp;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #region Atributos
        bool isBusy, isLoading = false;
        #endregion

        #region Propiedades

        //Propiedades para 
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                OnPropertyChanged();
            }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
        [CallerMemberName] string propertyName = "",
        Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion

        #region PropiedadDeCambios
        //Esto permite que tu vista se pueda refrescar a ala par de los cambios que haya en el Sistema
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            //PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        #endregion
    }
}
