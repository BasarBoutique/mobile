using BazarBoutique.Modelos;
using BazarBoutique.Services;
using BazarBoutique.Services.CursoServices;
using BazarBoutique.Vistas.FiltrosVistas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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
                //BuscandoElementoAsync(value);
            }
        }

        private LayoutState _estadocursos;

        public LayoutState EstadoCursos
        {
            get => _estadocursos;
            set => SetProperty(ref _estadocursos, value);
        }

        public Command VistaFiltroAutorCommand { get; set; }
        public Command VistaFiltroCategoriaCommand { get; set; }
        public Command<Uri> PaginaAnteriorCommand { get; set; }
        public Command<Uri> PaginaSiguienteCommand { get; set; }
        public Command MostrandoFiltrosCommand { get; }

        public Command EliminandoFiltrosCommand { get; }
        public Command<FiltroPorNombreId> EliminandoFiltroCommand { get; }

        SearchCourseFilters FiltrosRealizados = new SearchCourseFilters() {
            filters = new FiltrosModelo(){}
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


        #endregion

        public FiltroCursosViewModel(INavigation navigation, ContentPage page, SearchCourseFilters FiltrosRealizados)
        {
            //Instanciando propiedades de navegacion
            this.navigation = navigation;
            this.page = page;

            FiltrosListados = new ObservableCollection<FiltroPorNombreId>();

            VistaFiltroAutorCommand = new Command(RedireccionFiltroAutores);
            VistaFiltroCategoriaCommand = new Command(RedireccionFiltroCategorias);

            RedireccionPaginaCommand = new Command<PaginaRedireccion>(SeleccionandoPagina);
            PaginaAnteriorCommand = new Command<Uri>(PaginaPorBoton);
            PaginaSiguienteCommand = new Command<Uri>(PaginaPorBoton);


            MostrandoFiltrosCommand = new Command(MostrandoFiltros);

            EliminandoFiltrosCommand = new Command(EliminandoUnFiltro);
            EliminandoFiltroCommand = new Command<FiltroPorNombreId>(EliminandoTodosLosFiltros);

            PaginasListadas = new ObservableCollection<PaginaRedireccion>();
            Cursos = new ObservableCollection<CursosModelo>();
            EstadoCursos = LayoutState.Loading;
        }

        public async void OnAppearing()
        {
            IsBusy = true;

            //bool MuestrenDatos = false; 
            ////Cursos = new ObservableCollection<CursosModelo>(await ServiciosCursos.GetCurso(true));
            //FiltrosRealizados.filters.title = "";
            //FiltrosRealizados.filters.categories = new List<int>();
            //FiltrosRealizados.filters.authors = new List<int>();

            Uri link = new Uri("https://monolith-stage.herokuapp.com/api/v1/courses/search");

            //PaginaDatos = await ServiciosCursos.GetPaginationCurso(FiltrosRealizados, link);

            //Cursos = new ObservableCollection<CursosModelo>(PaginaDatos.courses);
            //PaginasListadas = new ObservableCollection<PaginaRedireccion>();

            //for (int i = 0; i < PaginaDatos.pagination.total; )
            //{
            //    i++;
            //    PaginasListadas.Add(new PaginaRedireccion
            //    {
            //        //HttpResponseMessage response = await clie.GetAsync("?" + parametro + "=" + Convert.ToString(withDisabled).ToLower());
            //        LinkPagina = new Uri("https://monolith-stage.herokuapp.com/api/v1/courses/search?page=" + i) ,
            //        NumeroPagina = i
            //    });
            //}

            EstableciendoValoresDePagina(link, FiltrosRealizados);

            EstadoCursos = LayoutState.Success;
            IsBusy = false;

            FiltrosListados.Clear();
            foreach (var arg in FiltrosAlmacenados.AlmacenamientoFiltros)
            {
                FiltrosListados.Add(arg);
            }

        }

        private void EliminandoTodosLosFiltros(FiltroPorNombreId obj)
        {
            Application.Current.MainPage.DisplayAlert("Work in progres", "Has Eliminado el "+obj.NombreFiltro , "Ok");
        }

        private void EliminandoUnFiltro()
        {
            Application.Current.MainPage.DisplayAlert("Work in progres", "Has Eliminado todos los productos", "Ok");
        }

        private void MostrandoFiltros()
        {
            EsVisibleFiltros = (EsVisibleFiltros == false) ?  true : false;
            
        }

        private void SeleccionandoPagina(PaginaRedireccion obj)
        {
            if (PaginaDatos.pagination.current_page != obj.LinkPagina)
                EstableciendoValoresDePagina(obj.LinkPagina, FiltrosRealizados);
        }

        //private async void RecargandoElementosPagina(PaginaRedireccion obj)
        //{
        //    IsBusy = true;
        //    PaginaDatos = await ServiciosCursos.GetPaginationCurso(FiltrosRealizados, obj.LinkPagina);
        //    Cursos.Clear();

        //    foreach (var arg in PaginaDatos.courses)
        //    {
        //        Cursos.Add(arg);
        //    }
        //    IsBusy = false;
        //}

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
            //FiltrosRealizados.filters.categories.Add();
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
            navigation.PushModalAsync(new FiltroAutorVista());
            //FiltrosRealizados.filters.authors.Add();
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

                fillter.filters.withDisabled = false;
                fillter.filters.title = "";

                fillter.filters.categories = new List<int>();
                fillter.filters.authors = new List<int>();

                PaginaDatos = await ServiciosCursos.GetPaginationCurso(link, fillter);
                Cursos.Clear();
                if (PaginaDatos.courses != null)
                {
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
            IsBusy = false;
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
