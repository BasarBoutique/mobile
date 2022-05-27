using BazarBoutique.Modelos;
using BazarBoutique.Services;
using BazarBoutique.Services.CursoServices;
using BazarBoutique.Services.LessonServices;
using BazarBoutique.Vistas.CarritoVistas;
using BazarBoutique.Vistas.DetallesVistas;
using BazarBoutique.Vistas.InicioSesíonVistas;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BazarBoutique.VistaModelos.DetallesViewModels
{
    public class DetallesCursoViewModel : BaseViewModel
    {
        private readonly INavigation navigation;
        private readonly ContentPage page;
        ICursoService ServicioCursos = DependencyService.Get<ICursoService>();
        ILessonService ServiciosLexiones = DependencyService.Get<ILessonService>();
        public CursosModelo CursoSeleccionado = new CursosModelo();
        private CursosModelo _cursodetalles;

        public Command RedireccionLexionesCommand { get; set; }
        public Command AgregandoCarritoCommand { get; set; }
        public Command RedireccionCarritoCommand { get; set; }
        public CursosModelo CursoDetalles
        {
            get => _cursodetalles;
            set
            {
                _cursodetalles = value;
                RaisePropertyChanged("DatosCurso");
            }
        }

        //public CursosModelo CursoDetalles = new CursosModelo();
        private string _imagen;
        public string ImagenProducto
        {
            get => _imagen;
            set => SetProperty(ref _imagen, value);
        }

        //Nombre
        private string _nombre;
        public string NombreProducto
        {
            get => _nombre;
            set => SetProperty(ref _nombre, value);
        }

        //Nombre autor
        private string _autor;
        public string NombreAutor
        {
            get => _autor;
            set => SetProperty(ref _autor, value);
        }

        //Foto autor
        private string _fotoautor;
        public string FotoAutor
        {
            get => _fotoautor;
            set => SetProperty(ref _fotoautor, value);
        }

        //Precio
        private decimal _precio;
        public decimal PrecioCurso
        {
            get => _precio;
            set => SetProperty(ref _precio, value);
        }

        //Categoria perteneciente
        private string _categoriaperteneciente;
        public string CategoriaPerteneciente
        {
            get => _categoriaperteneciente;
            set => SetProperty(ref _categoriaperteneciente, value);
        }

        //Fecha de actualizacion
        private DateTime _fechaactual;
        public DateTime FechaActualizacion
        {
            get => _fechaactual;
            set => SetProperty(ref _fechaactual, value);
        }

        //Cantidad de Sucriptores
        private int _suscriptores;
        public int CantidadSuscriptores
        {
            get => _suscriptores;
            set => SetProperty(ref _suscriptores, value);
        }

        //Cantidad de Sesiones
        private int _sesiones;
        public int Sesiones
        {
            get => _sesiones;
            set => SetProperty(ref _sesiones, value);
        }

        //Suscriptores
        private string _descripcion;
        public string Descripcion
        {
            get => _descripcion;
            set => SetProperty(ref _descripcion, value);
        }

        private bool _botonCarrito;
        public bool BotonCarrito
        {
            get => _botonCarrito;
            set => SetProperty(ref _botonCarrito, value);
        }

        private bool _botonLexiones;
        public bool BotonLexiones
        {
            get => _botonLexiones;
            set => SetProperty(ref _botonLexiones, value);
        }

        public DetallesCursoViewModel(INavigation navigation, ContentPage page, CursosModelo CursoSeleccionado)
        {
            this.navigation = navigation;
            this.page = page;
            this.CursoSeleccionado = CursoSeleccionado;

            RedireccionLexionesCommand = new Command(RedirigendoApartadoLexiones);
            AgregandoCarritoCommand = new Command(AgregandoProductoAlCarro);
            RedireccionCarritoCommand = new Command(RedireccionACarritoPagina);
            //CursoDetalles = new CursosModelo();
        }


        private async void AgregandoProductoAlCarro()
        {
            if(SesionServicios.apiResponse.success == true)
            {
                bool Exist = false;

                foreach (var item in CarroServices.Carritos)
                {
                    if(item.CursoId == CursoDetalles.id)
                    {
                        Exist = true;
                    }
                }

                if (!Exist)
                {
                    CarroServices.Carritos.Add(new CarritoModelo
                    {
                        CursoId = CursoDetalles.id,
                        imagenCurso = CursoDetalles.ImagenCurso,
                        PrecioCurso = CursoDetalles.detail.about.price,
                        TituloCurso = CursoDetalles.title
                    });
                    ContandoProductosEnCarrito();
                }

                else
                    await navigation.PushAsync(new CarritoVista());
                
            }
            else
            {
                await navigation.PushAsync(new LoginVista());
            }
            
        }

        public void RedireccionACarritoPagina()
        {
            navigation.PushAsync(new CarritoVista());
        }

        private void RedirigendoApartadoLexiones()
        {
            if (SesionServicios.apiResponse.success == true)
                navigation.PushAsync(new LexionesVista(CursoSeleccionado.id));
        }

        public async void OnAppearing()
        {
            await TraendoDatosCurso();
            
            ImagenProducto = CursoDetalles.ImagenCurso;
            NombreProducto = CursoDetalles.title;
            NombreAutor = CursoDetalles.detail.author.name;
            FotoAutor = CursoDetalles.detail.author.Foto_Author;

            Sesiones = CursoDetalles.nro_lessons;
            CantidadSuscriptores = CursoDetalles.nro_students;
            CategoriaPerteneciente = CursoDetalles.category.title;

            PrecioCurso = CursoDetalles.detail.about.price;
            Descripcion = CursoDetalles.detail.about.description;
            await VerificandoLaExistenciaDelCurso();
            
            VerificandoUsuario();
        }

        private async Task TraendoDatosCurso()
        {
            CursoDetalles = await ServicioCursos.GetCursoDetail(CursoSeleccionado);
        }

        private async Task VerificandoLaExistenciaDelCurso()
        {
            bool EstaSuscrito = false;
            if (SesionServicios.apiResponse.success == true)
                EstaSuscrito = await ServiciosLexiones.ComprobarSiEstaSuscrito(CursoDetalles.id);

            if (!EstaSuscrito)
            {
                BotonCarrito = true;
                BotonLexiones = false;
            }
            else
            {
                BotonCarrito = false;
                BotonLexiones = true;
            }
        }
    }
}
