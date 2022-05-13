using BazarBoutique.Modelos;
using BazarBoutique.Services;
using BazarBoutique.Services.UsuarioServices;
using BazarBoutique.Vistas.FiltrosVistas;
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
                BuscandoElementoAsync(value);
            }
        }
        public Command<Uri> PaginaAnteriorCommand { get; set; }
        public Command<Uri> PaginaSiguienteCommand { get; set; }
        public Command GestoRefrescamientoCommand { get; }
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
        //public FiltrosModelo Filtros = new FiltrosModelo() { };

        public Command<UsuarioModelo> RedirigirApartadoCursoAutorCommand { get; set; }

        SearchCourseFilters FiltrosRealizados = new SearchCourseFilters()
        {
            filters = new FiltrosModelo() { }
        };
        private Uri LinkPagina;
        #endregion
        public FiltroAutoresViewModel(INavigation navigation, ContentPage page)
        {
            //Instanciando propiedades de navegacion
            this.navigation = navigation;
            this.page = page;

            GestoRefrescamientoCommand = new Command(RecargarAutores);
            RedireccionPaginaCommand = new Command<PaginaRedireccion>(SeleccionandoPagina);
            PaginaAnteriorCommand = new Command<Uri>(PaginaPorBoton);
            PaginaSiguienteCommand = new Command<Uri>(PaginaPorBoton);

            PaginasListadas = new ObservableCollection<PaginaRedireccion>();
            UsuarioLista = new ObservableCollection<UsuarioModelo>();
            LinkPagina = new Uri("https://monolith-stage.herokuapp.com/api/v1/auth/users/search");

            RedirigirApartadoCursoAutorCommand = new Command<UsuarioModelo>(RedireciconApartadoCursos);

            EstadoAutores = LayoutState.Loading;
            //FiltrosRealizados.filters.categories = new List<int>();
            //FiltrosRealizados.filters.authors = new List<int>();

        }

        private void RedireciconApartadoCursos(UsuarioModelo obj)
        {
            FiltrosAlmacenados.AlmacenamientoFiltros.Clear();
            var ElementoSeleccionado = new FiltroPorNombreId
            {
                CodigoFiltro = obj.id,
                NombreFiltro = obj.detail.fullname,
                TipoFiltro = "authors"
            };

            bool Existe = FiltrosAlmacenados.AlmacenamientoFiltros.Any(x => x.CodigoFiltro == ElementoSeleccionado.CodigoFiltro);

            //Confirmar que el articulo no exista
            if (!Existe)
            {
                //Aqui se activa
                FiltrosAlmacenados.AlmacenamientoFiltros.Add(ElementoSeleccionado);
                //Comprobar si esta habilitado o no
            }
            SearchCourseFilters FiltrosRealizados = new SearchCourseFilters();
            navigation.PushAsync(new FiltroCursoVista(FiltrosRealizados));
        }

        public async void OnAppearing()
        {
            //Uri direccion = new Uri("https://monolith-stage.herokuapp.com/api/v1/auth/users/search");
            FiltrosRealizados.paginate = 4;
            //Filtros.title = "";
            FiltrosRealizados.filters.withDisabled = false;
            //Filtros.roles.Add(3);

            await EstableciendoValoresDePagina(LinkPagina, FiltrosRealizados);

            //IsBusy = true;
            ////CatalogosCategorias = new ObservableCollection<CategoriaModelo>(await ServicioCategoria.GetCategorias());
            //IsBusy = false;
        }

        private void BuscandoElementoAsync(string value)
        {
            //FiltrosRealizados.filters.name = value;
            //IsLoading = true;
            //IsLoading = false;
        }
        private async void RecargarAutores()
        {
            IsLoading = true;

            await EstableciendoValoresDePagina(LinkPagina, FiltrosRealizados);

            IsLoading = false;
        }

        private async void SeleccionandoPagina(PaginaRedireccion obj)
        {
            if (PaginaDatos.paginate.current_page != obj.LinkPagina)
                await EstableciendoValoresDePagina(obj.LinkPagina, FiltrosRealizados);
        }

        
        private async void PaginaPorBoton(Uri uri)
        {
            await EstableciendoValoresDePagina(uri, FiltrosRealizados);
        }

        private async Task EstableciendoValoresDePagina(Uri link, SearchCourseFilters fillter)
        {
            EstadoAutores = LayoutState.Loading;
            //IsBusy = true;

            if (link != null)
            {
                //PaginaDatos.users = new List<UsuarioModelo>();
                //PaginaDatos.filters = new FiltrosModelo();
                //PaginaDatos.users.Add(new UsuarioModelo{id=1 });

                fillter.filters.withDisabled = false;
                fillter.filters.roles = new List<int>
                {
                    3
                };
                //fillter.filters.title = "";
                UsuarioLista.Clear();
                PaginaDatos = await ServicioUsuarios.GetPaginacionUsuario(link,fillter);
                
                if (PaginaDatos.users != null)
                {
                    UsuarioLista.Clear();
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
            //IsBusy = false;
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
