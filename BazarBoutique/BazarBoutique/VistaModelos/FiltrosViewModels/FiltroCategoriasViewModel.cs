using BazarBoutique.Modelos;
using BazarBoutique.Services.CategoriaServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public Command<Uri> PaginaAnteriorCommand { get; set; }
        public Command<Uri> PaginaSiguienteCommand { get; set; }
        public Command<PaginaRedireccion> RedireccionPaginaCommand { get; set; }
        DataCategorias PaginaDatos = new DataCategorias();

        public ObservableCollection<CategoriaModelo> CatalogosCategorias
        {
            get => categoriasLista;
            set
            {
                categoriasLista = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<PaginaRedireccion> _PaginasListadas;

        public ObservableCollection<PaginaRedireccion> PaginasListadas
        {
            get => _PaginasListadas; 
            set
            {
                _PaginasListadas = value;
                OnPropertyChanged();
            }
        }

        //Propiedad para boton de regreso de pagina
        private Color _BackButtonColor;
        public Color BackButtonColor
        {
            get => _BackButtonColor;
            set
            {
                SetProperty(ref _BackButtonColor, value);
            }
        }

        //Propiedad para boton de pagina siguiente
        private Color _NextButtonColor;
        public Color NextButtonColor
        {
            get
            {
                return _NextButtonColor;
            }
            set
            {
                SetProperty(ref _NextButtonColor, value);
            }
        }

        private Uri _ParametroPaginaAnterior;

        public Uri ParametroPaginaAnterior
        {
            get => _ParametroPaginaAnterior;
            set{
                BackButtonColor = value == null ? Color.Gray : Color.Green;
                SetProperty(ref _ParametroPaginaAnterior, value);
            }
        }

        private Uri _ParametroPaginaSiguiente;

        public Uri ParametroPaginaSiguiente
        {
            get => _ParametroPaginaSiguiente;
            set {
                    NextButtonColor = value == null ? Color.Gray : Color.Green;
                    SetProperty(ref _ParametroPaginaSiguiente, value);
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


            RedireccionPaginaCommand = new Command<PaginaRedireccion>(SeleccionandoPagina);
            PaginaAnteriorCommand = new Command<Uri>(PaginaAnteriorCommsand);
            PaginaSiguienteCommand = new Command<Uri>(PaginaAnteriorCommsand);

            PaginasListadas = new ObservableCollection<PaginaRedireccion>();
            CatalogosCategorias = new ObservableCollection<CategoriaModelo>();

            EstadoCategoria = LayoutState.Loading;
        }

        private void PaginaAnteriorCommsand(Uri uri)
        {
            EstableciendoValoresDePagina(uri);            
        }

        private async void EstableciendoValoresDePagina(Uri link)
        {
            IsBusy = true;

            if (link != null)
            {
                PaginaDatos = await ServicioCategoria.GetPaginacionCategoria(link);
                CatalogosCategorias.Clear();
                if(PaginaDatos.categories != null)
                {
                    foreach (var arg in PaginaDatos.categories)
                    {
                        CatalogosCategorias.Add(arg);
                    }
                    RefrescarPaginaSeleccionada();
                    ParametroPaginaSiguiente = PaginaDatos.pagination.next_page;
                }
                else
                {
                    PaginaDatos.categories = new List<CategoriaModelo>();
                }
            }
            IsBusy = false;
            EstadoCategoria = LayoutState.Success;

        }


        private void SeleccionandoPagina(PaginaRedireccion obj)
        {
            if (PaginaDatos.pagination.current_page != obj.LinkPagina)
                EstableciendoValoresDePagina(obj.LinkPagina);   
        }

        private void RefrescarPaginaSeleccionada()
        {
            PaginasListadas.Clear();
            for (int i = 0; i < PaginaDatos.pagination.total;)
            {
                i++;
                var Elemento = new PaginaRedireccion();
                Elemento.LinkPagina = new Uri("https://monolith-stage.herokuapp.com/api/v1/categories/all?page=" + i);
                Elemento.NumeroPagina = i;
                    
                if (PaginaDatos.pagination.current_page == Elemento.LinkPagina)
                {
                    Elemento.PaginaSeleccionada = true;
                    if (i <= 1)
                    {
                        ParametroPaginaAnterior = null;
                    }
                    else
                    {
                        ParametroPaginaAnterior = new Uri("https://monolith-stage.herokuapp.com/api/v1/categories/all?page=" + (i - 1));
                    }
                }
                else
                {
                    Elemento.PaginaSeleccionada = false;
                }
                PaginasListadas.Add(Elemento);
            }

            
        }

        private async void BuscandoElementoAsync(string obj)
        {

        }


        public async void OnAppearing()
        {
            Uri direccion = new Uri("https://monolith-stage.herokuapp.com/api/v1/categories/all");
            EstableciendoValoresDePagina(direccion);

            
        }
    }
}
