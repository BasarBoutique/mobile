using BazarBoutique.Modelos;
using BazarBoutique.Services.CursoServices;
using BazarBoutique.Vistas.FiltrosVistas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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
        public Command VistaFiltroAutorCommand { get; set; }
        public Command VistaFiltroCategoriaCommand { get; set; }
        public Command PaginaAnteriorCommand { get; set; }
        public Command PaginaSiguienteCommand { get; set; }


        ObservableCollection<CursosModelo> _cursos;

        public ObservableCollection<CursosModelo> Cursos
        {
            get { return _cursos; }
            set { _cursos = value;
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
        }

        private void RedireccionFiltroCategorias()
        {
            navigation.PushModalAsync(new FiltroCategoriasVista());
        }

        private void RedireccionFiltroAutores()
        {
            navigation.PushModalAsync(new FiltroAutorVista());

        }

        public async void OnAppearing()
        {
            IsBusy = true;
            Cursos = new ObservableCollection<CursosModelo>(await ServiciosCursos.GetCurso(true));
            IsBusy = false;
        }


    }
}
