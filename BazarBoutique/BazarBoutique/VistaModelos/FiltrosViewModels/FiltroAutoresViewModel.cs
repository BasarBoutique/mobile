using BazarBoutique.Modelos;
using BazarBoutique.Services.UsuarioServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace BazarBoutique.VistaModelos.FiltrosViewModels
{
    public class FiltroAutoresViewModel : BaseViewModel
    {
        #region Campos
        private readonly INavigation navigation;
        private readonly ContentPage page;
        private string _elementoBusquedaAutores;
        IUsuarioServices ServicioUsuarios = DependencyService.Get<IUsuarioServices>();
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
        public Command<Uri> PaginaAnteriorCommand { get; set; }
        public Command<Uri> PaginaSiguienteCommand { get; set; }
        public Command<PaginaRedireccion> RedireccionPaginaCommand { get; set; }

        DataUsuario PaginaDatos = new DataUsuario();

        private ObservableCollection<UsuarioModelo> usuariolista;
        public ObservableCollection<UsuarioModelo> UsuarioLista
        {
            get => usuariolista;
            set
            {
                usuariolista = value;
                OnPropertyChanged();
            }
        }

        //Paginacion 
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

        private Uri _ParametroPaginaAnterior;
        public Uri ParametroPaginaAnterior
        {
            get => _ParametroPaginaAnterior;
            set
            {
                BackButtonColor = value == null ? Color.Gray : Color.Green;
                SetProperty(ref _ParametroPaginaAnterior, value);
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

        private Uri _ParametroPaginaSiguiente;
        public Uri ParametroPaginaSiguiente
        {
            get => _ParametroPaginaSiguiente;
            set
            {
                NextButtonColor = value == null ? Color.Gray : Color.Green;
                SetProperty(ref _ParametroPaginaSiguiente, value);
            }
        }

        private LayoutState _estadoAutores;

        public LayoutState EstadoAutores
        {
            get => _estadoAutores;
            set => SetProperty(ref _estadoAutores, value);
        }
        public FiltrosModelo Filtros = new FiltrosModelo() { };


        SearchCourseFilters FiltrosRealizados = new SearchCourseFilters()
        {
            filters = new FiltrosModelo() { }
        };

        #endregion
        public FiltroAutoresViewModel(INavigation navigation, ContentPage page)
        {
            //Instanciando propiedades de navegacion
            this.navigation = navigation;
            this.page = page;

            RedireccionPaginaCommand = new Command<PaginaRedireccion>(SeleccionandoPagina);
            PaginaAnteriorCommand = new Command<Uri>(PaginaPorBoton);
            PaginaSiguienteCommand = new Command<Uri>(PaginaPorBoton);

            PaginasListadas = new ObservableCollection<PaginaRedireccion>();
            UsuarioLista = new ObservableCollection<UsuarioModelo>();


            //FiltrosRealizados.filters.categories = new List<int>();
            //FiltrosRealizados.filters.authors = new List<int>();

        }

        public void OnAppearing()
        {
            Uri direccion = new Uri("https://monolith-stage.herokuapp.com/api/v1/auth/users/search");
            
            //Filtros.title = "";
            //Filtros.WhitDisabled = false;
            //Filtros.roles.Add(3);
            EstableciendoValoresDePagina(direccion, FiltrosRealizados);
            //IsBusy = true;
            ////CatalogosCategorias = new ObservableCollection<CategoriaModelo>(await ServicioCategoria.GetCategorias());
            //IsBusy = false;
        }

        private void SeleccionandoPagina(PaginaRedireccion obj)
        {
            if (PaginaDatos.paginate.current_page != obj.LinkPagina)
                EstableciendoValoresDePagina(obj.LinkPagina, FiltrosRealizados);
        }

        
        private void PaginaPorBoton(Uri uri)
        {
            EstableciendoValoresDePagina(uri, FiltrosRealizados);
        }

        private async void EstableciendoValoresDePagina(Uri link, SearchCourseFilters fillter)
        {
            IsBusy = true;

            if (link != null)
            {
                //PaginaDatos.users = new List<UsuarioModelo>();
                //PaginaDatos.filters = new FiltrosModelo();
                //PaginaDatos.users.Add(new UsuarioModelo{id=1 });

                fillter.filters.withDisabled = false;
                fillter.filters.roles = new List<int>();
                fillter.filters.roles.Add(3);
                fillter.filters.title = "";

                PaginaDatos = await ServicioUsuarios.GetPaginacionUsuario(link,fillter);
                UsuarioLista.Clear();
                if (PaginaDatos.users != null)
                {
                    foreach (var arg in PaginaDatos.users)
                    {
                        UsuarioLista.Add(arg);
                    }
                    RefrescarPaginaSeleccionada();
                    ParametroPaginaSiguiente = PaginaDatos.paginate.next_page;
                }
                else
                {
                    PaginaDatos.users = new List<UsuarioModelo>();
                }
            }
            IsBusy = false;
            EstadoAutores = LayoutState.Success;
        }

        private void RefrescarPaginaSeleccionada()
        {
            PaginasListadas.Clear();
            for (int i = 0; i < PaginaDatos.paginate.total;)
            {
                i++;
                var Elemento = new PaginaRedireccion();
                //Uri LinkNormal = new Uri();
                Elemento.LinkPagina = new Uri("https://monolith-stage.herokuapp.com/api/v1/auth/users/search?page=" + i);
                Elemento.NumeroPagina = i;

                if (PaginaDatos.paginate.current_page == Elemento.LinkPagina)
                {
                    Elemento.PaginaSeleccionada = true;
                    if (i <= 1)
                    {
                        ParametroPaginaAnterior = null;
                    }
                    else
                    {
                        ParametroPaginaAnterior = new Uri("https://monolith-stage.herokuapp.com/api/v1/auth/users/search?page=" + (i - 1));
                    }
                }
                else
                {
                    Elemento.PaginaSeleccionada = false;
                }
                PaginasListadas.Add(Elemento);
            }
        }



    }
}
