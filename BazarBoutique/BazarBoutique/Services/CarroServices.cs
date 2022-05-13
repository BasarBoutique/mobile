using BazarBoutique.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BazarBoutique.Services
{
    public class CarroServices
    {
        private static List<CarritoModelo> carritos;
        public static List<CarritoModelo> Carritos
        {
            get
            {
                if (carritos == null) carritos = new List<CarritoModelo>();

                return carritos;
            }
            set
            {
                carritos = value;
            }
        }
    }
}
