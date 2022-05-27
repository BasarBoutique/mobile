using BazarBoutique.Modelos;
using BazarBoutique.Services;
using BazarBoutique.Services.CursoServices;
using BazarBoutique.Vistas.CarritoVistas;
using BazarBoutique.Vistas.DetallesVistas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace BazarBoutique.VistaModelos.CursosPorUsuarioViewModels
{
    public class CursosPorUsuarioViewModel : BaseViewModel
    {
        readonly INavigation navigation;
        readonly ContentPage page;
        ICursoService ServiciosCursos = DependencyService.Get<ICursoService>();

        ObservableCollection<DataCursosPorUsuario> _cursos;
        public ObservableCollection<DataCursosPorUsuario> Cursos
        {
            get { return _cursos; }
            set
            {
                _cursos = value;
                OnPropertyChanged();
            }
        }
        public Command GestoRefrescamientoCommand { get; }
        public Command RedireccionCarritoCommand { get; }
        public Command<DataCursosPorUsuario> RedireccionLexionesCommand { get; }

        private LayoutState _estadoListaCursos;
        public LayoutState EstadoCursosLista
        {
            get => _estadoListaCursos;
            set => SetProperty(ref _estadoListaCursos, value);
        }
        
        public CursosPorUsuarioViewModel(INavigation navigation, ContentPage page)
        {
            //Instanciando propiedades de navegacion
            this.navigation = navigation;
            this.page = page;

            GestoRefrescamientoCommand = new Command(RecargarAutores);
            RedireccionCarritoCommand = new Command(RedireccionACarritoPagina);
            Cursos = new ObservableCollection<DataCursosPorUsuario>();
            RedireccionLexionesCommand = new Command<DataCursosPorUsuario>(RedirigiendApartadoCursos);
            EstadoCursosLista = LayoutState.Loading;
        }
        public void RedireccionACarritoPagina()
        {
            navigation.PushAsync(new CarritoVista());
        }

        private void RedirigiendApartadoCursos(DataCursosPorUsuario obj)
        {
            navigation.PushAsync(new LexionesVista(obj.course.id));
        }

        private async void RecargarAutores()
        {
            IsLoading = true;

            await TraendoCursos();

            IsLoading = false;
        }

        public async void OnAppearing()
        {
            IsBusy = true;

            await TraendoCursos();
            VerificandoUsuario();
            IsBusy = false;
        }

        private async Task TraendoCursos()
        {
            EstadoCursosLista = LayoutState.Loading;
            if (!string.IsNullOrEmpty(SesionServicios.UsuarioGoogle.IdToken) || SesionServicios.apiResponse.success == true)
            {
                Cursos.Clear();
                var DatosCusos = await ServiciosCursos.GetCursosUsuario(SesionServicios.apiResponse.data.access_token);

                foreach (var args in DatosCusos)
                {
                    Cursos.Add(args);
                }
            }
            EstadoCursosLista = LayoutState.Success;
        }
    }
}
