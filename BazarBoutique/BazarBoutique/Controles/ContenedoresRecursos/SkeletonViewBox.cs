using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BazarBoutique.Controles.ContenedoresRecursos
{
    public class SkeletonViewBox : BoxView
    {
        public SkeletonViewBox()
        {
            Device.StartTimer(TimeSpan.FromSeconds(0.5), () =>
            {
                this.FadeTo(0.2, 850, Easing.CubicInOut).ContinueWith((x) =>
                {
                    this.FadeTo(7, 850, Easing.CubicInOut);
                });

                return true;
            });
        }
    }
}
