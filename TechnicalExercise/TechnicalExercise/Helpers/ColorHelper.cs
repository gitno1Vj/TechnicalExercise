using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TechnicalExercise.Models;

namespace TechnicalExercise.Helpers
{
    internal class ColorHelper
    {
        public static List<ColorModel> GetKnownColors()
        {
            return typeof(Colors).GetProperties(BindingFlags.Static | BindingFlags.Public)
                                 .Select(c => new ColorModel
                                 {
                                     ColorName = c.Name,
                                     ColorBrush = new SolidColorBrush((Color)c.GetValue(null, null))
                                 })
                                 .ToList();
        }
    }
}
