using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace BazarBoutique.VistaModelos.FiltrosViewModels
{
    public class FiltroAutoresViewModel : BaseViewModel
    {
        #region Campos
        private readonly INavigation navigation;
        private readonly ContentPage page;
        private string _elementoBusquedaAutores;
        #endregion


        #region Propiedades

        public string ElementoBusquedaAutores
        {
            get { 
                return _elementoBusquedaAutores; 
            }
            set {
                SetProperty(ref _elementoBusquedaAutores, value);
            }
        }
        public Command PaginaAnteriorCommand { get; set; }
        public Command PaginaSiguienteCommand { get; set; }

        //private ObservableCollection _listAutores;

        //public ObservableCollection ListaAutores
        //{
        //    get { return myVar; }
        //    set { myVar = value; }
        //}


        #endregion
        public FiltroAutoresViewModel(INavigation navigation, ContentPage page)
        {
            //Instanciando propiedades de navegacion
            this.navigation = navigation;
            this.page = page;
        }

        public void OnAppearing()
        {
            IsBusy = true;
            //CatalogosCategorias = new ObservableCollection<CategoriaModelo>(await ServicioCategoria.GetCategorias());
            IsBusy = false;
        }
    }
}
