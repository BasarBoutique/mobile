using BazarBoutique.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BazarBoutique.Services
{
    public class FiltrosAlmacenados
    {
        private static List<FiltroPorNombreId> _almacenamientoFiltros;

        public static List<FiltroPorNombreId> AlmacenamientoFiltros
        {
            get 
            { 
                if (_almacenamientoFiltros == null) _almacenamientoFiltros = new List<FiltroPorNombreId>();

                return _almacenamientoFiltros;
            }
            set 
            {
                _almacenamientoFiltros = value;
            }
        }



        //public static List<FiltroPorNombreId> AlmacenamientoFiltros = new List<FiltroPorNombreId>();
    }
}
