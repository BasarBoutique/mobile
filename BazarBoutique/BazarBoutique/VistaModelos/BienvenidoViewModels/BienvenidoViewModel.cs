using BazarBoutique.Modelos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BazarBoutique.VistaModelos.BienvenidoViewModels
{
    public class BienvenidoViewModel
    {
        public ObservableCollection<PresentacionesModelo> Presentaciones { get; set; }
        public BienvenidoViewModel()
        {
            Presentaciones = new ObservableCollection<PresentacionesModelo>();
            AgregandoElementos();
        }

        private void AgregandoElementos()
        {
            Presentaciones.Add(new PresentacionesModelo()
            {
                id = 1,
                NombreDato = "Bienvenido a Bazar boutique",
                NombreImagen = "bienvenida.png"

            });

            Presentaciones.Add(new PresentacionesModelo()
            {
                id = 2,
                NombreDato = "Profesionalizate con nuestros cursos",
                NombreImagen = "cursoonline.png"
            });

            Presentaciones.Add(new PresentacionesModelo()
            {
                id = 3,
                NombreDato = "Explora una cantidad basta de cursos que tenemos para ti",
                NombreImagen = "explorar.png"
            });

        }
    }
}
