using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace DantistApp.Tools
{
    public static class EffectsHelper
    {
        public static DropShadowEffect CreateGlowEffect(Color color)
        {
            DropShadowEffect glowEffect = new DropShadowEffect()
            {
                ShadowDepth = 0,
                Color = color,
                Opacity = 1,
                BlurRadius = 20
            };

            return glowEffect;
        }
    }
}
