using BazarBoutique.Modelos;
using BazarBoutique.Services;
using BazarBoutique.Services.UsuarioServices;
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
    public class SoloFiltroAutoresViewModel : BaseViewModel
    {
        #region Campos
        private ObservableCollection<UsuarioModelo> _usuariolista;
        private readonly INavigation navigation;
        private readonly ContentPage page;
        private string _elementoBusqueda;
        IUsuarioServices ServicioAutores = DependencyService.Get<IUsuarioServices>();
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
        public Command GestoRefrescamientoCommand { get; }
        public Command<Uri> PaginaAnteriorCommand { get; set; }
        public Command<Uri> PaginaSiguienteCommand { get; set; }
        public Command<PaginaRedireccion> RedireccionPaginaCommand { get; set; }
        DataUsuario PaginaDatos = new DataUsuario();

        public ObservableCollection<UsuarioModelo> CatalogosUsuario
        {
            get => _usuariolista;
            set
            {
                _usuariolista = value;
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
            set
            {
                BackButtonColor = value == null ? Color.Gray : Color.Green;
                SetProperty(ref _ParametroPaginaAnterior, value);
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

        private LayoutState _estadoListaAutores;

        public LayoutState EstadoAutoresLista
        {
            get => _estadoListaAutores;
            set => SetProperty(ref _estadoListaAutores, value);
        }

        //Botones Filtros
        public Command<UsuarioModelo> CambiandoEstadoFiltroCommand { get; set; }

        SearchCourseFilters FiltrosRealizados = new SearchCourseFilters()
        {
            filters = new FiltrosModelo() { }
        };
        private Uri LinkPagina;
        #endregion


        public SoloFiltroAutoresViewModel(INavigation navigation, ContentPage page)
        {
            this.navigation = navigation;
            this.page = page;

            GestoRefrescamientoCommand = new Command(RecargarAutores);
            RedireccionPaginaCommand = new Command<PaginaRedireccion>(SeleccionandoPagina);
            PaginaAnteriorCommand = new Command<Uri>(PaginaPorBoton);
            PaginaSiguienteCommand = new Command<Uri>(PaginaPorBoton);
            CambiandoEstadoFiltroCommand = new Command<UsuarioModelo>(CambiandoEstadoFiltro);

            PaginasListadas = new ObservableCollection<PaginaRedireccion>();
            CatalogosUsuario = new ObservableCollection<UsuarioModelo>();

            LinkPagina = new Uri("https://monolith-stage.herokuapp.com/api/v1/auth/users/search");

            EstadoAutoresLista = LayoutState.Loading;
        }
        public async void OnAppearing()
        {
            FiltrosRealizados.paginate = 6;
            FiltrosRealizados.filters.withDisabled = false;

            await EstableciendoValoresDePagina(LinkPagina, FiltrosRealizados);
        }
        private void BuscandoElementoAsync(string value)
        {
            FiltrosRealizados.filters.title = value;
            IsLoading = true;
            IsLoading = false;
        }

        private async void RecargarAutores()
        {
            IsLoading = true;

            await EstableciendoValoresDePagina(LinkPagina, FiltrosRealizados);

            IsLoading = false;
        }

        private void CambiandoEstadoFiltro(UsuarioModelo obj)
        {

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

                //Comprobar si esta habilitado o no
                if (obj.EstaSeleccionado)
                {
                    //Aqui se activa
                    FiltrosAlmacenados.AlmacenamientoFiltros.Add(ElementoSeleccionado);
                }
                else
                {
                    //Aqui se elimina
                    FiltrosAlmacenados.AlmacenamientoFiltros.Remove(ElementoSeleccionado);
                }
            }
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
            //IsBusy = true;
            EstadoAutoresLista = LayoutState.Loading;

            if (link != null)
            {

                fillter.filters.roles = new List<int>
                {
                    3
                };

                CatalogosUsuario.Clear();
                PaginaDatos = await ServicioAutores.GetPaginacionUsuario(link, FiltrosRealizados);
                if (PaginaDatos.users != null)
                {
                    foreach (var arg in PaginaDatos.users)
                    {
                        //Esto detectara los filtros existentes y los activara
                        foreach (var item in FiltrosAlmacenados.AlmacenamientoFiltros)
                        {
                            if (item.TipoFiltro == "authors" && item.CodigoFiltro == arg.id)
                                arg.EstaSeleccionado = true;
                        }
                        CatalogosUsuario.Add(arg);
                    }
                    RefrescarPaginaSeleccionada(link);
                    ParametroPaginaSiguiente = PaginaDatos.paginate.next_page;
                }
                else
                {
                    PaginaDatos.users = new List<UsuarioModelo>();
                }
            }
            //IsBusy = false;
            EstadoAutoresLista = LayoutState.Success;

        }

        private void RefrescarPaginaSeleccionada(Uri link)
        {
            PaginasListadas.Clear();
            for (int i = 0; i < PaginaDatos.paginate.total;)
            {
                i++;
                var Elemento = new PaginaRedireccion();
                Elemento.LinkPagina = new Uri( link+ "?page=" + i);
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
                        ParametroPaginaAnterior = new Uri(link + "?page=" + (i - 1));
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
