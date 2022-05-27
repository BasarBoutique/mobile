using BazarBoutique.Modelos;
using BazarBoutique.Services;
using BazarBoutique.Services.CursoServices;
using BazarBoutique.Vistas.CarritoVistas;
using BazarBoutique.Vistas.DetallesVistas;
using BazarBoutique.Vistas.FiltrosVistas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace BazarBoutique.VistaModelos.FiltrosViewModels
{
    public class FiltroCursosViewModel : BaseViewModel
    {
        #region Campos
        readonly INavigation navigation;
        readonly ContentPage page;
        string _elementoBusqueda;
        ICursoService ServiciosCursos = DependencyService.Get<ICursoService>();
        #endregion

        #region Propiedad
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

        private LayoutState _estadocursos;

        public LayoutState EstadoCursos
        {
            get => _estadocursos;
            set => SetProperty(ref _estadocursos, value);
        }
        public Command RedireccionCarritoCommand { get; set; }
        public Command VistaFiltroAutorCommand { get; set; }
        public Command VistaFiltroCategoriaCommand { get; set; }
        public Command<Uri> PaginaAnteriorCommand { get; set; }
        public Command<Uri> PaginaSiguienteCommand { get; set; }
        public Command MostrandoFiltrosCommand { get; }
        public Command GestoRefrescamientoCommand { get; }
        public Command EliminandoFiltrosCommand { get; }
        public Command<FiltroPorNombreId> EliminandoFiltroCommand { get; }

        SearchCourseFilters FiltrosRealizados = new SearchCourseFilters()
        {
            filters = new FiltrosModelo() { }
        };

        public Command<PaginaRedireccion> RedireccionPaginaCommand { get; set; }

        DataCursos PaginaDatos = new DataCursos();

        ObservableCollection<CursosModelo> _cursos;
        public ObservableCollection<CursosModelo> Cursos
        {
            get { return _cursos; }
            set { _cursos = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<PaginaRedireccion> _PaginasListadas;

        public ObservableCollection<PaginaRedireccion> PaginasListadas
        {
            get { return _PaginasListadas; }
            set { _PaginasListadas = value;
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

        private ObservableCollection<FiltroPorNombreId> _FiltrosListados;

        public ObservableCollection<FiltroPorNombreId> FiltrosListados
        {
            get { return _FiltrosListados; }
            set
            {
                _FiltrosListados = value;
                OnPropertyChanged();
            }
        }

        private bool _esVisibleFiltros;

        public bool EsVisibleFiltros 
        {
            get => _esVisibleFiltros;
            set => SetProperty(ref _esVisibleFiltros, value);
        }
        public Command<CursosModelo> SeleccionandoCursoCommand { get; set; }

        private Uri LinkPagina;
        #endregion

        public FiltroCursosViewModel(INavigation navigation, ContentPage page, SearchCourseFilters FiltrosRealizados)
        {
            //Instanciando propiedades de navegacion
            this.navigation = navigation;
            this.page = page;
            SeleccionandoCursoCommand = new Command<CursosModelo>(RedireccionACursoEspecifico);

            GestoRefrescamientoCommand = new Command(RecargarCursos);
            FiltrosListados = new ObservableCollection<FiltroPorNombreId>();

            VistaFiltroAutorCommand = new Command(RedireccionFiltroAutores);
            VistaFiltroCategoriaCommand = new Command(RedireccionFiltroCategorias);
            RedireccionCarritoCommand = new Command(RedireccionACarritoPagina);

            RedireccionPaginaCommand = new Command<PaginaRedireccion>(SeleccionandoPagina);
            PaginaAnteriorCommand = new Command<Uri>(PaginaPorBoton);
            PaginaSiguienteCommand = new Command<Uri>(PaginaPorBoton);

            MostrandoFiltrosCommand = new Command(MostrandoFiltros);

            EliminandoFiltrosCommand = new Command(EliminandoTodosLosFiltros);
            EliminandoFiltroCommand = new Command<FiltroPorNombreId>(EliminandoUnFiltro);

            PaginasListadas = new ObservableCollection<PaginaRedireccion>();
            Cursos = new ObservableCollection<CursosModelo>();
            

            LinkPagina = new Uri("https://monolith-stage.herokuapp.com/api/v1/courses/search");

            EstadoCursos = LayoutState.Loading;
        }

        public async void OnAppearing()
        {
            IsBusy = true;
            FiltrosRealizados.filters.withDisabled = false;
            FiltrosRealizados.filters.categories = new List<int>();
            FiltrosRealizados.filters.authors = new List<int>();
            FiltrosRealizados.paginate = 5;

            FiltrosListados.Clear();
            foreach (var arg in FiltrosAlmacenados.AlmacenamientoFiltros)
            {
                FiltrosListados.Add(arg);
            }
            ActualizandoFiltros();

            VerificandoUsuario();

            await EstableciendoValoresDePagina(LinkPagina, FiltrosRealizados);

            IsBusy = false;
        }

        public void RedireccionACarritoPagina()
        {
            navigation.PushAsync(new CarritoVista());
        }

        private void RedireccionACursoEspecifico(CursosModelo obj)
        {
            navigation.PushAsync(new DetallesCursoVista(obj));
        }

        //Metodo para la busqueda de datos en Cursos
        private void BuscandoElementoAsync(string value)
        {
            FiltrosRealizados.filters.title = value;
            IsLoading = true;
            IsLoading = false;
        }


        private async void RecargarCursos()
        {
            IsLoading = true;

            await EstableciendoValoresDePagina(LinkPagina, FiltrosRealizados);

            IsLoading = false;
        }



        private void EliminandoUnFiltro(FiltroPorNombreId obj)
        {
            FiltrosListados.Remove(obj);
            FiltrosAlmacenados.AlmacenamientoFiltros.Remove(obj);

            switch (obj.TipoFiltro)
            {
                case "categories":
                    FiltrosRealizados.filters.categories.Remove(obj.CodigoFiltro);
                    break;
                case "authors":
                    FiltrosRealizados.filters.authors.Remove(obj.CodigoFiltro);
                    break;
                default:
                    break;
            }
            ActualizandoFiltros();


            IsLoading = true;

            IsLoading = false;
        }

        private void EliminandoTodosLosFiltros()
        {
            FiltrosListados.Clear();
            FiltrosAlmacenados.AlmacenamientoFiltros.Clear();
            FiltrosRealizados.filters.categories.Clear();
            FiltrosRealizados.filters.authors.Clear();
            IsLoading = true;

            IsLoading = false;
        }

        private void ActualizandoFiltros()
        {
            FiltrosRealizados.filters.categories.Clear();
            FiltrosRealizados.filters.authors.Clear();
            foreach (var filtros in FiltrosListados)
            {
                switch (filtros.TipoFiltro)
                {
                    case "categories":
                        FiltrosRealizados.filters.categories.Add(filtros.CodigoFiltro);
                        break;

                    case "authors":
                        FiltrosRealizados.filters.authors.Add(filtros.CodigoFiltro);
                        break;
                    default:
                        break;
                }
            }
            //FiltrosListados
        }

        private void MostrandoFiltros()
        {
            EsVisibleFiltros = (EsVisibleFiltros == false);
        }

        private async void SeleccionandoPagina(PaginaRedireccion obj)
        {
            if (PaginaDatos.pagination.current_page != obj.LinkPagina)
                await EstableciendoValoresDePagina(obj.LinkPagina, FiltrosRealizados);
        }

        private void RedireccionFiltroCategorias()
        {
            //Para activar esto debes de configurar la pestaña de categorias para que reciva multiples parametros 

            //FiltrosRealizados.filters.categories.Clear();
            //List<Int16> CategoriasElegidas = new List<Int16>();
            //navigation.PushModalAsync(new FiltroCategoriasVista());

            //var FiltroCategoria = new FiltrosModelo();

            //foreach (var arg in CategoriasElegidas)
            //{
            //    FiltrosRealizados.filters.categories.Add(arg);
            //}

            FiltrosRealizados.filters.categories.Clear();
            navigation.PushAsync(new SoloFiltroCategoriaVista());
        }

        private void RedireccionFiltroAutores()
        {
            //navigation.PushModalAsync(new FiltroAutorVista());

            //Para activar esto debes de configurar la pestaña de auotes para que reciva multiples parametros 

            //FiltrosRealizados.filters.authors.Clear();
            //List<Int16> AutoresElegidos = new List<Int16>();
            //navigation.PushModalAsync(new FiltroAutorVista());

            //var FiltroCategoria = new FiltrosModelo();

            //foreach (var arg in AutoresElegidos)
            //{
            //    FiltrosRealizados.filters.authors.Add(arg);
            //}

            FiltrosRealizados.filters.categories.Clear();
            navigation.PushAsync(new SoloFiltroAutorVista());
            //FiltrosRealizados.filters.authors.Add();
        }

        private async void PaginaPorBoton(Uri uri)
        {
            await EstableciendoValoresDePagina(uri, FiltrosRealizados);
        }

        private async Task EstableciendoValoresDePagina(Uri link, SearchCourseFilters fillter)
        {
            //IsBusy = true;
            //await Task.Delay(1000);
            
            EstadoCursos = LayoutState.Loading;
            if (link != null)
            {
                //Definiendo Filtros
                //fillter.filters.withDisabled = false;
                //fillter.filters.categories = new List<int>();
                //fillter.filters.authors = new List<int>();

                Cursos.Clear();
                PaginaDatos = await ServiciosCursos.GetPaginationCurso(link, fillter);
               
                if (PaginaDatos.courses != null)
                {
                    Cursos.Clear();
                    foreach (var arg in PaginaDatos.courses)
                    {
                        Cursos.Add(arg);
                    }
                    RefrescarPaginaSeleccionada();
                    ParametroPaginaSiguiente = PaginaDatos.pagination.next_page;
                }
                else
                {
                    PaginaDatos.courses = new List<CursosModelo>();
                }
            }
            //IsBusy = false;
            EstadoCursos = LayoutState.Success;
        }

        private void RefrescarPaginaSeleccionada()
        {
            PaginasListadas.Clear();
            for (int i = 0; i < PaginaDatos.pagination.total;)
            {
                i++;
                var Elemento = new PaginaRedireccion();
                //Uri LinkNormal = new Uri();
                Elemento.LinkPagina = new Uri("https://monolith-stage.herokuapp.com/api/v1/courses/search?page=" + i);
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
                        ParametroPaginaAnterior = new Uri("https://monolith-stage.herokuapp.com/api/v1/courses/search?page=" + (i - 1));
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
