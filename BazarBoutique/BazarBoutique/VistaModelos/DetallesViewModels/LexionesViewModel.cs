using BazarBoutique.Modelos;
using BazarBoutique.Services.LessonServices;
using BazarBoutique.Vistas.CarritoVistas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BazarBoutique.VistaModelos.DetallesViewModels
{
    public class LexionesViewModel : BaseViewModel
    {
        private readonly INavigation navigation;
        private readonly ContentPage page;
        ILessonService ServiciosLexiones = DependencyService.Get<ILessonService>();
        private int IdCursoElegido;
        
        private ObservableCollection<LessonsModelo> _lexionesCollecion;

        public ObservableCollection<LessonsModelo> LexionesCollecion
        {
            get { return _lexionesCollecion; }
            set
            {
                _lexionesCollecion = value;
                OnPropertyChanged();
            }
        }
        public Command GestoRefrescamientoCommand { get; set; }
        public Command<Uri> RedirigirAlVideoCommand { get; set; }
        public Command RedireccionCarritoCommand { get; set; }

        private LayoutState _estadoListaLexiones;
        public LayoutState EstadoLexionesLista
        {
            get => _estadoListaLexiones;
            set => SetProperty(ref _estadoListaLexiones, value);
        }

        public LexionesViewModel(INavigation navigation, ContentPage page, int CursoSeleccionado)
        {
            this.IdCursoElegido = CursoSeleccionado;
            this.navigation = navigation;
            this.page = page;
            LexionesCollecion = new ObservableCollection<LessonsModelo>();
            GestoRefrescamientoCommand = new Command(RecargarLexiones);
            RedirigirAlVideoCommand = new Command<Uri>(AbrirVideoLexion);
            RedireccionCarritoCommand = new Command(RedireccionACarritoPagina);

            EstadoLexionesLista = LayoutState.Loading;
        }
        public void RedireccionACarritoPagina()
        {
            navigation.PushAsync(new CarritoVista());
        }

        private async void AbrirVideoLexion(Uri obj)
        {
            await Browser.OpenAsync(obj);
        }

        private async void RecargarLexiones()
        {
            IsLoading = true;
            await TraendoLexiones();
            IsLoading = false;

        }

        public async void OnAppearing()
        {
            IsBusy = true;

            await TraendoLexiones();
            VerificandoUsuario();
            IsBusy = false;
        }

        private async Task TraendoLexiones()
        {
            EstadoLexionesLista = LayoutState.Loading;
            LexionesCollecion.Clear();
            var LexionesFrescas = await ServiciosLexiones.GetLessonsForUser(IdCursoElegido);
            foreach (var lexion in LexionesFrescas)
            {
                LexionesCollecion.Add(lexion);
            }
            EstadoLexionesLista = LayoutState.Success;
        }
    }
}
