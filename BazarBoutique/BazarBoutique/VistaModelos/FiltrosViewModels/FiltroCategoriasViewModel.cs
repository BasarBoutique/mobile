using BazarBoutique.Modelos;
using BazarBoutique.Services.CategoriaServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace BazarBoutique.VistaModelos.FiltrosViewModels
{
    public class FiltroCategoriasViewModel : BaseViewModel
    {
        #region Campos
        private ObservableCollection<CategoriaModelo> categoriasLista;
        private readonly INavigation navigation;
        private readonly ContentPage page;
        private string _elementoBusqueda;
        ICategoryService ServicioCategoria = DependencyService.Get<ICategoryService>();

        #endregion

        #region Propiedades
        public string ElementoBusqueda
        {
            get
            {
                return _elementoBusqueda;
            }
            set
            {
                SetProperty(ref _elementoBusqueda, value);
                BuscandoElementoAsync(value);
            }
        }
        public Command PaginaAnteriorCommand { get; set; }
        public Command PaginaSiguienteCommand { get; set; }

        public ObservableCollection<CategoriaModelo> CatalogosCategorias
        {
            get => categoriasLista;
            set
            {
                categoriasLista = value;
                OnPropertyChanged();
            }
        }

        private LayoutState _estadoCategoria;

        public LayoutState EstadoCategoria
        {
            get => _estadoCategoria;
            set => SetProperty(ref _estadoCategoria, value);
        }

        #endregion
        public FiltroCategoriasViewModel(INavigation navigation, ContentPage page)
        {
            //Instanciando propiedades de navegacion
            this.navigation = navigation;
            this.page = page;

            EstadoCategoria = LayoutState.Loading; 
        }

        private async void BuscandoElementoAsync(string obj)
        {
            //IsBusy = true;
            var ElementosEnCatagorias = await ServicioCategoria.GetCategoriaSlide();
            if (string.IsNullOrEmpty(obj))
            {
                CatalogosCategorias = new ObservableCollection<CategoriaModelo>(ElementosEnCatagorias);
            }
            else
            {
                CatalogosCategorias = new ObservableCollection<CategoriaModelo>(ElementosEnCatagorias.Where(e => e.category.ToLower().Contains(obj.ToLower())));
            }
            //IsBusy = false;
        }


        public async void OnAppearing()
        {
            IsBusy = true;
            CatalogosCategorias = new ObservableCollection<CategoriaModelo>(await ServicioCategoria.GetCategoriaSlide());
            EstadoCategoria = LayoutState.Success;
            IsBusy = false;
        }
    }
}
