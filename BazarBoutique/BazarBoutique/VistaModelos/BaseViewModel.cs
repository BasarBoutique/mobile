using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace BazarBoutique.VistaModelos
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Atributos
        public bool isBusy = false;
        #endregion

        #region Propiedades
        public bool Isbusy
        {
            get { return isBusy = false; }
            set
            {
                isBusy = value;
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
