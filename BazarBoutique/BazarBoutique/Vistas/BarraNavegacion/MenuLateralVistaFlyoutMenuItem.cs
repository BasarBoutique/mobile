using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazarBoutique.Vistas.BarraNavegacion
{
    public class MenuLateralVistaFlyoutMenuItem
    {
        public MenuLateralVistaFlyoutMenuItem()
        {
            TargetType = typeof(MenuLateralVistaFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}