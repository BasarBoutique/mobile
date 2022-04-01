using BazarBoutique.Modelos;
using BazarBoutique.Services.CategoriaServices;
using BazarBoutique.Vistas.FiltrosVistas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private int _rowHeigth;

        ICategoryService ServicioCategoria = DependencyService.Get<ICategoryService>();

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
        public ObservableCollection<CursosModelo> Cursos { get; }
        public Command RedireccionApartadoCategorias { get; set; }
        public Command<CategoriaModelo> RedireccionApartadoCursos { get; set; }

        public CatalogoCursosViewModel(INavigation navigation, ContentPage page)
        {
            //Instanciando propiedades 
            this.navigation = navigation;
            this.page = page;
            RedireccionApartadoCategorias = new Command(RedireccionACategoriasPagina); 
            RedireccionApartadoCursos = new Command<CategoriaModelo>(RedireccionACursosPagina);

            //Categorias = new ObservableCollection<CategoriaModelo> ( async() => DefiniendoCateogriasRandom(5));
            //Categorias = new ObservableCollection<CategoriaModelo>();
            Cursos = new ObservableCollection<CursosModelo>();
            //Func<Task<List<CategoriaModelo>>> func =

            IsInitialized = true;
        }

        private async Task<List<CategoriaModelo>> DefiniendoCateogriasRandom(int CantidadCategorias)
        {
            var CategoriasRandom = await ServicioCategoria.GetCategorias();
            var SoloAlgunasCategorias = new List<CategoriaModelo>();
            Shuffle(CategoriasRandom);
            int cantidadprimaria = 0;
            foreach (var arg in CategoriasRandom)
            {
                if (cantidadprimaria <= CantidadCategorias)
                {
                    SoloAlgunasCategorias.Add(arg);
                    cantidadprimaria++;
                }
            }
            SoloAlgunasCategorias.Add(new CategoriaModelo
            {
                IsMoreElement = true
            });

            return SoloAlgunasCategorias;
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
                Categorias = new ObservableCollection<CategoriaModelo>(await DefiniendoCateogriasRandom(5));
                AgregandoDatosCursos();
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
        private void AgregandoDatosCursos()
        {
            Cursos.Add(new CursosModelo
            {
                course_id = 1,
                course_title = "Curso de C#",
                course_photo = "https://niixer.com/wp-content/uploads/2020/11/csharp.jpg",
                is_enabled = true,
                created_at = DateTime.Now
            });

            Cursos.Add(new CursosModelo
            {
                course_id = 2,
                course_title = "Curso de C++",
                course_photo = "https://i.ytimg.com/vi/dJzLmjSJc2c/maxresdefault.jpg",
                is_enabled = true,
                created_at = DateTime.Now
            });

            Cursos.Add(new CursosModelo
            {
                course_id = 2,
                course_title = "Curso de Javascript",
                course_photo = "https://soyhorizonte.com/wp-content/uploads/2020/10/Javascript-by-SoyHorizonte.jpg",
                is_enabled = true,
                created_at = DateTime.Now
            });

            Cursos.Add(new CursosModelo
            {
                course_id = 5,
                course_title = "Curso de WordPress",
                course_photo = "https://ayudawp.com/wp-content/uploads/2017/09/instalar-wordpress.jpg",
                is_enabled = true,
                created_at = DateTime.Now
            });
        }

    }
}
