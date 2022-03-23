using BazarBoutique.Modelos;
using BazarBoutique.Services.CategoriaServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BazarBoutique.VistaModelos.CatalogoCursosViewModels
{
    public class CatalogoCursosViewModel : BaseViewModel
    {
        private ObservableCollection<CategoriaModelo> categoriasLista;
        private int _rowHeigth;
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

        public CatalogoCursosViewModel()
        {
            Categorias = new ObservableCollection<CategoriaModelo>();
            Cursos = new ObservableCollection<CursosModelo>();
            
        }

        public async void OnAppearing()
        {
            IsBusy = true;
            //AgregandoDatosCatalogo();
            //Categorias = await CategoryService.GetCategorias();
            AgregandoDatosCursos();
            DefiniendoAltura();
            Categorias = new ObservableCollection<CategoriaModelo>(await CategoryService.GetCategorias());
            IsBusy = false;
        }

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
        private void AgregandoDatosCatalogo()
        {
            //Categorias.Add(new CategoriaModelo
            //{
            //    category_id = 1,
            //    category_tile = "Cursos de Juegos de Azar",
            //    category_ico = "https://gabinetpsicologicmataro.com/wp-content/uploads/2019/03/poker1-OR630-3445.jpg",
            //    is_enabled = true,
            //    created_at = DateTime.Now,
            //    updated_at = DateTime.Now
            //});

            //Categorias.Add(new CategoriaModelo
            //{
            //    category_id = 2,
            //    category_tile = "Cursos de Informatica",
            //    category_ico = "https://concepto.de/wp-content/uploads/2015/08/informatica-1-e1590711788135.jpg",
            //    is_enabled = true,
            //    created_at = DateTime.Now,
            //    updated_at = DateTime.Now
            //});

            //Categorias.Add(new CategoriaModelo
            //{
            //    category_id = 3,
            //    category_tile = "Cursos de Audicion",
            //    category_ico = "https://www.centroauditivocampos.es/wp-content/uploads/2018/09/C%C3%B3mo-podemos-evitar-la-p%C3%A9rdida-de-audici%C3%B3n.jpg",
            //    is_enabled = true,
            //    created_at = DateTime.Now,
            //    updated_at = DateTime.Now
            //});
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
