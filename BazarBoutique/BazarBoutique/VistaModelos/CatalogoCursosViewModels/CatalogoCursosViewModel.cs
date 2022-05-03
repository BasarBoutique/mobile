using BazarBoutique.Modelos;
using BazarBoutique.Services.CategoriaServices;
using BazarBoutique.Services.CursoServices;
using BazarBoutique.Services.UsuarioServices;
using BazarBoutique.Vistas.FiltrosVistas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace BazarBoutique.VistaModelos.CatalogoCursosViewModels
{
    public class CatalogoCursosViewModel : BaseViewModel
    {
        private readonly INavigation navigation;
        private readonly ContentPage page;
        private bool IsInitialized;
        private ObservableCollection<CategoriaSlideModelo> categoriasLista;
        private ObservableCollection<CursosModelo> cursosLista;

        ICategoryService ServicioCategoria = DependencyService.Get<ICategoryService>();
        ICursoService ServicioCursos = DependencyService.Get<ICursoService>();
        IUsuarioServices ServicioUsuarios = DependencyService.Get<IUsuarioServices>();

        public ObservableCollection<CategoriaSlideModelo> Categorias 
        {
            get => categoriasLista;
            set
            {
                categoriasLista = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<CursosModelo> Cursos
        {
            get => cursosLista;
            set
            {
                cursosLista = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<UsuarioModelo> _listaAutores;
       
        public ObservableCollection<UsuarioModelo> Autores
        {
            get => _listaAutores;
            set
            {
                _listaAutores = value;
                OnPropertyChanged();
            }
        }

        private LayoutState _estadoactual, _estadocursos;

        public LayoutState EstadoActual
        {
            get => _estadoactual;
            set => SetProperty(ref _estadoactual, value);
        }

        public LayoutState EstadoCursos
        {
            get => _estadocursos;
            set => SetProperty(ref _estadocursos, value);
        }


        public Command RedireccionApartadoCategorias { get; set; }
        public Command RedireccionApartadoAutores { get; set; }
        
        public Command<CategoriaSlideModelo> RedireccionApartadoCursos { get; set; }

        public CatalogoCursosViewModel(INavigation navigation, ContentPage page)
        {
            //Instanciando propiedades 
            this.navigation = navigation;
            this.page = page;
            RedireccionApartadoCategorias = new Command(RedireccionACategoriasPagina);
            RedireccionApartadoAutores = new Command(RedireccionAAutoresPagina);
            RedireccionApartadoCursos = new Command<CategoriaSlideModelo>(RedireccionACursosPagina);

            IsInitialized = true;

            EstadoActual = LayoutState.Loading;
            EstadoCursos = LayoutState.Loading;
        }

        private void RedireccionAAutoresPagina()
        {
            navigation.PushAsync(new FiltroAutorVista());
        }

        private async Task<List<CategoriaSlideModelo>> DefiniendoCateogriasRandom(int CantidadCategorias)
        {
            var CategoriasRandom = await ServicioCategoria.GetCategoriaSlide();
            
            var SoloAlgunasCategorias = new List<CategoriaSlideModelo>();
            SoloAlgunasCategorias.AddRange(CategoriasRandom.OrderByDescending(e => e.user).Take(CantidadCategorias));

            Shuffle(SoloAlgunasCategorias);
            //Shuffle(CategoriasRandom);
            //int cantidadprimaria = 0;
            SoloAlgunasCategorias.Add(new CategoriaSlideModelo
            {
                IsMoreElement = true
            });
            EstadoActual = LayoutState.Success;
            return SoloAlgunasCategorias;
        }

        private async Task<List<UsuarioModelo>> DefiniendoUsuariosRandos()
        {
            //La api con la que se necesita anidar ya desordena la lista y trae datos random
            var UsuariosRandom = await ServicioUsuarios.GetUsuarioSlide();

            EstadoActual = LayoutState.Success;
            return UsuariosRandom;
        }

        

        private async Task<List<CursosModelo>> DefiniendoCursosRandom(int CantidadCategorias)
        {
            var SoloEstosCursos = new List<CursosModelo>();

            var TodosLosCursos = await ServicioCursos.GetCurso(false);

            Shuffle(TodosLosCursos);
            SoloEstosCursos.AddRange(TodosLosCursos.Take(CantidadCategorias));

            EstadoCursos = LayoutState.Success;
            return SoloEstosCursos;
        }


        private void RedireccionACursosPagina(CategoriaSlideModelo args)
        {
            SearchCourseFilters FiltrosRealizados = new SearchCourseFilters();
            navigation.PushAsync(new FiltroCursoVista(FiltrosRealizados));
        }

        private void RedireccionACategoriasPagina()
        {
            navigation.PushAsync(new FiltroCategoriasVista());
        }

        public async void OnAppearing()
        {
            IsBusy = true;
            if (IsInitialized == true)
            {
                Categorias = new ObservableCollection<CategoriaSlideModelo>(await DefiniendoCateogriasRandom(6));

                Cursos = new ObservableCollection<CursosModelo>(await DefiniendoCursosRandom(4));

                Autores = new ObservableCollection<UsuarioModelo>(await DefiniendoUsuariosRandos());
                
                IsInitialized = false;
            }
            
            IsBusy = false;
        }

    }
}
