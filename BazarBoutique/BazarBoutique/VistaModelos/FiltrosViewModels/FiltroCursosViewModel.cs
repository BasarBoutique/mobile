using BazarBoutique.Modelos;
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
        public Command PaginaAnteriorCommand { get; set; }
        public Command PaginaSiguienteCommand { get; set; }

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

        #endregion

        public FiltroCursosViewModel(INavigation navigation, ContentPage page)
        {
            //Instanciando propiedades de navegacion
            this.navigation = navigation;
            this.page = page;

            VistaFiltroAutorCommand = new Command(RedireccionFiltroAutores);
            VistaFiltroCategoriaCommand = new Command(RedireccionFiltroCategorias);
            RedireccionPaginaCommand = new Command<PaginaRedireccion>(RecargandoElementosPagina);
           
            EstadoCursos = LayoutState.Loading;
        }
        private async void RecargandoElementosPagina(PaginaRedireccion obj)
        {
            IsBusy = true;
            PaginaDatos = await ServiciosCursos.GetPaginationCurso(FiltrosRealizados, obj.LinkPagina);
            Cursos.Clear();

            foreach (var arg in PaginaDatos.courses)
            {
                Cursos.Add(arg);
            }
            IsBusy = false;
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
            navigation.PushModalAsync(new FiltroCategoriasVista());
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

        public async void OnAppearing()
        {
            IsBusy = true;

            bool MuestrenDatos = false; 
            //Cursos = new ObservableCollection<CursosModelo>(await ServiciosCursos.GetCurso(true));
            FiltrosRealizados.filters.title = "";
            FiltrosRealizados.filters.categories = new List<int>();
            FiltrosRealizados.filters.authors = new List<int>();

            Uri link = new Uri("https://monolith-stage.herokuapp.com/api/v1/courses/search");

            PaginaDatos = await ServiciosCursos.GetPaginationCurso(FiltrosRealizados, link);

            Cursos = new ObservableCollection<CursosModelo>(PaginaDatos.courses);
            PaginasListadas = new ObservableCollection<PaginaRedireccion>();

            for (int i = 0; i < PaginaDatos.pagination.total; )
            {
                i++;
                PaginasListadas.Add(new PaginaRedireccion
                {
                    //HttpResponseMessage response = await clie.GetAsync("?" + parametro + "=" + Convert.ToString(withDisabled).ToLower());
                    LinkPagina = new Uri("https://monolith-stage.herokuapp.com/api/v1/courses/search?page=" + i) ,
                    NumeroPagina = i
                });

                
            }

            EstadoCursos = LayoutState.Success;
            IsBusy = false;
        }

        public async void CargandoPaginacion()
        {

        }


    }
}
