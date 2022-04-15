using BazarBoutique.Modelos;
using BazarBoutique.Services.CategoriaServices;
using BazarBoutique.Services.CursoServices;
using BazarBoutique.Vistas.FiltrosVistas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BazarBoutique.VistaModelos.CatalogoCursosViewModels
{
    public class CatalogoCursosViewModel : BaseViewModel
    {
        private readonly INavigation navigation;
        private readonly ContentPage page;
        private bool IsInitialized;
        private ObservableCollection<CategoriaModelo> categoriasLista;
        private ObservableCollection<CursosModelo> cursosLista;
        private int _rowHeigth;

        ICategoryService ServicioCategoria = DependencyService.Get<ICategoryService>();
        ICursoService ServicioCursos = DependencyService.Get<ICursoService>();

        public int rowHeigth
        {
            get { return _rowHeigth; }
            set
            {
                _rowHeigth = value;
                RaisePropertyChanged("rowHeigth");
            }
        }
        public ObservableCollection<CategoriaModelo> Categorias 
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

        public Command RedireccionApartadoCategorias { get; set; }
        public Command<CategoriaModelo> RedireccionApartadoCursos { get; set; }

        public CatalogoCursosViewModel(INavigation navigation, ContentPage page)
        {
            //Instanciando propiedades 
            this.navigation = navigation;
            this.page = page;
            RedireccionApartadoCategorias = new Command(RedireccionACategoriasPagina); 
            RedireccionApartadoCursos = new Command<CategoriaModelo>(RedireccionACursosPagina);

            IsInitialized = true;
        }

        private async Task<List<CategoriaModelo>> DefiniendoCateogriasRandom(int CantidadCategorias)
        {
            var CategoriasRandom = await ServicioCategoria.GetCategoriaSlide();
            

            var SoloAlgunasCategorias = new List<CategoriaModelo>();
            SoloAlgunasCategorias.AddRange(CategoriasRandom.OrderByDescending(e => e.user).Take(CantidadCategorias));


            //Shuffle(CategoriasRandom);
            //int cantidadprimaria = 0;
            //SoloAlgunasCategorias.Add(new CategoriaModelo
            //{
            //    IsMoreElement = true
            //});

            return SoloAlgunasCategorias;
        }

        private async Task<List<CursosModelo>> DefiniendoCursosRandom()
        {
            var SoloEstosCursos = new List<CursosModelo>();

            var TodosLosCursos = await ServicioCursos.GetCurso(false);

            Shuffle(TodosLosCursos);
            SoloEstosCursos.AddRange(TodosLosCursos.Take(6));
            return SoloEstosCursos;
        }


        private void RedireccionACursosPagina(CategoriaModelo args)
        {
            navigation.PushAsync(new FiltroCursoVista());
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
                Categorias = new ObservableCollection<CategoriaModelo>(await DefiniendoCateogriasRandom(6));
                Cursos = new ObservableCollection<CursosModelo>(await DefiniendoCursosRandom());

                DefiniendoAltura();
                IsInitialized = false;
            }
            
            IsBusy = false;
        }

        //Habilitar esto cuando se tenga cursos

        //private async Task<List<CursosModelo>> DefiniendoCursosRandom(int CantidadCursos)
        //{
        //    var CursosRandom = await CursosService.GetCursos();
        //    var SoloAlgunosCursos = new List<CursosModelo>();
        //    Shuffle(CursosRandom);
        //    int cantidadprimaria = 0;
        //    foreach (var arg in CursosRandom)
        //    {
        //        if (cantidadprimaria <= CantidadCursos)
        //        {
        //            SoloAlgunosCursos.Add(arg);
        //            cantidadprimaria++;
        //        }
        //    }

        //    return SoloAlgunosCursos;
        //}

        private void DefiniendoAltura()
        {
            int Elementos = Cursos.Count;
            if (Elementos <= 1)
            {
                rowHeigth = 275;
            }
            else if (Elementos % 2 == 0)
            {
                rowHeigth = (Elementos / 2) * 275;
            }

            else
                rowHeigth = ((Elementos / 2) * 275) + 275 ;
        }

    }
}
